using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    public interface IApizrSharedOptionsBuilder<out TApizrSharedOptions, out TApizrSharedOptionsBuilder> : 
        IApizrSharedOptionsBuilderBase<TApizrSharedOptions, TApizrSharedOptionsBuilder>
        where TApizrSharedOptions : IApizrSharedOptionsBase
        where TApizrSharedOptionsBuilder : IApizrSharedOptionsBuilderBase<TApizrSharedOptions, TApizrSharedOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory);

        /// <summary>
        /// Define your web api base path (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="basePathFactory">Your web api base path factory</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithBasePath(Func<string> basePathFactory);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandlerFactory">An <see cref="HttpClientHandler"/> instance factory</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithHttpClientHandler(Func<HttpClientHandler> httpClientHandlerFactory);

        /// <summary>
        /// Provide your own <see cref="AuthenticationHandlerBase"/> implementation factory
        /// </summary>
        /// <typeparam name="TAuthenticationHandler">Your <see cref="AuthenticationHandlerBase"/> implementation</typeparam>
        /// <param name="authenticationHandlerFactory">A <typeparamref name="TAuthenticationHandler"/> instance factory</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<ILogger, IApizrOptionsBase, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase;

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="tokenService">A <typeparamref name="TTokenService"/> instance</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(TSettingsService settingsService, Expression<Func<TSettingsService, string>> tokenProperty, TTokenService tokenService, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="tokenServiceFactory">A <typeparamref name="TTokenService"/> instance factory</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<TTokenService> tokenServiceFactory, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own settings management service and a method to refresh the token
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenFactory">The method factory called to refresh the token</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService, Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Provide your own settings management service and a method to refresh the token
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenFactory">The method factory called to refresh the token</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithAuthenticationHandler<TSettingsService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder AddDelegatingHandler(Func<ILogger, DelegatingHandler> delegatingHandlerFactory);

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder AddDelegatingHandler(Func<ILogger, IApizrOptionsBase, DelegatingHandler> delegatingHandlerFactory);

        /// <summary>
        /// Configure logging level for the api
        /// </summary>
        /// <param name="httpTracerModeFactory">Http traffic tracing mode</param>
        /// <param name="trafficVerbosityFactory">Http traffic tracing verbosity factory</param>
        /// <param name="logLevelsFactory"></param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithLogging(Func<HttpTracerMode> httpTracerModeFactory,
            Func<HttpMessageParts> trafficVerbosityFactory, Func<LogLevel[]> logLevelsFactory);
    }
}
