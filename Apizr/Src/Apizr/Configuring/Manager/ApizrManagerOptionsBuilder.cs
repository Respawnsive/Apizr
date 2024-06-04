using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Requesting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Refit;

namespace Apizr.Configuring.Manager
{
    /// <summary>
    /// Builder options available for static registrations
    /// </summary>
    public class ApizrManagerOptionsBuilder : IApizrManagerOptionsBuilder, IApizrInternalRegistrationOptionsBuilder
    {
        /// <summary>
        /// The options
        /// </summary>
        protected readonly ApizrManagerOptions Options;

        internal ApizrManagerOptionsBuilder(ApizrManagerOptions apizrOptions)
        {
            Options = apizrOptions;
        }

        /// <inheritdoc />
        IApizrManagerOptions IApizrManagerOptionsBuilder.ApizrOptions => Options;

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithConfiguration(IConfiguration configuration)
            => WithConfiguration(configuration?.GetSection("Apizr"));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithConfiguration(IConfigurationSection configurationSection)
        {
            if (configurationSection is not null)
            {
                var isApizrSection = configurationSection.Key == "Apizr";
                var apiName = typeof(ICrudApi<,,,>).IsAssignableFromGenericType(Options.WebApiType)
                    ? Options.WebApiType.GetGenericArguments().First().Name
                    : Options.WebApiType.Name;
                var configs = configurationSection.GetChildren().Where(config =>
                    !isApizrSection || config.Key == "Common" || config.Key == apiName);
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
                            Options.HttpTracerModeFactory = () => (HttpTracerMode)Enum.Parse(typeof(HttpTracerMode), config.Value!);
                            break;
                        case "TrafficVerbosity":
                            Options.TrafficVerbosityFactory = () => (HttpMessageParts)Enum.Parse(typeof(HttpMessageParts), config.Value!);
                            break;
                        case "LogLevels":
                            Options.LogLevelsFactory = () => config.GetChildren().Select(c => (LogLevel)Enum.Parse(typeof(LogLevel), c.Value!)).ToArray();
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
        public IApizrManagerOptionsBuilder WithBaseAddress(string baseAddress)
            => WithBaseAddress(() => baseAddress);

        /// <inheritdoc />
        IApizrManagerOptionsBuilder
            IApizrGlobalSharedRegistrationOptionsBuilderBase<IApizrManagerOptions, IApizrManagerOptionsBuilder>.
            WithBaseAddress(string baseAddress, ApizrDuplicateStrategy strategy)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.BaseAddressFactory ??= () => baseAddress;
                    break;
                default:
                    Options.BaseAddressFactory = () => baseAddress;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory)
        {
            Options.BaseAddressFactory = baseAddressFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithBaseAddress(Uri baseAddress)
            => WithBaseAddress(() => baseAddress);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory)
        {
            Options.BaseUriFactory = baseAddressFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithBasePath(string basePath)
            => WithBasePath(() => basePath);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithBasePath(Func<string> basePathFactory)
        {
            Options.BasePathFactory = basePathFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(() => httpClientHandler);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHttpClientHandler(Func<HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder ConfigureHttpClient(Action<HttpClient> configureHttpClient,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HttpClientConfigurationBuilder ??= configureHttpClient;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HttpClientConfigurationBuilder = configureHttpClient;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HttpClientConfigurationBuilder == null)
                    {
                        Options.HttpClientConfigurationBuilder = configureHttpClient;
                    }
                    else
                    {
                        Options.HttpClientConfigurationBuilder += configureHttpClient.Invoke;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<ILogger, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory)
            where TAuthenticationHandler : AuthenticationHandlerBase
            => WithDelegatingHandler(authenticationHandlerFactory);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((logger, options) =>
                new AuthenticationHandler(logger, options, refreshTokenFactory));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty)
            => WithAuthenticationHandler(() => settingsService, tokenProperty,
                _ => Task.FromResult(tokenProperty.Compile()(settingsService)));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => WithAuthenticationHandler(() => settingsService, tokenProperty, refreshTokenFactory);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TSettingsService>(Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> tokenProperty)
            => WithAuthenticationHandler(settingsServiceFactory, tokenProperty,
                _ => Task.FromResult(tokenProperty.Compile()(settingsServiceFactory.Invoke())));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((logger, options) =>
                new AuthenticationHandler<TSettingsService>(logger, options, settingsServiceFactory, tokenProperty,
                    refreshTokenFactory));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty, TTokenService tokenService,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
            => WithAuthenticationHandler(() => settingsService, tokenProperty, () => tokenService, refreshTokenMethod);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty,
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
            => WithDelegatingHandler((logger, options) =>
                new AuthenticationHandler<TSettingsService, TTokenService>(logger,
                    options,
                    settingsServiceFactory, tokenProperty,
                    tokenServiceFactory, refreshTokenMethod));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithDelegatingHandler<THandler>(THandler delegatingHandler,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((_, _) => delegatingHandler, strategy);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithDelegatingHandler<THandler>(
            Func<ILogger, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((logger, _) => delegatingHandlerFactory(logger), strategy);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithDelegatingHandler<THandler>(
            Func<ILogger, IApizrManagerOptionsBase, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.DelegatingHandlersFactories[typeof(THandler)] ??= delegatingHandlerFactory;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    Options.DelegatingHandlersFactories[typeof(THandler)] = delegatingHandlerFactory;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    {
                        Options.DelegatingHandlersFactories.Clear();
                        Options.DelegatingHandlersFactories[typeof(THandler)] = delegatingHandlerFactory;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHttpMessageHandler<THandler>(THandler httpMessageHandler) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((_, _) => httpMessageHandler);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHttpMessageHandler<THandler>(Func<ILogger, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((logger, _) => httpMessageHandlerFactory(logger));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHttpMessageHandler<THandler>(Func<ILogger, IApizrManagerOptionsBase, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
        {
            if (httpMessageHandlerFactory is DelegatingHandler delegatingHandler)
                return WithDelegatingHandler(delegatingHandler);

            Options.HttpMessageHandlerFactory = httpMessageHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithRefitSettings(RefitSettings refitSettings)
            => WithRefitSettings(() => refitSettings);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory)
        {
            Options.RefitSettingsFactory = refitSettingsFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithResiliencePipelineRegistry(
            ResiliencePipelineRegistry<string> resiliencePipelineRegistry)
            => WithResiliencePipelineRegistry(() => resiliencePipelineRegistry);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithResiliencePipelineRegistry(Func<ResiliencePipelineRegistry<string>> resiliencePipelineRegistryFactory)
        {
            Options.ResiliencePipelineRegistryFactory = resiliencePipelineRegistryFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithConnectivityHandler(Func<bool> connectivityCheckingFunction)
            => WithConnectivityHandler(() => new DefaultConnectivityHandler(connectivityCheckingFunction));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithConnectivityHandler(IConnectivityHandler connectivityHandler)
            => WithConnectivityHandler(() => connectivityHandler);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithConnectivityHandler(Func<IConnectivityHandler> connectivityHandlerFactory)
        {
            Options.ConnectivityHandlerFactory = connectivityHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithCacheHandler(ICacheHandler cacheHandler)
            => WithCacheHandler(() => cacheHandler);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithCacheHandler(Func<ICacheHandler> cacheHandlerFactory)
        {
            Options.CacheHandlerFactory = cacheHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithExCatching(Action<ApizrException> onException,
            bool letThrowOnExceptionWithEmptyCache = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
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
        public IApizrManagerOptionsBuilder WithExCatching<TResult>(Action<ApizrException<TResult>> onException,
            bool letThrowOnExceptionWithEmptyCache = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(ex => onException.Invoke((ApizrException<TResult>)ex), letThrowOnExceptionWithEmptyCache,
                strategy);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHandlerParameter(string key, object value)
        {
            Options.HandlersParameters[key] = value;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All,
            params LogLevel[] logLevels)
            => WithLogging(() => httpTracerMode, () => trafficVerbosity, () => logLevels);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithLogging(Func<HttpTracerMode> httpTracerModeFactory,
            Func<HttpMessageParts> trafficVerbosityFactory, Func<LogLevel[]> logLevelsFactory)
        {
            Options.HttpTracerModeFactory = httpTracerModeFactory;
            Options.TrafficVerbosityFactory = trafficVerbosityFactory;
            Options.LogLevelsFactory = logLevelsFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithLogging(
            Func<(HttpTracerMode, HttpMessageParts, LogLevel[])> loggingConfigurationFactory)
            => WithLogging(() => loggingConfigurationFactory.Invoke().Item1,
                () => loggingConfigurationFactory.Invoke().Item2,
                () => loggingConfigurationFactory.Invoke().Item3);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHeaders(IList<string> headers,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.Headers[mode] ??= headers;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.Headers.TryGetValue(mode, out var value))
                    {
                        headers?.ToList().ForEach(header => value.Add(header));
                    }
                    else
                    {
                        Options.Headers[mode] = headers;
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.Headers[mode] = headers;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHeaders(Func<IList<string>> headersFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HeadersFactories[(mode, scope)] ??= headersFactory;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HeadersFactories.TryGetValue((mode, scope), out var previous))
                    {
                        Options.HeadersFactories[(mode, scope)] = () => previous().Concat(headersFactory()).ToList();
                    }
                    else
                    {
                        Options.HeadersFactories[(mode, scope)] = headersFactory;
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HeadersFactories[(mode, scope)] = headersFactory;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHeaders<TSettingsService>(TSettingsService settingsService,
            Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
            => WithHeaders(() => settingsService, headerProperties, strategy, scope, mode);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHeaders<TSettingsService>(Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api, 
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            var settingsService = settingsServiceFactory.Invoke();
            var headersFactories = headerProperties.Select(exp => exp.Compile());

            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HeadersFactories[(mode, scope)] ??= () => headersFactories
                        .Select(headerFactory => headerFactory.Invoke(settingsService))
                        .ToList();
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HeadersFactories.TryGetValue((mode, scope), out var previous))
                    {
                        Options.HeadersFactories[(mode, scope)] = () => previous()
                            .Concat(headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)))
                            .ToList();
                    }
                    else
                    {
                        Options.HeadersFactories[(mode, scope)] = () => headersFactories
                            .Select(headerFactory => headerFactory.Invoke(settingsService))
                            .ToList();
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HeadersFactories[(mode, scope)] = () => headersFactories
                        .Select(headerFactory => headerFactory.Invoke(settingsService))
                        .ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithOperationTimeout(TimeSpan timeout)
            => WithOperationTimeout(() => timeout);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithOperationTimeout(Func<TimeSpan> timeoutFactory)
        {
            Options.OperationTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithRequestTimeout(TimeSpan timeout)
            => WithRequestTimeout(() => timeout);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithRequestTimeout(Func<TimeSpan> timeoutFactory)
        {
            Options.RequestTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, TValue value)
            => WithResilienceProperty(key, () => value);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, Func<TValue> valueFactory)
        {
            ((IApizrGlobalSharedOptionsBase)Options).ResiliencePropertiesFactories[key.Key] = () => valueFactory();

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
            => WithLoggerFactory(() => loggerFactory);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithLoggerFactory(Func<ILoggerFactory> loggerFactory)
        {
            Options.LoggerFactoryFactory = loggerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithMappingHandler(IMappingHandler mappingHandler)
            => WithMappingHandler(() => mappingHandler);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithMappingHandler(Func<IMappingHandler> mappingHandlerFactory)
        {
            Options.MappingHandlerFactory = mappingHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithResilienceContextOptions(Action<IApizrResilienceContextOptionsBuilder> contextOptionsBuilder)
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
        public IApizrManagerOptionsBuilder WithLoggedHeadersRedactionNames(IEnumerable<string> redactedLoggedHeaderNames,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
        {
            var sensitiveHeaders = new HashSet<string>(redactedLoggedHeaderNames, StringComparer.OrdinalIgnoreCase);

            return WithLoggedHeadersRedactionRule(header => sensitiveHeaders.Contains(header), strategy);
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithLoggedHeadersRedactionRule(Func<string, bool> shouldRedactHeaderValue,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
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
        public IApizrManagerOptionsBuilder WithResiliencePipelineKeys(string[] resiliencePipelineKeys,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    if (Options.ResiliencePipelineKeys.Count == 0)
                        Options.ResiliencePipelineKeys[ApizrConfigurationSource.ManagerOptions] = resiliencePipelineKeys;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.ResiliencePipelineKeys.TryGetValue(ApizrConfigurationSource.ManagerOptions, out var keys))
                    {
                        Options.ResiliencePipelineKeys[ApizrConfigurationSource.ManagerOptions] = keys.Union(resiliencePipelineKeys).ToArray();
                    }
                    else
                    {
                        Options.ResiliencePipelineKeys[ApizrConfigurationSource.ManagerOptions] = resiliencePipelineKeys;
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.ResiliencePipelineKeys.Clear();
                    Options.ResiliencePipelineKeys[ApizrConfigurationSource.ManagerOptions] = resiliencePipelineKeys;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

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
            => WithDelegatingHandler((_, opt) => handlerFactory.Invoke(opt));

        #endregion
    }
}
