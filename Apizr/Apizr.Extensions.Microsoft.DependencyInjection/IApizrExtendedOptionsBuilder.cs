using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public interface IApizrExtendedOptionsBuilder
    {
        /// <summary>
        /// Apizr options
        /// </summary>
        IApizrExtendedOptions ApizrOptions { get; }

        /// <summary>
        /// Adjust some HttpClient settings
        /// </summary>
        /// <param name="httpClientBuilder">The HttpClient builder</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder ConfigureHttpClientBuilder(Action<IHttpClientBuilder> httpClientBuilder);

        /// <summary>
        /// Provide a method to refresh the authorization token when needed
        /// </summary>
        /// <param name="refreshTokenFactory">Refresh token method called when expired or empty</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Provide your own <see cref="AuthenticationHandlerBase"/> implementation
        /// </summary>
        /// <typeparam name="TAuthenticationHandler">Your <see cref="AuthenticationHandlerBase"/> implementation</typeparam>
        /// <param name="authenticationHandlerFactory">A <see cref="TAuthenticationHandler"/> instance factory</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<IServiceProvider, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase;

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Expression<Func<TSettingsService, string>> tokenProperty, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own settings management service and a method to refresh the token
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenFactory">The method factory called to refresh the token</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService>(Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettings">A <see cref="RefitSettings"/> instance</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithRefitSettings(RefitSettings refitSettings);

        /// <summary>
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettingsFactory">A <see cref="RefitSettings"/> instance factory</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithRefitSettings(Func<IServiceProvider, RefitSettings> refitSettingsFactory);

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <typeparam name="TConnectivityHandler">Your <see cref="IConnectivityHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithConnectivityHandler<TConnectivityHandler>() where TConnectivityHandler : class, IConnectivityHandler;

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <param name="connectivityHandlerType">Type of your <see cref="IConnectivityHandler"/> mapping implementation</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithConnectivityHandler(Type connectivityHandlerType);

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <typeparam name="TCacheHandler">Your <see cref="ICacheHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithCacheHandler<TCacheHandler>() where TCacheHandler : class, ICacheHandler;

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandlerType">Type of your <see cref="ICacheHandler"/> mapping implementation</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithCacheHandler(Type cacheHandlerType);

        /// <summary>
        /// Provide a logging handler to log it all
        /// </summary>
        /// <typeparam name="TLogHandler">Your <see cref="ILogHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithLogHandler<TLogHandler>() where TLogHandler : class, ILogHandler;

        /// <summary>
        /// Provide a logging handler to log it all
        /// </summary>
        /// <param name="logHandlerType">Type of your <see cref="ILogHandler"/> mapping implementation</param>
        /// <returns></returns>
        IApizrExtendedOptionsBuilder WithLogHandler(Type logHandlerType);
    }
}
