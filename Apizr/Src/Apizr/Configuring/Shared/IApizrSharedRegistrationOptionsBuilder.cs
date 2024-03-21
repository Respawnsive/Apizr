using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Shared.Context;
using Apizr.Logging;
using Apizr.Resiliencing;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Builder options available at both common and proper level for static registrations
    /// </summary>
    public interface IApizrSharedRegistrationOptionsBuilder : IApizrSharedRegistrationOptionsBuilderBase
    {
    }

    /// <inheritdoc cref="IApizrSharedRegistrationOptionsBuilder" />
    public interface IApizrSharedRegistrationOptionsBuilder<out TApizrOptions, out TApizrOptionsBuilder> : 
        IApizrSharedRegistrationOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrSharedRegistrationOptionsBuilder
        where TApizrOptions : IApizrSharedRegistrationOptionsBase
        where TApizrOptionsBuilder : IApizrSharedRegistrationOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory);

        /// <summary>
        /// Define your web api base path (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="basePathFactory">Your web api base path factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBasePath(Func<string> basePathFactory);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandlerFactory">An <see cref="HttpClientHandler"/> instance factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHttpClientHandler(Func<HttpClientHandler> httpClientHandlerFactory);

        /// <summary>
        /// Configure HttpClient
        /// </summary>
        /// <param name="configureHttpClient">The configuration builder</param>
        /// <param name="strategy">The duplicate strategy if there's another one already (default: Merge)</param>
        /// <returns></returns>
        TApizrOptionsBuilder ConfigureHttpClient(Action<HttpClient> configureHttpClient,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge);

        /// <summary>
        /// Provide your own <see cref="AuthenticationHandlerBase"/> implementation factory
        /// </summary>
        /// <typeparam name="TAuthenticationHandler">Your <see cref="AuthenticationHandlerBase"/> implementation</typeparam>
        /// <param name="authenticationHandlerFactory">A <typeparamref name="TAuthenticationHandler"/> instance factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<ILogger, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase;

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
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(TSettingsService settingsService, Expression<Func<TSettingsService, string>> tokenProperty, TTokenService tokenService, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);

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
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<TTokenService> tokenServiceFactory, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own settings management service with its token source
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting token)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="tokenProperty">The token property to get from</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService, Expression<Func<TSettingsService, string>> tokenProperty);

        /// <summary>
        /// Provide your own settings management service and a method to refresh the token
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenFactory">The method factory called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService, Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Provide your own settings management service with its token property
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting token)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="tokenProperty">The token property to get from</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty);

        /// <summary>
        /// Provide your own settings management service and a method to refresh the token
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="tokenProperty">The token property used for saving</param>
        /// <param name="refreshTokenFactory">The method factory called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder AddDelegatingHandler<THandler>(Func<ILogger, THandler> delegatingHandlerFactory) where THandler : DelegatingHandler;

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder AddDelegatingHandler<THandler>(Func<ILogger, IApizrManagerOptionsBase, THandler> delegatingHandlerFactory) where THandler : DelegatingHandler;

        /// <summary>
        /// Define tracer mode, http traffic tracing verbosity and log levels (could be defined with LogAttribute)
        /// </summary>
        /// <param name="httpTracerModeFactory">Http traffic tracing mode</param>
        /// <param name="trafficVerbosityFactory">Http traffic tracing verbosity factory</param>
        /// <param name="logLevelsFactory">Log levels factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithLogging(Func<HttpTracerMode> httpTracerModeFactory,
            Func<HttpMessageParts> trafficVerbosityFactory, Func<LogLevel[]> logLevelsFactory);

        /// <summary>
        /// Define tracer mode, http traffic tracing verbosity and log levels (could be defined with LogAttribute)
        /// </summary>
        /// <param name="loggingConfigurationFactory">Logging configuration factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithLogging(Func<(HttpTracerMode, HttpMessageParts, LogLevel[])> loggingConfigurationFactory);

        /// <summary>
        /// Add some headers to the request
        /// </summary>
        /// <param name="headersFactory">Headers factory</param>
        /// <param name="strategy">The duplicate strategy if there's another one already (default: Add)</param>
        /// <param name="scope">Tells Apizr if you want to refresh or not headers values at request time (default: Api = no refresh)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHeaders(Func<IList<string>> headersFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api);

        /// <summary>
        /// Set a timeout to the operation (overall request tries)
        /// </summary>
        /// <param name="timeoutFactory">The operation timeout factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithOperationTimeout(Func<TimeSpan> timeoutFactory);

        /// <summary>
        /// Set a timeout to the request (each request try)
        /// </summary>
        /// <param name="timeoutFactory">The request timeout factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithRequestTimeout(Func<TimeSpan> timeoutFactory);

        /// <summary>
        /// Set some resilience properties to the resilience context
        /// </summary>
        /// <param name="key">The resilience property's key</param>
        /// <param name="valueFactory">The resilience property's value factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, Func<TValue> valueFactory);
    }
}
