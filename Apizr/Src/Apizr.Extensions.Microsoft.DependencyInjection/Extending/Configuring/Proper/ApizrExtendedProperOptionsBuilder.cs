using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Extending.Configuring.Shared;
using Apizr.Logging;
using Apizr.Requesting;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Extending.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for extended registrations
    /// </summary>
    public class ApizrExtendedProperOptionsBuilder : IApizrExtendedProperOptionsBuilder, IApizrInternalRegistrationOptionsBuilder
    {
        protected readonly ApizrExtendedProperOptions Options;

        internal ApizrExtendedProperOptionsBuilder(ApizrExtendedProperOptions properOptions)
        {
            Options = properOptions;
        }
        
        /// <inheritdoc />
        IApizrExtendedProperOptions IApizrExtendedProperOptionsBuilder.ApizrOptions => Options;

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(string baseAddress)
            => WithBaseAddress(_ => baseAddress);

        /// <inheritdoc />
        IApizrExtendedProperOptionsBuilder
            IApizrGlobalSharedRegistrationOptionsBuilderBase<IApizrExtendedProperOptions,
                IApizrExtendedProperOptionsBuilder>.WithBaseAddress(string baseAddress, ApizrDuplicateStrategy strategy)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.BaseAddressFactory ??= _ => baseAddress;
                    break;
                default:
                    Options.BaseAddressFactory = _ => baseAddress;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Uri baseAddress)
            => WithBaseAddress(_ => baseAddress);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithConfiguration(IConfiguration configuration)
            => WithConfiguration(configuration?.GetSection("Apizr"));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithConfiguration(IConfigurationSection configurationSection)
        {
            if (configurationSection is not null)
            {
                var isApizrSection = configurationSection.Key == "Apizr";
                var apiName = Options.CrudApiEntityType?.Name ?? Options.WebApiType.Name;
                var configs = configurationSection.GetChildren().Where(config =>
                    !isApizrSection || config.Key == apiName);
                foreach (var config in configs)
                {
                    switch (config.Key)
                    {
                        case "BaseAddress":
                            WithBaseAddress(config.Value);
                            break;
                        case "BasePath":
                            WithBasePath(config.Value);
                            break;
                        case "OperationTimeout":
                            WithOperationTimeout(TimeSpan.Parse(config.Value!));
                            break;
                        case "RequestTimeout":
                            WithRequestTimeout(TimeSpan.Parse(config.Value!));
                            break;
                        case "HttpTracerMode":
                            Options.HttpTracerModeFactory = _ => (HttpTracerMode)Enum.Parse(typeof(HttpTracerMode), config.Value!);
                            break;
                        case "TrafficVerbosity":
                            Options.TrafficVerbosityFactory = _ => (HttpMessageParts)Enum.Parse(typeof(HttpMessageParts), config.Value!);
                            break;
                        case "LogLevels":
                            Options.LogLevelsFactory = _ => config.GetChildren().Select(c => (LogLevel)Enum.Parse(typeof(LogLevel), c.Value!)).ToArray();
                            break;
                        case "Headers":
                            WithHeaders(config.GetChildren().Select(c => c.Value!).ToList());
                            break;
                        case "LoggedHeadersRedactionNames":
                            WithLoggedHeadersRedactionNames(config.GetChildren().Select(c => c.Value!).ToList());
                            break;
                        case "ContinueOnCapturedContext":
                            WithResilienceContextOptions(options => options.ContinueOnCapturedContext(bool.Parse(config.Value!)));
                            break;
                        case "ReturnContextToPoolOnComplete":
                            WithResilienceContextOptions(options => options.ReturnToPoolOnComplete(bool.Parse(config.Value!)));
                            break;
                        case "ResiliencePipelineKeys":
                            WithResiliencePipelineKeys(config.GetChildren().Select(c => c.Value!).ToArray());
                            break;
                        case "ResiliencePipelineOptions":
                        {
                            var resiliencePipelineOptions = config.GetChildren().ToList();
                            foreach (var resiliencePipelineOption in resiliencePipelineOptions)
                            {
                                if (!ApizrRequestMethod.TryParse(resiliencePipelineOption.Key, out var requestMethod))
                                    continue;

                                var resiliencePipelineKeys = resiliencePipelineOption.GetChildren().Select(c => c.Value!).ToArray();
                                WithResiliencePipelineKeys(resiliencePipelineKeys, [requestMethod]);
                            }

                            break;
                        }
                        case "Caching":
                        {
                            var cacheSection = config.GetChildren().ToList();
                            var modeValue = cacheSection.FirstOrDefault(c => c.Key == "Mode")?.Value;
                            var mode = !string.IsNullOrEmpty(modeValue) ? (CacheMode)Enum.Parse(typeof(CacheMode), modeValue) : CacheMode.GetAndFetch;
                            var lifeSpanValue = cacheSection.FirstOrDefault(c => c.Key == "LifeSpan")?.Value;
                            var lifeSpan = !string.IsNullOrEmpty(lifeSpanValue) ? TimeSpan.Parse(lifeSpanValue) : TimeSpan.Zero;
                            var shouldInvalidateOnErrorValue = cacheSection.FirstOrDefault(c => c.Key == "ShouldInvalidateOnError")?.Value;
                            var shouldInvalidateOnError = !string.IsNullOrEmpty(shouldInvalidateOnErrorValue) && bool.Parse(shouldInvalidateOnErrorValue);
                            WithCaching(mode, lifeSpan, shouldInvalidateOnError);
                            break;
                        }
                        default:
                            if (!config.GetChildren().Any())
                                throw new ArgumentOutOfRangeException(config.Key, $"Apizr does not handle any {config.Key} option. Make sure that your key target an option that Apizr could configure.");

                            WithConfiguration(config);
                            break;
                    }
                }
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory)
        {
            Options.BaseUriFactory = serviceProvider =>
                Uri.TryCreate(baseAddressFactory.Invoke(serviceProvider), UriKind.RelativeOrAbsolute, out var baseUri)
                    ? baseUri
                    : null;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory)
        {
            Options.BaseUriFactory = baseAddressFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBasePath(string basePath)
            => WithBasePath(_ => basePath);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBasePath(Func<IServiceProvider, string> basePathFactory)
        {
            Options.BasePathFactory = basePathFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(_ => httpClientHandler);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpClientHandler(Func<IServiceProvider, HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder ConfigureHttpClientBuilder(
            Action<IHttpClientBuilder> httpClientBuilder, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HttpClientBuilder ??= httpClientBuilder;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HttpClientBuilder = httpClientBuilder;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HttpClientBuilder == null)
                    {
                        Options.HttpClientBuilder = httpClientBuilder;
                    }
                    else
                    {
                        Options.HttpClientBuilder += httpClientBuilder.Invoke;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithDelegatingHandler<THandler>(Func<IServiceProvider, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((serviceProvider, _) => delegatingHandlerFactory.Invoke(serviceProvider), strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler(serviceProvider.GetService<ILogger>(),
                    options, 
                    refreshTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory)
            where TAuthenticationHandler : AuthenticationHandlerBase
            => WithDelegatingHandler(authenticationHandlerFactory);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Expression<Func<TSettingsService, string>> tokenProperty,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
            => WithDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService, TTokenService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty,
                    serviceProvider.GetRequiredService<TTokenService>, refreshTokenMethod));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, string>> tokenProperty)
            => WithDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty,
                    _ => Task.FromResult(
                        tokenProperty.Compile()(serviceProvider.GetRequiredService<TSettingsService>()))));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty, refreshTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHeaders(IList<string> headers,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrRegistrationMode behavior = ApizrRegistrationMode.Set)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.Headers[behavior] ??= headers;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.Headers.TryGetValue(behavior, out var value))
                    {
                        headers?.ToList().ForEach(header => value.Add(header));
                    }
                    else
                    {
                        Options.Headers[behavior] = headers;
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.Headers[behavior] = headers;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHeaders(Func<IServiceProvider, IList<string>> headersFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api, 
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HeadersExtendedFactories[(mode, scope)] ??= serviceProvider => () => headersFactory(serviceProvider);
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HeadersExtendedFactories.TryGetValue((mode, scope), out var previous))
                    {
                        Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () => previous(serviceProvider).Invoke().Concat(headersFactory(serviceProvider)).ToList();
                    }
                    else
                    {
                        Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () => headersFactory(serviceProvider);
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () => headersFactory(serviceProvider);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHeaders<TSettingsService>(
            Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api, ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            var headersFactories = headerProperties.Select(exp => exp.Compile());

            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HeadersExtendedFactories[(mode, scope)] ??= serviceProvider => () =>
                    {
                        var settingsService = serviceProvider.GetRequiredService<TSettingsService>();
                        return headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)).ToList();
                    };
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HeadersExtendedFactories.TryGetValue((mode, scope), out var previous))
                    {
                        Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () =>
                        {
                            var settingsService = serviceProvider.GetRequiredService<TSettingsService>();
                            return previous(serviceProvider).Invoke()
                                .Concat(headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)))
                                .ToList();
                        };
                    }
                    else
                    {
                        Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () =>
                        {
                            var settingsService = serviceProvider.GetRequiredService<TSettingsService>();
                            return headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)).ToList();
                        };
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () =>
                    {
                        var settingsService = serviceProvider.GetRequiredService<TSettingsService>();
                        return headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)).ToList();
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithOperationTimeout(TimeSpan timeout)
            => WithOperationTimeout(_ => timeout);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithOperationTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory)
        {
            Options.OperationTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithRequestTimeout(TimeSpan timeout)
            => WithRequestTimeout(_ => timeout);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithRequestTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory)
        {
            Options.RequestTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithDelegatingHandler<THandler>(THandler delegatingHandler,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((_, _) => delegatingHandler, strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithDelegatingHandler<THandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.DelegatingHandlersExtendedFactories[typeof(THandler)] ??= delegatingHandlerFactory;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    Options.DelegatingHandlersExtendedFactories[typeof(THandler)] = delegatingHandlerFactory;
                    break;
                case ApizrDuplicateStrategy.Replace:
                {
                    Options.DelegatingHandlersExtendedFactories.Clear();
                    Options.DelegatingHandlersExtendedFactories[typeof(THandler)] = delegatingHandlerFactory;
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpMessageHandler<THandler>(Func<IServiceProvider, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((serviceProvider, _) => httpMessageHandlerFactory.Invoke(serviceProvider));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpMessageHandler<THandler>(THandler httpMessageHandler) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((_, _) => httpMessageHandler);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpMessageHandler<THandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
        {
            if (typeof(DelegatingHandler).IsAssignableFrom(typeof(THandler)))
                return WithDelegatingHandler((serviceProvider, options) => httpMessageHandlerFactory.Invoke(serviceProvider, options) as DelegatingHandler);

            Options.HttpMessageHandlerFactory = httpMessageHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching(Action<ApizrException> onException,
            bool letThrowOnExceptionWithEmptyCache = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.OnException ??= onException;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.OnException = onException;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.OnException == null)
                    {
                        Options.OnException = onException;
                    }
                    else
                    {
                        Options.OnException += onException.Invoke;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            Options.LetThrowOnExceptionWithEmptyCache = letThrowOnExceptionWithEmptyCache;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching<TResult>(Action<ApizrException<TResult>> onException,
            bool letThrowOnExceptionWithEmptyCache = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(ex => onException.Invoke((ApizrException<TResult>)ex), letThrowOnExceptionWithEmptyCache,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHandlerParameter(string key, object value)
        {
            Options.HandlersParameters[key] = value;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, TValue value)
            => WithResilienceProperty(key, _ => value);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, Func<IServiceProvider, TValue> valueFactory)
        {
            ((IApizrExtendedSharedOptions)Options).ResiliencePropertiesExtendedFactories[key.Key] = serviceProvider => valueFactory(serviceProvider);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels)
            => WithLogging(_ => httpTracerMode, _ => trafficVerbosity, _ => logLevels);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLogging(
            Func<IServiceProvider, (HttpTracerMode, HttpMessageParts, LogLevel[])> loggingConfigurationFactory)
            => WithLogging(serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item1,
                serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item2,
                serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item3);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLogging(
            Func<IServiceProvider, HttpTracerMode> httpTracerModeFactory,
            Func<IServiceProvider, HttpMessageParts> trafficVerbosityFactory,
            Func<IServiceProvider, LogLevel[]> logLevelsFactory)
        {
            Options.HttpTracerModeFactory = httpTracerModeFactory;
            Options.TrafficVerbosityFactory = trafficVerbosityFactory;
            Options.LogLevelsFactory = logLevelsFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResilienceContextOptions(Action<IApizrResilienceContextOptionsBuilder> contextOptionsBuilder)
        {
            var options = Options as IApizrGlobalSharedOptionsBase;
            if (options.ContextOptionsBuilder == null)
            {
                options.ContextOptionsBuilder = contextOptionsBuilder;
            }
            else
            {
                options.ContextOptionsBuilder += contextOptionsBuilder.Invoke;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLoggedHeadersRedactionNames(IEnumerable<string> redactedLoggedHeaderNames,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
        {
            var sensitiveHeaders = new HashSet<string>(redactedLoggedHeaderNames, StringComparer.OrdinalIgnoreCase);

            return WithLoggedHeadersRedactionRule(header => sensitiveHeaders.Contains(header), strategy);
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLoggedHeadersRedactionRule(
            Func<string, bool> shouldRedactHeaderValue, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.ShouldRedactHeaderValue ??= shouldRedactHeaderValue;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.ShouldRedactHeaderValue == null)
                    {
                        Options.ShouldRedactHeaderValue = shouldRedactHeaderValue;
                    }
                    else
                    {
                        var previous = Options.ShouldRedactHeaderValue;
                        Options.ShouldRedactHeaderValue = header => previous(header) || shouldRedactHeaderValue(header);
                    }

                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.ShouldRedactHeaderValue = shouldRedactHeaderValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResiliencePipelineKeys(string[] resiliencePipelineKeys,
            IEnumerable<ApizrRequestMethod> methodScope = null,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add)
        {
            methodScope ??= [ApizrRequestMethod.All];

            switch (duplicateStrategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    if (Options.ResiliencePipelineOptions.Count == 0)
                        Options.ResiliencePipelineOptions[ApizrConfigurationSource.ProperOption] = methodScope
                            .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                            .Cast<ResiliencePipelineAttributeBase>()
                            .ToArray();
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.ResiliencePipelineOptions.TryGetValue(ApizrConfigurationSource.ProperOption, out var attributes))
                    {
                        foreach (var method in methodScope)
                        {
                            var attribute = attributes.FirstOrDefault(attribute => attribute.RequestMethod == method);
                            if (attribute != null)
                                attribute.RegistryKeys = attribute.RegistryKeys.Union(resiliencePipelineKeys).ToArray();
                            else
                                attributes = attributes.Concat([new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method }]).ToArray();
                        }

                        Options.ResiliencePipelineOptions[ApizrConfigurationSource.ProperOption] = attributes;
                    }
                    else
                    {
                        Options.ResiliencePipelineOptions[ApizrConfigurationSource.ProperOption] = methodScope
                            .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                            .Cast<ResiliencePipelineAttributeBase>()
                            .ToArray();
                    }

                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.ResiliencePipelineOptions.Clear();
                    Options.ResiliencePipelineOptions[ApizrConfigurationSource.ProperOption] = methodScope
                        .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                        .Cast<ResiliencePipelineAttributeBase>()
                        .ToArray();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(duplicateStrategy), duplicateStrategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithCaching(CacheMode mode = CacheMode.GetAndFetch, TimeSpan? lifeSpan = null,
            bool shouldInvalidateOnError = false)
        {
            Options.CacheOptions[ApizrConfigurationSource.ProperOption] = new CacheAttribute(mode, lifeSpan, shouldInvalidateOnError);

            return this;
        }

        #region Internal

        void IApizrInternalOptionsBuilder.SetHandlerParameter(string key, object value) => WithHandlerParameter(key, value);

        void IApizrInternalRegistrationOptionsBuilder.SetPrimaryHttpMessageHandler(Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> primaryHandlerFactory)
        {
            Options.PrimaryHandlerFactory = primaryHandlerFactory;
        }

        /// <inheritdoc />
        void IApizrInternalRegistrationOptionsBuilder.AddDelegatingHandler<THandler>(Func<IApizrManagerOptionsBase, THandler> handlerFactory) 
            => WithDelegatingHandler((_, options) => handlerFactory.Invoke(
                options));

        #endregion
    }
}
