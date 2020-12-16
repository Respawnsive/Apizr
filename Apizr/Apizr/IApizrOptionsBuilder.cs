using Apizr.Authenticating;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using HttpTracer;
using Polly.Registry;

namespace Apizr
{
    /// <summary>
    /// The options builder
    /// </summary>
    public interface IApizrOptionsBuilder : IApizrOptionsBuilderBase<IApizrOptions, IApizrOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory);

        /// <summary>
        /// Define http traces and Apizr logs verbosity (could be defined with TraceAttribute)
        /// </summary>
        /// <param name="trafficVerbosityFactory">Http traffic tracing verbosity factory</param>
        /// <param name="apizrVerbosityFactory">Apizr execution steps verbosity factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithLoggingVerbosity(Func<HttpMessageParts> trafficVerbosityFactory, Func<ApizrLogLevel> apizrVerbosityFactory);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandler">An <see cref="HttpClientHandler"/> instance</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandlerFactory">An <see cref="HttpClientHandler"/> instance factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithHttpClientHandler(Func<HttpClientHandler> httpClientHandlerFactory);

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
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder AddDelegatingHandler(Func<ILogHandler, DelegatingHandler> delegatingHandlerFactory);

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
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandler">An <see cref="ICacheHandler"/> mapping implementation instance</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithCacheHandler(ICacheHandler cacheHandler);

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandlerFactory">An <see cref="ICacheHandler"/> mapping implementation instance factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithCacheHandler(Func<ICacheHandler> cacheHandlerFactory);

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

        /// <summary>
        /// Provide a mapping handler to map entities
        /// </summary>
        /// <param name="mappingHandler">An <see cref="IMappingHandler"/> mapping implementation instance</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithMappingHandler(IMappingHandler mappingHandler);

        /// <summary>
        /// Provide a mapping handler to map entities
        /// </summary>
        /// <param name="mappingHandlerFactory">An <see cref="IMappingHandler"/> mapping implementation instance factory</param>
        /// <returns></returns>
        IApizrOptionsBuilder WithMappingHandler(Func<IMappingHandler> mappingHandlerFactory);
    }
}