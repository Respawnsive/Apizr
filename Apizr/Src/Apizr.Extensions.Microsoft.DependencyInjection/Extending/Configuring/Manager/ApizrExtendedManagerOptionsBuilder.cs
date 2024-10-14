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
using Apizr.Configuring.Request;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Shared;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Refit;

namespace Apizr.Extending.Configuring.Manager
{
    /// <summary>
    /// Builder options available for extended registrations
    /// </summary>
    public class ApizrExtendedManagerOptionsBuilder : IApizrExtendedManagerOptionsBuilder, IApizrInternalRegistrationOptionsBuilder
    {
        protected readonly ApizrExtendedManagerOptions Options;

        public ApizrExtendedManagerOptionsBuilder(ApizrExtendedManagerOptions apizrOptions)
        {
            Options = apizrOptions;
        }

        /// <inheritdoc />
        IApizrExtendedManagerOptions IApizrExtendedManagerOptionsBuilder.ApizrOptions => Options;

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithConfiguration(IConfiguration configuration)
            => WithConfiguration(configuration?.GetSection("Apizr"), null);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithConfiguration(IConfigurationSection configurationSection)
            => WithConfiguration(configurationSection, null);

        private IApizrExtendedManagerOptionsBuilder WithConfiguration(IConfigurationSection configurationSection, string requestName)
        {
            if (configurationSection is not null)
            {
                var isRequestOptions = !string.IsNullOrWhiteSpace(requestName);
                var apiName = Options.CrudApiEntityType?.Name ?? Options.WebApiType.Name;
                var configs = configurationSection.GetChildren().Where(config =>
                    config.Key == "Apizr" ||
                    config.Path.Contains("Apizr:CommonOptions") ||
                    config.Key == "ProperOptions" || config.Path.Contains($"Apizr:ProperOptions:{apiName}") ||
                    config.Key == "RequestOptions" || config.Path.Contains($"Apizr:ProperOptions:{apiName}:RequestOptions") ||
                    Constants.ConfigurableSettings.Contains(config.Key));
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
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithOperationTimeout(TimeSpan.Parse(config.Value!)));
                            else
                                WithOperationTimeout(TimeSpan.Parse(config.Value!));
                            break;
                        case "RequestTimeout":
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithRequestTimeout(TimeSpan.Parse(config.Value!)));
                            else
                                WithRequestTimeout(TimeSpan.Parse(config.Value!));
                            break;
                        case "Logging":
                            {
                                var logSection = config.GetChildren().ToList();
                                var httpTracerModeValue = logSection.FirstOrDefault(c => c.Key == "HttpTracerMode")?.Value;
                                var httpTracerMode = !string.IsNullOrEmpty(httpTracerModeValue) ? (HttpTracerMode)Enum.Parse(typeof(HttpTracerMode), httpTracerModeValue) : Constants.DefaultHttpTracerMode;
                                var trafficVerbosityValue = logSection.FirstOrDefault(c => c.Key == "TrafficVerbosity")?.Value;
                                var trafficVerbosity = !string.IsNullOrEmpty(trafficVerbosityValue) ? (HttpMessageParts)Enum.Parse(typeof(HttpMessageParts), trafficVerbosityValue) : Constants.DefaultTrafficVerbosity;
                                var logLevelsValue = logSection.FirstOrDefault(c => c.Key == "LogLevels");
                                var logLevels = logLevelsValue?.GetChildren().Select(c => (LogLevel)Enum.Parse(typeof(LogLevel), c.Value!)).ToArray() ?? Constants.DefaultLogLevels;

                                if (isRequestOptions)
                                    WithRequestOptions(requestName, requestOptions => requestOptions.WithLogging(httpTracerMode, trafficVerbosity, logLevels));
                                else
                                    WithLogging(httpTracerMode, trafficVerbosity, logLevels);

                                break;
                            }
                        case "Headers":
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithHeaders(config.GetChildren().Select(c => c.Value!).ToList()));
                            else
                                WithHeaders(config.GetChildren().Select(c => c.Value!).ToList());
                            break;
                        case "LoggedHeadersRedactionNames":
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithLoggedHeadersRedactionNames(config.GetChildren().Select(c => c.Value!).ToList()));
                            else
                                WithLoggedHeadersRedactionNames(config.GetChildren().Select(c => c.Value!).ToList());
                            break;
                        case "ContinueOnCapturedContext":
                            WithResilienceContextOptions(options => options.ContinueOnCapturedContext(bool.Parse(config.Value!)));
                            break;
                        case "ReturnContextToPoolOnComplete":
                            WithResilienceContextOptions(options => options.ReturnToPoolOnComplete(bool.Parse(config.Value!)));
                            break;
                        case "ResiliencePipelineKeys":
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithResiliencePipelineKeys(config.GetChildren().Select(c => c.Value!).ToArray()));
                            else
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
                                var mode = !string.IsNullOrEmpty(modeValue) ? (CacheMode)Enum.Parse(typeof(CacheMode), modeValue) : CacheMode.FetchOrGet;
                                var lifeSpanValue = cacheSection.FirstOrDefault(c => c.Key == "LifeSpan")?.Value;
                                var lifeSpan = !string.IsNullOrEmpty(lifeSpanValue) ? TimeSpan.Parse(lifeSpanValue) : TimeSpan.Zero;
                                var shouldInvalidateOnErrorValue = cacheSection.FirstOrDefault(c => c.Key == "ShouldInvalidateOnError")?.Value;
                                var shouldInvalidateOnError = !string.IsNullOrEmpty(shouldInvalidateOnErrorValue) && bool.Parse(shouldInvalidateOnErrorValue);

                                if (isRequestOptions)
                                    WithRequestOptions(requestName, requestOptions => requestOptions.WithCaching(mode, lifeSpan, shouldInvalidateOnError));
                                else
                                    WithCaching(mode, lifeSpan, shouldInvalidateOnError);

                                break;
                            }
                        case "Priority":
                            WithHandlerParameter(Constants.PriorityKey, config.Value);
                            break;
                        default:
                            {
                                if (config.GetChildren().Any())
                                {
                                    if (string.IsNullOrWhiteSpace(requestName))
                                        requestName = Options.RequestNames.Contains(config.Key) ? config.Key : null;

                                    WithConfiguration(config, requestName);
                                }

                                break;
                            }
                    }
                }
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithBaseAddress(string baseAddress,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithBaseAddress(_ => baseAddress, strategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.BaseAddressFactory ??= baseAddressFactory;
                    break;
                default:
                    Options.BaseAddressFactory = baseAddressFactory;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithBaseAddress(Uri baseAddress,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithBaseAddress(_ => baseAddress, strategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.BaseUriFactory ??= baseAddressFactory;
                    break;
                default:
                    Options.BaseUriFactory = baseAddressFactory;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithBasePath(string basePath, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithBasePath(_ => basePath, strategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithBasePath(Func<IServiceProvider, string> basePathFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.BasePathFactory ??= basePathFactory;
                    break;
                default:
                    Options.BasePathFactory = basePathFactory;
                    break;
            }

            return this;
        }
        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(_ => httpClientHandler);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithHttpClientHandler(Func<IServiceProvider, HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder ConfigureHttpClientBuilder(
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
        public IApizrExtendedManagerOptionsBuilder WithDelegatingHandler<THandler>(
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((serviceProvider, _) => serviceProvider.GetRequiredService<THandler>());

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithDelegatingHandler<THandler>(Func<IServiceProvider, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((serviceProvider, _) => delegatingHandlerFactory.Invoke(serviceProvider), strategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler(serviceProvider.GetService<ILogger>(),
                    options, 
                    refreshTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory)
            where TAuthenticationHandler : AuthenticationHandlerBase
            => WithDelegatingHandler(authenticationHandlerFactory);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Expression<Func<TSettingsService, string>> tokenProperty,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
            => WithDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService, TTokenService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty,
                    serviceProvider.GetRequiredService<TTokenService>, refreshTokenMethod));

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, string>> tokenProperty)
            => WithDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty,
                    _ => Task.FromResult(
                        tokenProperty.Compile()(serviceProvider.GetRequiredService<TSettingsService>()))));

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty, refreshTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithHeaders(IList<string> headers,
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
        public IApizrExtendedManagerOptionsBuilder WithHeaders(Func<IServiceProvider, IList<string>> headersFactory,
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
        public IApizrExtendedManagerOptionsBuilder WithHeaders<TSettingsService>(
            Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api, 
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
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
        public IApizrExtendedManagerOptionsBuilder WithOperationTimeout(TimeSpan timeout)
            => WithOperationTimeout(_ => timeout);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithOperationTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory)
        {
            Options.OperationTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithRequestTimeout(TimeSpan timeout)
            => WithRequestTimeout(_ => timeout);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithRequestTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory)
        {
            Options.RequestTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithDelegatingHandler<THandler>(THandler delegatingHandler,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((_, _) => delegatingHandler, strategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithDelegatingHandler<THandler>(
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
        public IApizrExtendedManagerOptionsBuilder WithHttpMessageHandler<THandler>() where THandler : HttpMessageHandler
            => WithHttpMessageHandler((serviceProvider, _) => serviceProvider.GetRequiredService<THandler>());

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithHttpMessageHandler<THandler>(Func<IServiceProvider, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((serviceProvider, _) => httpMessageHandlerFactory.Invoke(serviceProvider));

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithHttpMessageHandler<THandler>(THandler httpMessageHandler) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((_, _) => httpMessageHandler);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithHttpMessageHandler<THandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
        {
            if (typeof(DelegatingHandler).IsAssignableFrom(typeof(THandler)))
                return WithDelegatingHandler((serviceProvider, options) => httpMessageHandlerFactory.Invoke(serviceProvider, options) as DelegatingHandler);

            Options.HttpMessageHandlerFactory = httpMessageHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
        public IApizrExtendedManagerOptionsBuilder WithExCatching(Action<ApizrException> onException,
            bool letThrowOnException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(ex =>
            {
                onException.Invoke(ex);
                return Task.FromResult(true);
            }, letThrowOnException,
                strategy);

        /// <inheritdoc />
        [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
        public IApizrExtendedManagerOptionsBuilder WithExCatching<TResult>(Action<ApizrException<TResult>> onException,
            bool letThrowOnException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(ex =>
            {
                onException.Invoke((ApizrException<TResult>)ex);
                return Task.FromResult(true);
            }, letThrowOnException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithExCatching(Func<ApizrException, bool> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(ex => Task.FromResult(onException.Invoke(ex)), letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithExCatching<TResult>(Func<ApizrException<TResult>, bool> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(ex => Task.FromResult(onException.Invoke((ApizrException<TResult>)ex)), letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithExCatching(Func<ApizrException, Task<bool>> onException, bool letThrowOnHandledException = true,
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
                        var previous = Options.OnException;
                        Options.OnException = async ex =>
                            await previous(ex).ConfigureAwait(false) ||
                            await onException(ex).ConfigureAwait(false);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            Options.LetThrowOnHandledException = letThrowOnHandledException;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithExCatching<TResult>(Func<ApizrException<TResult>, Task<bool>> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(ex => onException.Invoke((ApizrException<TResult>)ex), letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithHandlerParameter(string key, object value)
        {
            Options.HandlersParameters[key] = value;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, TValue value)
            => WithResilienceProperty(key, _ => value);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, Func<IServiceProvider, TValue> valueFactory)
        {
            ((IApizrExtendedSharedOptions)Options).ResiliencePropertiesExtendedFactories[key.Key] = serviceProvider => valueFactory(serviceProvider);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All,
            params LogLevel[] logLevels)
            => WithLogging(_ => httpTracerMode, _ => trafficVerbosity, _ => logLevels);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithLogging(
            Func<IServiceProvider, (HttpTracerMode, HttpMessageParts, LogLevel[])> loggingConfigurationFactory)
            => WithLogging(serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item1,
                serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item2,
                serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item3);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithLogging(Func<IServiceProvider, HttpTracerMode> httpTracerModeFactory,
            Func<IServiceProvider, HttpMessageParts> trafficVerbosityFactory,
            Func<IServiceProvider, LogLevel[]> logLevelFactory)
        {
            Options.HttpTracerModeFactory = httpTracerModeFactory;
            Options.TrafficVerbosityFactory = trafficVerbosityFactory;
            Options.LogLevelsFactory = logLevelFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithRefitSettings(RefitSettings refitSettings)
            => WithRefitSettings(_ => refitSettings);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithRefitSettings(
            Func<IServiceProvider, RefitSettings> refitSettingsFactory)
        {
            Options.RefitSettingsFactory = refitSettingsFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithConnectivityHandler(IConnectivityHandler connectivityHandler)
            => WithConnectivityHandler(_ => connectivityHandler);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithConnectivityHandler(Func<IServiceProvider, IConnectivityHandler> connectivityHandlerFactory)
        {
            Options.ConnectivityHandlerFactory = connectivityHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithConnectivityHandler<TConnectivityHandler>(Expression<Func<TConnectivityHandler, bool>> factory)
        {
            Options.ConnectivityHandlerFactory = serviceProvider => new DefaultConnectivityHandler(() => factory.Compile().Invoke(serviceProvider.GetRequiredService<TConnectivityHandler>()));

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithConnectivityHandler(Func<bool> connectivityCheckingFunction)
        {
            Options.ConnectivityHandlerFactory = _ => new DefaultConnectivityHandler(connectivityCheckingFunction);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithConnectivityHandler<TConnectivityHandler>()
            where TConnectivityHandler : class, IConnectivityHandler
            => WithConnectivityHandler(typeof(TConnectivityHandler));

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithConnectivityHandler(Type connectivityHandlerType)
        {
            if (!typeof(IConnectivityHandler).IsAssignableFrom(connectivityHandlerType))
                throw new ArgumentException(
                    $"Your connectivity handler class must inherit from {nameof(IConnectivityHandler)} interface or derived");

            Options.ConnectivityHandlerType = connectivityHandlerType;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithCacheHandler(ICacheHandler cacheHandler)
            => WithCacheHandler(_ => cacheHandler);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithCacheHandler(Func<IServiceProvider, ICacheHandler> cacheHandlerFactory)
        {
            Options.CacheHandlerFactory = cacheHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithCacheHandler<TCacheHandler>()
            where TCacheHandler : class, ICacheHandler
            => WithCacheHandler(typeof(TCacheHandler));

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithCacheHandler(Type cacheHandlerType)
        {
            if (!typeof(ICacheHandler).IsAssignableFrom(cacheHandlerType))
                throw new ArgumentException(
                    $"Your cache handler class must inherit from {nameof(ICacheHandler)} interface or derived");

            Options.CacheHandlerType = cacheHandlerType;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithMappingHandler(IMappingHandler mappingHandler)
            => WithMappingHandler(_ => mappingHandler);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithMappingHandler(Func<IServiceProvider, IMappingHandler> mappingHandlerFactory)
        {
            Options.MappingHandlerFactory = mappingHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithMappingHandler<TMappingHandler>()
            where TMappingHandler : class, IMappingHandler
            => WithMappingHandler(typeof(TMappingHandler));

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithMappingHandler(Type mappingHandlerType)
        {
            if (!typeof(IMappingHandler).IsAssignableFrom(mappingHandlerType))
                throw new ArgumentException(
                    $"Your mapping handler class must inherit from {nameof(IMappingHandler)} interface or derived");

            Options.MappingHandlerType = mappingHandlerType;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithResilienceContextOptions(Action<IApizrResilienceContextOptionsBuilder> contextOptionsBuilder)
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
        public IApizrExtendedManagerOptionsBuilder WithLoggedHeadersRedactionNames(IEnumerable<string> redactedLoggedHeaderNames,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
        {
            var sensitiveHeaders = new HashSet<string>(redactedLoggedHeaderNames, StringComparer.OrdinalIgnoreCase);

            return WithLoggedHeadersRedactionRule(header => sensitiveHeaders.Contains(header), strategy);
        }

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithLoggedHeadersRedactionRule(
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
        public IApizrExtendedManagerOptionsBuilder WithResiliencePipelineKeys(string[] resiliencePipelineKeys,
            IEnumerable<ApizrRequestMethod> methodScope = null,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add)
        {
            methodScope ??= [ApizrRequestMethod.All];

            switch (duplicateStrategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    if (Options.ResiliencePipelineOptions.Count == 0)
                        Options.ResiliencePipelineOptions[ApizrConfigurationSource.ManagerOption] = methodScope
                            .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                            .Cast<ResiliencePipelineAttributeBase>()
                            .ToArray();
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.ResiliencePipelineOptions.TryGetValue(ApizrConfigurationSource.ManagerOption, out var attributes))
                    {
                        foreach (var method in methodScope)
                        {
                            var attribute = attributes.FirstOrDefault(attribute => attribute.RequestMethod == method);
                            if (attribute != null)
                                attribute.RegistryKeys = attribute.RegistryKeys.Union(resiliencePipelineKeys).ToArray();
                            else
                                attributes = attributes.Concat([new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method }]).ToArray();
                        }

                        Options.ResiliencePipelineOptions[ApizrConfigurationSource.ManagerOption] = attributes;
                    }
                    else
                    {
                        Options.ResiliencePipelineOptions[ApizrConfigurationSource.ManagerOption] = methodScope
                            .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                            .Cast<ResiliencePipelineAttributeBase>()
                            .ToArray();
                    }

                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.ResiliencePipelineOptions.Clear();
                    Options.ResiliencePipelineOptions[ApizrConfigurationSource.ManagerOption] = methodScope
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
        public IApizrExtendedManagerOptionsBuilder WithCaching(CacheMode mode = CacheMode.FetchOrGet, TimeSpan? lifeSpan = null,
            bool shouldInvalidateOnError = false)
        {
            Options.CacheOptions[ApizrConfigurationSource.ManagerOption] = new CacheAttribute(mode, lifeSpan, shouldInvalidateOnError);

            return this;
        }

        #region Internal

        void IApizrInternalOptionsBuilder.SetHandlerParameter(string key, object value) => WithHandlerParameter(key, value);

        void IApizrInternalRegistrationOptionsBuilder.SetPrimaryHttpMessageHandler(Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> primaryHandlerFactory)
        {
            Options.PrimaryHandlerFactory = primaryHandlerFactory;
        }

        void IApizrInternalRegistrationOptionsBuilder.AddDelegatingHandler<THandler>(Func<IApizrManagerOptionsBase, THandler> handlerFactory) 
            => WithDelegatingHandler((_, options) => handlerFactory.Invoke(options));

        /// <inheritdoc />
        void IApizrExtendedManagerOptionsBuilder.WithHeaders(
            IDictionary<(ApizrRegistrationMode, ApizrLifetimeScope), Func<IList<string>>> headersFactories)
        {
            Options.HeadersFactories = headersFactories;
        }

        #endregion

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithRequestOptions(string requestName,
            Action<IApizrRequestOptionsBuilder> optionsBuilder,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add)
            => WithRequestOptions([requestName], optionsBuilder, duplicateStrategy);

        /// <inheritdoc />
        public IApizrExtendedManagerOptionsBuilder WithRequestOptions(string[] requestNames,
            Action<IApizrRequestOptionsBuilder> optionsBuilder,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add)
        {
            foreach (var requestName in requestNames)
            {
                if (Options.RequestOptionsBuilders.ContainsKey(requestName))
                {
                    switch (duplicateStrategy)
                    {
                        case ApizrDuplicateStrategy.Ignore:
                            // Skip request configuration
                            break;
                        case ApizrDuplicateStrategy.Replace:
                            Options.RequestOptionsBuilders[requestName] = optionsBuilder;
                            break;
                        case ApizrDuplicateStrategy.Add:
                        case ApizrDuplicateStrategy.Merge:
                            Options.RequestOptionsBuilders[requestName] += optionsBuilder;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(duplicateStrategy), duplicateStrategy, null);
                    }
                }
                else
                {
                    Options.RequestOptionsBuilders[requestName] = optionsBuilder;
                }
            }

            return this;
        }
    }
}
