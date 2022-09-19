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

namespace Apizr.Extending.Configuring
{
    /// <inheritdoc />
    public class ApizrExtendedOptionsBuilder : IApizrExtendedOptionsBuilder
    {
        protected readonly ApizrExtendedOptions Options;

        public ApizrExtendedOptionsBuilder(ApizrExtendedOptions apizrOptions)
        {
            Options = apizrOptions;
        }

        /// <inheritdoc />
        public IApizrExtendedOptions ApizrOptions => Options;

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithBaseAddress(string baseAddress)
            => WithBaseAddress(_ => baseAddress);

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory)
        {
            Options.BaseAddressFactory = baseAddressFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithBaseAddress(Uri baseAddress)
            => WithBaseAddress(_ => baseAddress);

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory)
        {
            Options.BaseUriFactory = baseAddressFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithBasePath(string basePath)
            => WithBasePath(_ => basePath);

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithBasePath(Func<IServiceProvider, string> basePathFactory)
        {
            Options.BasePathFactory = basePathFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(_ => httpClientHandler);

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithHttpClientHandler(Func<IServiceProvider, HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder ConfigureHttpClientBuilder(Action<IHttpClientBuilder> httpClientBuilder)
        {
            Options.HttpClientBuilder = httpClientBuilder;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            var authenticationHandler = new Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>((serviceProvider, options) =>
                new AuthenticationHandler(serviceProvider.GetService<ILogger>(), options, refreshTokenFactory));
            Options.DelegatingHandlersExtendedFactories.Add(authenticationHandler);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<IServiceProvider, IApizrOptionsBase, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase
        {
            Options.DelegatingHandlersExtendedFactories.Add(authenticationHandlerFactory);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Expression<Func<TSettingsService, string>> tokenProperty,
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
        public IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService>(Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            Options.DelegatingHandlersExtendedFactories.Add((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty, refreshTokenFactory));

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler)
            => AddDelegatingHandler(_ => delegatingHandler);

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder AddDelegatingHandler(
            Func<IServiceProvider, DelegatingHandler> delegatingHandlerFactory)
            => AddDelegatingHandler((serviceProvider, _) => delegatingHandlerFactory(serviceProvider));

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder AddDelegatingHandler(Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler> delegatingHandlerFactory)
        {
            Options.DelegatingHandlersExtendedFactories.Add(delegatingHandlerFactory);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All,
            params LogLevel[] logLevels)
            => WithLogging(_ => httpTracerMode, _ => trafficVerbosity, _ => logLevels);

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithLogging(Func<IServiceProvider, HttpTracerMode> httpTracerModeFactory,
            Func<IServiceProvider, HttpMessageParts> trafficVerbosityFactory,
            Func<IServiceProvider, LogLevel[]> logLevelFactory)
        {
            Options.HttpTracerModeFactory = httpTracerModeFactory;
            Options.TrafficVerbosityFactory = trafficVerbosityFactory;
            Options.LogLevelsFactory = logLevelFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithRefitSettings(RefitSettings refitSettings)
            => WithRefitSettings(_ => refitSettings);

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithConnectivityHandler(IConnectivityHandler connectivityHandler)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithRefitSettings(
            Func<IServiceProvider, RefitSettings> refitSettingsFactory)
        {
            Options.RefitSettingsFactory = refitSettingsFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithConnectivityHandler(Func<IServiceProvider, IConnectivityHandler> connectivityHandlerFactory)
        {
            Options.ConnectivityHandlerFactory = connectivityHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithConnectivityHandler<TConnectivityHandler>(Expression<Func<TConnectivityHandler, bool>> factory)
        {
            Options.ConnectivityHandlerFactory = serviceProvider => new DefaultConnectivityHandler(() => factory.Compile()(serviceProvider.GetRequiredService<TConnectivityHandler>()));

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithConnectivityHandler(Func<bool> connectivityCheckingFunction)
        {
            Options.ConnectivityHandlerFactory = _ => new DefaultConnectivityHandler(connectivityCheckingFunction);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithConnectivityHandler<TConnectivityHandler>()
            where TConnectivityHandler : class, IConnectivityHandler
            => WithConnectivityHandler(typeof(TConnectivityHandler));

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithConnectivityHandler(Type connectivityHandlerType)
        {
            if (!typeof(IConnectivityHandler).IsAssignableFrom(connectivityHandlerType))
                throw new ArgumentException(
                    $"Your connectivity handler class must inherit from {nameof(IConnectivityHandler)} interface or derived");

            Options.ConnectivityHandlerType = connectivityHandlerType;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithCacheHandler(ICacheHandler cacheHandler)
            => WithCacheHandler(_ => cacheHandler);

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithCacheHandler(Func<IServiceProvider, ICacheHandler> cacheHandlerFactory)
        {
            Options.CacheHandlerFactory = cacheHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithCacheHandler<TCacheHandler>()
            where TCacheHandler : class, ICacheHandler
            => WithCacheHandler(typeof(TCacheHandler));

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithCacheHandler(Type cacheHandlerType)
        {
            if (!typeof(ICacheHandler).IsAssignableFrom(cacheHandlerType))
                throw new ArgumentException(
                    $"Your cache handler class must inherit from {nameof(ICacheHandler)} interface or derived");

            Options.CacheHandlerType = cacheHandlerType;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithMappingHandler(IMappingHandler mappingHandler)
            => WithMappingHandler(_ => mappingHandler);

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithMappingHandler(Func<IServiceProvider, IMappingHandler> mappingHandlerFactory)
        {
            Options.MappingHandlerFactory = mappingHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithMappingHandler<TMappingHandler>()
            where TMappingHandler : class, IMappingHandler
            => WithMappingHandler(typeof(TMappingHandler));

        /// <inheritdoc />
        public IApizrExtendedOptionsBuilder WithMappingHandler(Type mappingHandlerType)
        {
            if (!typeof(IMappingHandler).IsAssignableFrom(mappingHandlerType))
                throw new ArgumentException(
                    $"Your mapping handler class must inherit from {nameof(IMappingHandler)} interface or derived");

            Options.MappingHandlerType = mappingHandlerType;

            return this;
        }
    }
}
