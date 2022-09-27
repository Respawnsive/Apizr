using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Extending.Configuring.Common
{
    /// <summary>
    /// Builder options available at common level for extended registration
    /// </summary>
    public class ApizrExtendedCommonOptionsBuilder : IApizrExtendedCommonOptionsBuilder
    {
        protected readonly ApizrExtendedCommonOptions Options;

        internal ApizrExtendedCommonOptionsBuilder(ApizrExtendedCommonOptions commonOptions)
        {
            Options = commonOptions;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptions ApizrOptions => Options;

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithBaseAddress(string baseAddress)
            => WithBaseAddress(_ => baseAddress);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory)
        {
            Options.BaseAddressFactory = baseAddressFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithBaseAddress(Uri baseAddress)
            => WithBaseAddress(_ => baseAddress);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory)
        {
            Options.BaseUriFactory = baseAddressFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithBasePath(string basePath)
            => WithBasePath(_ => basePath);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithBasePath(Func<IServiceProvider, string> basePathFactory)
        {
            Options.BasePathFactory = basePathFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(_ => httpClientHandler);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithHttpClientHandler(Func<IServiceProvider, HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            var authenticationHandler = new Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>((serviceProvider, options) =>
                new AuthenticationHandler(serviceProvider.GetService<ILogger>(), options, refreshTokenFactory));
            Options.DelegatingHandlersExtendedFactories.Add(authenticationHandler);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<IServiceProvider, IApizrOptionsBase, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase
        {
            Options.DelegatingHandlersExtendedFactories.Add(authenticationHandlerFactory);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Expression<Func<TSettingsService, string>> tokenProperty,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
        {
            Options.DelegatingHandlersExtendedFactories.Add((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService, TTokenService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty,
                    serviceProvider.GetRequiredService<TTokenService>, refreshTokenMethod));

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithAuthenticationHandler<TSettingsService>(Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            Options.DelegatingHandlersExtendedFactories.Add((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty, refreshTokenFactory));

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler)
            => AddDelegatingHandler(_ => delegatingHandler);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder AddDelegatingHandler(
            Func<IServiceProvider, DelegatingHandler> delegatingHandlerFactory)
            => AddDelegatingHandler((serviceProvider, _) => delegatingHandlerFactory(serviceProvider));

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder AddDelegatingHandler(Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler> delegatingHandlerFactory)
        {
            Options.DelegatingHandlersExtendedFactories.Add(delegatingHandlerFactory);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder ConfigureHttpClientBuilder(Action<IHttpClientBuilder> httpClientBuilder)
        {
            Options.HttpClientBuilder = httpClientBuilder;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All,
            params LogLevel[] logLevels)
            => WithLogging(_ => httpTracerMode, _ => trafficVerbosity, _ => logLevels);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithLogging(
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
        public IApizrExtendedCommonOptionsBuilder WithRefitSettings(RefitSettings refitSettings)
            => WithRefitSettings(_ => refitSettings);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithRefitSettings(
            Func<IServiceProvider, RefitSettings> refitSettingsFactory)
        {
            Options.RefitSettingsFactory = refitSettingsFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithConnectivityHandler(IConnectivityHandler connectivityHandler)
            => WithConnectivityHandler(_ => connectivityHandler);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithConnectivityHandler(Func<IServiceProvider, IConnectivityHandler> connectivityHandlerFactory)
        {
            Options.ConnectivityHandlerFactory = connectivityHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithConnectivityHandler<TConnectivityHandler>(Expression<Func<TConnectivityHandler, bool>> factory)
        {
            Options.ConnectivityHandlerFactory = serviceProvider => new DefaultConnectivityHandler(() => factory.Compile()(serviceProvider.GetRequiredService<TConnectivityHandler>()));

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithConnectivityHandler(Func<bool> connectivityCheckingFunction)
        {
            Options.ConnectivityHandlerFactory = _ => new DefaultConnectivityHandler(connectivityCheckingFunction);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithConnectivityHandler<TConnectivityHandler>()
            where TConnectivityHandler : class, IConnectivityHandler
            => WithConnectivityHandler(typeof(TConnectivityHandler));

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithConnectivityHandler(Type connectivityHandlerType)
        {
            if (!typeof(IConnectivityHandler).IsAssignableFrom(connectivityHandlerType))
                throw new ArgumentException(
                    $"Your connectivity handler class must inherit from {nameof(IConnectivityHandler)} interface or derived");

            Options.ConnectivityHandlerType = connectivityHandlerType;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithCacheHandler(ICacheHandler cacheHandler)
            => WithCacheHandler(_ => cacheHandler);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithCacheHandler(Func<IServiceProvider, ICacheHandler> cacheHandlerFactory)
        {
            Options.CacheHandlerFactory = cacheHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithCacheHandler<TCacheHandler>()
            where TCacheHandler : class, ICacheHandler
            => WithCacheHandler(typeof(TCacheHandler));

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithCacheHandler(Type cacheHandlerType)
        {
            if (!typeof(ICacheHandler).IsAssignableFrom(cacheHandlerType))
                throw new ArgumentException(
                    $"Your cache handler class must inherit from {nameof(ICacheHandler)} interface or derived");

            Options.CacheHandlerType = cacheHandlerType;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithMappingHandler(IMappingHandler mappingHandler)
            => WithMappingHandler(_ => mappingHandler);

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithMappingHandler(Func<IServiceProvider, IMappingHandler> mappingHandlerFactory)
        {
            Options.MappingHandlerFactory = mappingHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithMappingHandler<TMappingHandler>()
            where TMappingHandler : class, IMappingHandler
            => WithMappingHandler(typeof(TMappingHandler));

        /// <inheritdoc />
        public IApizrExtendedCommonOptionsBuilder WithMappingHandler(Type mappingHandlerType)
        {
            if (!typeof(IMappingHandler).IsAssignableFrom(mappingHandlerType))
                throw new ArgumentException(
                    $"Your mapping handler class must inherit from {nameof(IMappingHandler)} interface or derived");

            Options.MappingHandlerType = mappingHandlerType;

            return this;
        }
    }
}
