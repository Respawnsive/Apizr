using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Configuring.Common;
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
    public class ApizrManagerOptionsBuilder : IApizrManagerOptionsBuilder
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
        public IApizrManagerOptionsBuilder WithHttpClient(Func<HttpMessageHandler, Uri, HttpClient> httpClientFactory)
        {
            Options.HttpClientFactory = httpClientFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<ILogger, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandler) where TAuthenticationHandler : AuthenticationHandlerBase
        {
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            var authenticationHandler = new Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>((logger, options) =>
                new AuthenticationHandler(logger, options, refreshToken));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshToken)
            => WithAuthenticationHandler(() => settingsService, tokenProperty, refreshToken);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            var authenticationHandler = new Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>((logger, options) =>
                new AuthenticationHandler<TSettingsService>(logger, options, settingsServiceFactory, tokenProperty, refreshToken));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

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
        {
            var authenticationHandler = new Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>((logHger, options) =>
                new AuthenticationHandler<TSettingsService, TTokenService>(logHger,
                    options,
                    settingsServiceFactory, tokenProperty,
                    tokenServiceFactory, refreshTokenMethod));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler)
            => AddDelegatingHandler(_ => delegatingHandler);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder AddDelegatingHandler(Func<ILogger, DelegatingHandler> delegatingHandlerFactory)
            => AddDelegatingHandler((logger, _) => delegatingHandlerFactory(logger));

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder AddDelegatingHandler(Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler> delegatingHandlerFactory)
        {
            Options.DelegatingHandlersFactories.Add(delegatingHandlerFactory);

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
        public IApizrManagerOptionsBuilder WithPolicyRegistry(IReadOnlyPolicyRegistry<string> policyRegistry)
            => WithPolicyRegistry(() => policyRegistry);

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithPolicyRegistry(Func<IReadOnlyPolicyRegistry<string>> policyRegistryFactory)
        {
            Options.PolicyRegistryFactory = policyRegistryFactory;

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
        public IApizrManagerOptionsBuilder WithContext(Func<Context> contextFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.ContextFactory ??= contextFactory;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.ContextFactory = contextFactory;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.ContextFactory == null)
                    {
                        Options.ContextFactory = contextFactory;
                    }
                    else
                    {
                        Options.ContextFactory = () => new Context(null,
                            Options.ContextFactory.Invoke().Concat(contextFactory.Invoke().ToList())
                                .ToDictionary(x => x.Key, x => x.Value));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrManagerOptionsBuilder WithExCatching(Action<ApizrException> onException,
            bool letThrowOnExceptionWithEmptyCache = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        {
            Options.OnException = onException;
            Options.LetThrowOnExceptionWithEmptyCache = letThrowOnExceptionWithEmptyCache;

            return this;
        }

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
    }
}
