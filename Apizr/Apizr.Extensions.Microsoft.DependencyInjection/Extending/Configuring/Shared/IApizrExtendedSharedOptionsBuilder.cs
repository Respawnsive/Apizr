using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Configuring;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending.Configuring.Shared
{
    public interface IApizrExtendedSharedOptionsBuilder<out TApizrExtendedSharedOptions, out TApizrExtendedSharedOptionsBuilder> : IApizrExtendedSharedOptionsBuilderBase, 
        IApizrGlobalSharedOptionsBuilderBase<TApizrExtendedSharedOptions, TApizrExtendedSharedOptionsBuilder>
        where TApizrExtendedSharedOptions : IApizrSharedOptionsBase
        where TApizrExtendedSharedOptionsBuilder : IApizrGlobalSharedOptionsBuilderBase<TApizrExtendedSharedOptions, TApizrExtendedSharedOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory);

        /// <summary>
        /// Define your web api base path (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="basePathFactory">Your web api base path factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithBasePath(Func<IServiceProvider, string> basePathFactory);

        /// <summary>
        /// Define http traffic tracing verbosity and log level (could be defined with TraceAttribute)
        /// </summary>
        /// <param name="httpTracerModeFactory">Http traffic tracing mode factory</param>
        /// <param name="trafficVerbosityFactory">Http traffic tracing verbosity factory</param>
        /// <param name="logLevelsFactory">Log levels factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithLogging(Func<IServiceProvider, HttpTracerMode> httpTracerModeFactory, Func<IServiceProvider, HttpMessageParts> trafficVerbosityFactory, Func<IServiceProvider, LogLevel[]> logLevelsFactory);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandlerFactory">An <see cref="HttpClientHandler"/> instance factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithHttpClientHandler(Func<IServiceProvider, HttpClientHandler> httpClientHandlerFactory);

        /// <summary>
        /// Adjust some HttpClient settings
        /// </summary>
        /// <param name="httpClientBuilder">The HttpClient builder</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder ConfigureHttpClientBuilder(Action<IHttpClientBuilder> httpClientBuilder);

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder AddDelegatingHandler(Func<IServiceProvider, DelegatingHandler> delegatingHandlerFactory);

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder AddDelegatingHandler(Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler> delegatingHandlerFactory);

        /// <summary>
        /// Provide your own <see cref="AuthenticationHandlerBase"/> implementation
        /// </summary>
        /// <typeparam name="TAuthenticationHandler">Your <see cref="AuthenticationHandlerBase"/> implementation</typeparam>
        /// <param name="authenticationHandlerFactory">A <typeparamref name="TAuthenticationHandler"/> instance factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<IServiceProvider, IApizrOptionsBase, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase;

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Expression<Func<TSettingsService, string>> tokenProperty, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own settings management service and a method to refresh the token
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenFactory">The method factory called to refresh the token</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TSettingsService>(Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

    }
}
