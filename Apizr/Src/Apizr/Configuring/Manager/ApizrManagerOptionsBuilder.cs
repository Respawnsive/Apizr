using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Configuring.Shared;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
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
        public IApizrManagerOptionsBuilder WithHttpClient(Func<HttpMessageHandler, Uri, HttpClient> httpClientFactory)
        {
            Options.HttpClientFactory = httpClientFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<ILogger, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory)
            where TAuthenticationHandler : AuthenticationHandlerBase
            => AddDelegatingHandler(authenticationHandlerFactory);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => AddDelegatingHandler((logger, options) =>
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
            => AddDelegatingHandler((logger, options) =>
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
            => AddDelegatingHandler((logger, options) =>
                new AuthenticationHandler<TSettingsService, TTokenService>(logger,
                    options,
                    settingsServiceFactory, tokenProperty,
                    tokenServiceFactory, refreshTokenMethod));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder AddDelegatingHandler<THandler>(THandler delegatingHandler) where THandler : DelegatingHandler
            => AddDelegatingHandler((_, _) => delegatingHandler);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder AddDelegatingHandler<THandler>(Func<ILogger, THandler> delegatingHandlerFactory) where THandler : DelegatingHandler
            => AddDelegatingHandler((logger, _) => delegatingHandlerFactory(logger));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder AddDelegatingHandler<THandler>(Func<ILogger, IApizrManagerOptionsBase, THandler> delegatingHandlerFactory) where THandler : DelegatingHandler
        {
            Options.DelegatingHandlersFactories[typeof(THandler)] = delegatingHandlerFactory;

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
        public IApizrManagerOptionsBuilder WithHeaders(params string[] headers)
            => WithHeaders(() => headers?.ToList());

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithHeaders(Func<IList<string>> headersFactory)
        {
            Options.HeadersFactories.Add(headersFactory);

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

        #region Internal

        void IApizrInternalOptionsBuilder.SetHandlerParameter(string key, object value) => WithHandlerParameter(key, value);

        void IApizrInternalRegistrationOptionsBuilder.SetPrimaryHttpMessageHandler(Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> primaryHandlerFactory)
        {
            Options.PrimaryHandlerFactory = primaryHandlerFactory;
        }

        /// <inheritdoc />
        void IApizrInternalRegistrationOptionsBuilder.AddDelegatingHandler<THandler>(Func<IApizrManagerOptionsBase, THandler> handlerFactory) 
            => AddDelegatingHandler((_, opt) => handlerFactory.Invoke(opt));

        #endregion
    }
}
