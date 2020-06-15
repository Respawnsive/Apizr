using Apizr.Authenticating;
using Refit;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Polly.Registry;

namespace Apizr
{
    /// <summary>
    /// The options builder
    /// </summary>
    /// <typeparam name="TApizrOptions">Options built with options builder</typeparam>
    public interface IApizrOptionsBuilder<out TApizrOptions> where TApizrOptions : class, IApizrOptions
    {
        /// <summary>
        /// Apizr options
        /// </summary>
        IApizrOptions ApizrOptions { get; }
    }

    /// <summary>
    /// The options builder
    /// </summary>
    public interface IApizrOptionsBuilder : IApizrOptionsBuilder<ApizrOptions>
    {
        /// <summary>
        /// Provide a method to refresh the authorization token when needed
        /// </summary>
        /// <param name="refreshTokenFactory">Refresh token method called when expired or empty</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Provide your own <see cref="AuthenticationHandlerBase"/> implementation
        /// </summary>
        /// <typeparam name="TAuthenticationHandler">Your <see cref="AuthenticationHandlerBase"/> implementation</typeparam>
        /// <param name="authenticationHandler">A <see cref="TAuthenticationHandler"/> instance</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(TAuthenticationHandler authenticationHandler) where TAuthenticationHandler : AuthenticationHandlerBase;

        /// <summary>
        /// Provide your own <see cref="AuthenticationHandlerBase"/> implementation factory
        /// </summary>
        /// <typeparam name="TAuthenticationHandler">Your <see cref="AuthenticationHandlerBase"/> implementation</typeparam>
        /// <param name="authenticationHandlerFactory">A <see cref="TAuthenticationHandler"/> instance factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<ILogHandler, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase;

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="settingsService">A <see cref="TSettingsService"/> instance</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="tokenService">A <see cref="TTokenService"/> instance</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(TSettingsService settingsService, Expression<Func<TSettingsService, string>> tokenProperty, TTokenService tokenService, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="settingsServiceFactory">A <see cref="TSettingsService"/> instance factory</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="tokenServiceFactory">A <see cref="TTokenService"/> instance factory</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<TTokenService> tokenServiceFactory, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own settings management service and a method to refresh the token
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="settingsService">A <see cref="TSettingsService"/> instance</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenFactory">The method factory called to refresh the token</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService, Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Provide your own settings management service and a method to refresh the token
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="settingsServiceFactory">A <see cref="TSettingsService"/> instance factory</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenFactory">The method factory called to refresh the token</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Provide a policy registry
        /// </summary>
        /// <param name="policyRegistry">A policy registry instance</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithPolicyRegistry(IReadOnlyPolicyRegistry<string> policyRegistry);

        /// <summary>
        /// Provide a policy registry
        /// </summary>
        /// <param name="policyRegistryFactory">A policy registry instance factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithPolicyRegistry(Func<IReadOnlyPolicyRegistry<string>> policyRegistryFactory);

        /// <summary>
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettings">A <see cref="RefitSettings"/> instance</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithRefitSettings(RefitSettings refitSettings);

        /// <summary>
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettingsFactory">A <see cref="RefitSettings"/> instance factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory);

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <param name="connectivityHandler">An <see cref="IConnectivityHandler"/> mapping implementation instance</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithConnectivityHandler(IConnectivityHandler connectivityHandler);

        /// <summary>
        /// Provide a connectivity handler
        /// </summary>
        /// <param name="connectivityHandlerFactory">An <see cref="IConnectivityHandler"/> mapping implementation instance factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithConnectivityHandler(Func<IConnectivityHandler> connectivityHandlerFactory);

        /// <summary>
        /// Provide a cache provider to cache data
        /// </summary>
        /// <param name="cacheProvider">An <see cref="ICacheProvider"/> mapping implementation instance</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithCacheProvider(ICacheProvider cacheProvider);

        /// <summary>
        /// Provide a cache provider to cache data
        /// </summary>
        /// <param name="cacheProviderFactory">An <see cref="ICacheProvider"/> mapping implementation instance factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithCacheProvider(Func<ICacheProvider> cacheProviderFactory);

        /// <summary>
        /// Provide a logging handler to log it all
        /// </summary>
        /// <param name="logHandler">An <see cref="ILogHandler"/> mapping implementation instance</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithLogHandler(ILogHandler logHandler);

        /// <summary>
        /// Provide a logging handler to log it all
        /// </summary>
        /// <param name="logHandlerFactory">An <see cref="ILogHandler"/> mapping implementation instance factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithLogHandler(Func<ILogHandler> logHandlerFactory);
    }
}