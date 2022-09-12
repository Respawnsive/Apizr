using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Refit;

namespace Apizr.Configuring
{
    public class ApizrOptionsBuilder : IApizrOptionsBuilder
    {
        protected readonly ApizrOptions Options;

        internal ApizrOptionsBuilder(ApizrOptions apizrOptions)
        {
            Options = apizrOptions;
        }

        public IApizrOptions ApizrOptions => Options;

        public IApizrOptionsBuilder WithBaseAddress(string baseAddress)
            => WithBaseAddress(() => baseAddress);

        public IApizrOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory)
        {
            Options.BaseAddressFactory = baseAddressFactory;

            return this;
        }

        public IApizrOptionsBuilder WithBaseAddress(Uri baseAddress)
            => WithBaseAddress(() => baseAddress);

        public IApizrOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory)
        {
            Options.BaseUriFactory = baseAddressFactory;

            return this;
        }

        public IApizrOptionsBuilder WithBasePath(string basePath)
            => WithBasePath(() => basePath);

        public IApizrOptionsBuilder WithBasePath(Func<string> basePathFactory)
        {
            Options.BasePathFactory = basePathFactory;

            return this;
        }

        public IApizrOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(() => httpClientHandler);

        public IApizrOptionsBuilder WithHttpClientHandler(Func<HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<ILogger, IApizrOptionsBase, TAuthenticationHandler> authenticationHandler) where TAuthenticationHandler : AuthenticationHandlerBase
        {
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            var authenticationHandler = new Func<ILogger, IApizrOptionsBase, DelegatingHandler>((logger, options) =>
                new AuthenticationHandler(logger, options, refreshToken));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshToken)
            => WithAuthenticationHandler(() => settingsService, tokenProperty, refreshToken);

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            var authenticationHandler = new Func<ILogger, IApizrOptionsBase, DelegatingHandler>((logger, options) =>
                new AuthenticationHandler<TSettingsService>(logger, options, settingsServiceFactory, tokenProperty, refreshToken));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty, TTokenService tokenService,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
            => WithAuthenticationHandler(() => settingsService, tokenProperty, () => tokenService, refreshTokenMethod);

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty,
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
        {
            var authenticationHandler = new Func<ILogger, IApizrOptionsBase, DelegatingHandler>((logHger, options) =>
                new AuthenticationHandler<TSettingsService, TTokenService>(logHger,
                    options,
                    settingsServiceFactory, tokenProperty,
                    tokenServiceFactory, refreshTokenMethod));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler)
            => AddDelegatingHandler(_ => delegatingHandler);

        public IApizrOptionsBuilder AddDelegatingHandler(Func<ILogger, DelegatingHandler> delegatingHandlerFactory)
            => AddDelegatingHandler((logger, _) => delegatingHandlerFactory(logger));

        public IApizrOptionsBuilder AddDelegatingHandler(Func<ILogger, IApizrOptionsBase, DelegatingHandler> delegatingHandlerFactory)
        {
            Options.DelegatingHandlersFactories.Add(delegatingHandlerFactory);

            return this;
        }

        public IApizrOptionsBuilder WithRefitSettings(RefitSettings refitSettings)
            => WithRefitSettings(() => refitSettings);

        public IApizrOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory)
        {
            Options.RefitSettingsFactory = refitSettingsFactory;

            return this;
        }

        public IApizrOptionsBuilder WithPolicyRegistry(IReadOnlyPolicyRegistry<string> policyRegistry)
            => WithPolicyRegistry(() => policyRegistry);

        public IApizrOptionsBuilder WithPolicyRegistry(Func<IReadOnlyPolicyRegistry<string>> policyRegistryFactory)
        {
            Options.PolicyRegistryFactory = policyRegistryFactory;

            return this;
        }

        public IApizrOptionsBuilder WithConnectivityHandler(Func<bool> connectivityCheckingFunction)
            => WithConnectivityHandler(() => new DefaultConnectivityHandler(connectivityCheckingFunction));

        public IApizrOptionsBuilder WithConnectivityHandler(IConnectivityHandler connectivityHandler)
            => WithConnectivityHandler(() => connectivityHandler);

        public IApizrOptionsBuilder WithConnectivityHandler(Func<IConnectivityHandler> connectivityHandlerFactory)
        {
            Options.ConnectivityHandlerFactory = connectivityHandlerFactory;

            return this;
        }

        public IApizrOptionsBuilder WithCacheHandler(ICacheHandler cacheHandler)
            => WithCacheHandler(() => cacheHandler);

        public IApizrOptionsBuilder WithCacheHandler(Func<ICacheHandler> cacheHandlerFactory)
        {
            Options.CacheHandlerFactory = cacheHandlerFactory;

            return this;
        }

        public IApizrOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All,
            params LogLevel[] logLevels)
            => WithLogging(() => httpTracerMode, () => trafficVerbosity, () => logLevels);

        public IApizrOptionsBuilder WithLogging(Func<HttpTracerMode> httpTracerModeFactory,
            Func<HttpMessageParts> trafficVerbosityFactory, Func<LogLevel[]> logLevelsFactory)
        {
            Options.HttpTracerModeFactory = httpTracerModeFactory;
            Options.TrafficVerbosityFactory = trafficVerbosityFactory;
            Options.LogLevelsFactory = logLevelsFactory;

            return this;
        }

        public IApizrOptionsBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
            => WithLoggerFactory(() => loggerFactory);

        public IApizrOptionsBuilder WithLoggerFactory(Func<ILoggerFactory> loggerFactory)
        {
            Options.LoggerFactoryFactory = loggerFactory;

            return this;
        }

        public IApizrOptionsBuilder WithMappingHandler(IMappingHandler mappingHandler)
            => WithMappingHandler(() => mappingHandler);

        public IApizrOptionsBuilder WithMappingHandler(Func<IMappingHandler> mappingHandlerFactory)
        {
            Options.MappingHandlerFactory = mappingHandlerFactory;

            return this;
        }
    }
}
