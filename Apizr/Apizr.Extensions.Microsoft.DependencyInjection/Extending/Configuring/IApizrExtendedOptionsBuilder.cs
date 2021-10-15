using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Logging;
using Apizr.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Extending.Configuring
{
    public interface IApizrExtendedOptionsBuilder<out TApizrExtendedOptions, out TApizrExtendedOptionsBuilder> : IApizrOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedCommonOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedProperOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
        where TApizrExtendedOptions : IApizrOptionsBase
        where TApizrExtendedOptionsBuilder : IApizrOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandler">An <see cref="HttpClientHandler"/> instance</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandlerFactory">An <see cref="HttpClientHandler"/> instance factory</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithHttpClientHandler(Func<IServiceProvider, HttpClientHandler> httpClientHandlerFactory);

        /// <summary>
        /// Adjust some HttpClient settings
        /// </summary>
        /// <param name="httpClientBuilder">The HttpClient builder</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder ConfigureHttpClientBuilder(Action<IHttpClientBuilder> httpClientBuilder);

        /// <summary>
        /// Provide your own <see cref="AuthenticationHandlerBase"/> implementation
        /// </summary>
        /// <typeparam name="TAuthenticationHandler">Your <see cref="AuthenticationHandlerBase"/> implementation</typeparam>
        /// <param name="authenticationHandlerFactory">A <see cref="TAuthenticationHandler"/> instance factory</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<IServiceProvider, IApizrOptionsBase, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase;

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Expression<Func<TSettingsService, string>> tokenProperty, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own settings management service and a method to refresh the token
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenFactory">The method factory called to refresh the token</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService>(Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder AddDelegatingHandler(Func<IServiceProvider, DelegatingHandler> delegatingHandlerFactory);

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder AddDelegatingHandler(Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler> delegatingHandlerFactory);

        /// <summary>
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettingsFactory">A <see cref="RefitSettings"/> instance factory</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithRefitSettings(Func<IServiceProvider, RefitSettings> refitSettingsFactory);

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <typeparam name="TConnectivityHandler">Your <see cref="IConnectivityHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithConnectivityHandler<TConnectivityHandler>(Expression<Func<TConnectivityHandler, bool>> connectivityProperty);

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <typeparam name="TConnectivityHandler">Your <see cref="IConnectivityHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithConnectivityHandler<TConnectivityHandler>() where TConnectivityHandler : class, IConnectivityHandler;

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <param name="connectivityHandlerType">Type of your <see cref="IConnectivityHandler"/> mapping implementation</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithConnectivityHandler(Type connectivityHandlerType);

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <typeparam name="TCacheHandler">Your <see cref="ICacheHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithCacheHandler<TCacheHandler>() where TCacheHandler : class, ICacheHandler;

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandlerType">Type of your <see cref="ICacheHandler"/> mapping implementation</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithCacheHandler(Type cacheHandlerType);

        /// <summary>
        /// Define http traffic tracing verbosity and log level (could be defined with TraceAttribute)
        /// </summary>
        /// <param name="httpTracerModeFactory">Http traffic tracing mode factory</param>
        /// <param name="trafficVerbosityFactory">Http traffic tracing verbosity factory</param>
        /// <param name="logLevelFactory">Log level factory</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithLogging(Func<IServiceProvider, HttpTracerMode> httpTracerModeFactory, Func<IServiceProvider, HttpMessageParts> trafficVerbosityFactory, Func<IServiceProvider, LogLevel> logLevelFactory);

        /// <summary>
        /// Provide a mapping handler to auto map entities during mediation
        /// </summary>
        /// <typeparam name="TMappingHandler">Your <see cref="IMappingHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithMappingHandler<TMappingHandler>() where TMappingHandler : class, IMappingHandler;

        /// <summary>
        /// Provide a mapping handler to auto map entities during mediation
        /// </summary>
        /// <param name="mappingHandlerType">Type of your <see cref="IMappingHandler"/> mapping implementation</param>
        /// <returns></returns>
        TApizrExtendedOptionsBuilder WithMappingHandler(Type mappingHandlerType);
    }

    public interface IApizrExtendedOptionsBuilder : IApizrExtendedOptionsBuilder<IApizrExtendedOptions, IApizrExtendedOptionsBuilder>
    { }
}
