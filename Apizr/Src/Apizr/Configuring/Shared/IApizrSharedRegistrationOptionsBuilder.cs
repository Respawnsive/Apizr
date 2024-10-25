using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
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
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Replace)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Replace)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Define your web api base path (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="basePathFactory">Your web api base path factory</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Replace)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBasePath(Func<string> basePathFactory, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

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
        TApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<ILogger, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory)
            where TAuthenticationHandler : AuthenticationHandlerBase;

        /// <summary>
        /// Provide your own settings management service
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting constant token)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="getTokenExpression">The get only token expression</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression);

        /// <summary>
        /// Provide your own settings management service
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="getTokenExpression">The get token expression</param>
        /// <param name="setTokenExpression">The set token expression</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression);

        /// <summary>
        /// Provide your own token management services
        /// </summary>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="tokenService">A <typeparamref name="TTokenService"/> instance</param>
        /// <param name="refreshTokenExpression">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TTokenService>(
            TTokenService tokenService,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="getTokenExpression">The get token expression</param>
        /// <param name="setTokenExpression">The set token expression</param>
        /// <param name="tokenService">A <typeparamref name="TTokenService"/> instance</param>
        /// <param name="refreshTokenExpression">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            TTokenService tokenService,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TAuthService">Your auth management service (saving/getting/refreshing token)</typeparam>
        /// <param name="authService">A <typeparamref name="TAuthService"/> instance</param>
        /// <param name="getTokenExpression">The get token expression</param>
        /// <param name="setTokenExpression">The set token expression</param>
        /// <param name="refreshTokenExpression">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TAuthService>(
            TAuthService authService,
            Expression<Func<TAuthService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression);

        /// <summary>
        /// Provide your own settings management service with its token source
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting token)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="tokenPropertyExpression">The token property to get from and set to</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenPropertyExpression);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="tokenPropertyExpression">The token property to get from and set to</param>
        /// <param name="tokenService">A <typeparamref name="TTokenService"/> instance</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            TSettingsService settingsService, 
            Expression<Func<TSettingsService, string>> tokenPropertyExpression,
            TTokenService tokenService,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own auth management service
        /// </summary>
        /// <typeparam name="TAuthService">Your auth management service (saving/getting/refreshing token)</typeparam>
        /// <param name="authService">A <typeparamref name="TAuthService"/> instance</param>
        /// <param name="tokenPropertyExpression">The token property to get from and set to</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TAuthService>(
            TAuthService authService,
            Expression<Func<TAuthService, string>> tokenPropertyExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own settings management service
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting constant token)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="getTokenExpression">The get only token expression</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression);

        /// <summary>
        /// Provide your own settings management service
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="getTokenExpression">The get token expression</param>
        /// <param name="setTokenExpression">The set token expression</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression);

        /// <summary>
        /// Provide your own token management services
        /// </summary>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="tokenServiceFactory">A <typeparamref name="TTokenService"/> instance factory</param>
        /// <param name="refreshTokenExpression">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TTokenService>(
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="getTokenExpression">The get token expression</param>
        /// <param name="setTokenExpression">The set token expression</param>
        /// <param name="tokenServiceFactory">A <typeparamref name="TTokenService"/> instance factory</param>
        /// <param name="refreshTokenExpression">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression);

        /// <summary>
        /// Provide your own auth management service
        /// </summary>
        /// <typeparam name="TAuthService">Your auth management service (saving/getting/refreshing token)</typeparam>
        /// <param name="authServiceFactory">A <typeparamref name="TAuthService"/> instance factory</param>
        /// <param name="getTokenExpression">The get token expression</param>
        /// <param name="setTokenExpression">The set token expression</param>
        /// <param name="refreshTokenExpression">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TAuthService>(
            Func<TAuthService> authServiceFactory,
            Expression<Func<TAuthService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression);

        /// <summary>
        /// Provide your own settings management service with its token source
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting token)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="tokenPropertyExpression">The token property to get from and set to</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> tokenPropertyExpression);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="tokenPropertyExpression">The token property to get from and set to</param>
        /// <param name="tokenServiceFactory">A <typeparamref name="TTokenService"/> instance factory</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> tokenPropertyExpression,
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own auth management service
        /// </summary>
        /// <typeparam name="TAuthService">Your auth management service (saving/getting/refreshing token)</typeparam>
        /// <param name="authServiceFactory">A <typeparamref name="TAuthService"/> instance factory</param>
        /// <param name="tokenPropertyExpression">The token property to get from and set to</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler<TAuthService>(
            Func<TAuthService> authServiceFactory,
            Expression<Func<TAuthService, string>> tokenPropertyExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Add a custom delegating handler inheriting from <see cref="DelegatingHandler"/> (serial call)
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Add)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithDelegatingHandler<THandler>(Func<ILogger, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler;

        /// <summary>
        /// Add a custom delegating handler inheriting from <see cref="DelegatingHandler"/> (serial call)
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Add)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithDelegatingHandler<THandler>(Func<ILogger, IApizrManagerOptionsBase, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler;

        /// <summary>
        /// Add a custom http message handler inheriting from <see cref="HttpMessageHandler"/> (last call)
        /// </summary>
        /// <param name="httpMessageHandlerFactory">A http message handler factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHttpMessageHandler<THandler>(Func<ILogger, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler;

        /// <summary>
        /// Add a custom http message handler inheriting from <see cref="HttpMessageHandler"/> (last call)
        /// </summary>
        /// <param name="httpMessageHandlerFactory">A http message handler factory</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHttpMessageHandler<THandler>(Func<ILogger, IApizrManagerOptionsBase, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler;

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
        /// <param name="mode">Set headers right the way or store it for further attribute key match use (default: Set)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHeaders(Func<IList<string>> headersFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set);

        /// <summary>
        /// Add some headers to the request loaded from service properties
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting headers)</typeparam>
        /// <param name="settingsService">A <typeparamref name="TSettingsService"/> instance</param>
        /// <param name="headerProperties">The header properties to get from</param>
        /// <param name="strategy">The duplicate strategy if there's another one already (default: Add)</param>
        /// <param name="scope">Tells Apizr if you want to refresh or not headers values at request time (default: Api = no refresh)</param>
        /// <param name="mode">Set headers right the way or store it for further attribute key match use (default: Set)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHeaders<TSettingsService>(TSettingsService settingsService, 
            Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set);

        /// <summary>
        /// Add some headers to the request loaded from service properties
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting headers)</typeparam>
        /// <param name="settingsServiceFactory">A <typeparamref name="TSettingsService"/> instance factory</param>
        /// <param name="headerProperties">The header properties to get from</param>
        /// <param name="strategy">The duplicate strategy if there's another one already (default: Add)</param>
        /// <param name="scope">Tells Apizr if you want to refresh or not headers values at request time (default: Api = no refresh)</param>
        /// <param name="mode">Set headers right the way or store it for further attribute key match use (default: Set)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHeaders<TSettingsService>(Func<TSettingsService> settingsServiceFactory, 
            Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set);

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

        /// <summary>
        /// Catch potential exceptions
        /// </summary>
        /// <param name="exceptionHandlerFactory">The exception handler called back and returning handled boolean flag Task</param>
        /// <param name="letThrowOnHandledException">Let throw potential exception even if it's handled (default: true)</param>
        /// <param name="strategy">The duplicate strategy if there's another callback already (default: Replace)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithExCatching<THandler>(Func<THandler> exceptionHandlerFactory, bool letThrowOnHandledException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace) where THandler : IApizrExceptionHandler;
    }
}
