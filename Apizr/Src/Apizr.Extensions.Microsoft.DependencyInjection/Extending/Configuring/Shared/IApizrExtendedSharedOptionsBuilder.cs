﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Extending.Configuring.Shared
{
    /// <summary>
    /// Builder options available at both common and proper level for extended registrations
    /// </summary>
    public interface IApizrExtendedSharedOptionsBuilder<out TApizrExtendedSharedOptions, out TApizrExtendedSharedOptionsBuilder> : 
        IApizrExtendedSharedRegistrationOptionsBuilderBase, 
        IApizrGlobalSharedRegistrationOptionsBuilderBase<TApizrExtendedSharedOptions, TApizrExtendedSharedOptionsBuilder>
        where TApizrExtendedSharedOptions : IApizrSharedRegistrationOptionsBase
        where TApizrExtendedSharedOptionsBuilder : IApizrGlobalSharedRegistrationOptionsBuilderBase<TApizrExtendedSharedOptions, TApizrExtendedSharedOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Replace)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Replace)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Define your web api base path (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="basePathFactory">Your web api base path factory</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Replace)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithBasePath(Func<IServiceProvider, string> basePathFactory, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Define tracer mode, http traffic tracing verbosity and log levels (could be defined with LogAttribute)
        /// </summary>
        /// <param name="httpTracerModeFactory">Http traffic tracing mode factory</param>
        /// <param name="trafficVerbosityFactory">Http traffic tracing verbosity factory</param>
        /// <param name="logLevelsFactory">Log levels factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithLogging(Func<IServiceProvider, HttpTracerMode> httpTracerModeFactory, Func<IServiceProvider, HttpMessageParts> trafficVerbosityFactory, Func<IServiceProvider, LogLevel[]> logLevelsFactory);

        /// <summary>
        /// Define tracer mode, http traffic tracing verbosity and log levels (could be defined with LogAttribute)
        /// </summary>
        /// <param name="loggingConfigurationFactory">Logging configuration factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithLogging(Func<IServiceProvider, (HttpTracerMode, HttpMessageParts, LogLevel[])> loggingConfigurationFactory);

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
        /// <param name="strategy">The duplicate strategy if there's another one already (default: Merge)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder ConfigureHttpClientBuilder(Action<IHttpClientBuilder> httpClientBuilder,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge);

        /// <summary>
        /// Add a custom delegating handler inheriting from <see cref="DelegatingHandler"/> (serial call)
        /// </summary>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Add)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithDelegatingHandler<THandler>(
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler;

        /// <summary>
        /// Add a custom delegating handler inheriting from <see cref="DelegatingHandler"/> (serial call)
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Add)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithDelegatingHandler<THandler>(Func<IServiceProvider, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler;

        /// <summary>
        /// Add a custom delegating handler inheriting from <see cref="DelegatingHandler"/> (serial call)
        /// </summary>
        /// <param name="delegatingHandlerFactory">A delegating handler factory</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Add)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithDelegatingHandler<THandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler;

        /// <summary>
        /// Add a custom http message handler inheriting from <see cref="HttpMessageHandler"/> (last call)
        /// </summary>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithHttpMessageHandler<THandler>()
            where THandler : HttpMessageHandler;

        /// <summary>
        /// Add a custom http message handler inheriting from <see cref="HttpMessageHandler"/> (last call)
        /// </summary>
        /// <param name="httpMessageHandlerFactory">A http message handler factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithHttpMessageHandler<THandler>(
            Func<IServiceProvider, THandler> httpMessageHandlerFactory)
            where THandler : HttpMessageHandler;

        /// <summary>
        /// Add a custom http message handler inheriting from <see cref="HttpMessageHandler"/> (last call)
        /// </summary>
        /// <param name="httpMessageHandlerFactory">A http message handler factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithHttpMessageHandler<THandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, THandler> httpMessageHandlerFactory)
            where THandler : HttpMessageHandler;

        /// <summary>
        /// Provide your own <see cref="AuthenticationHandlerBase"/> implementation
        /// </summary>
        /// <typeparam name="TAuthenticationHandler">Your <see cref="AuthenticationHandlerBase"/> implementation</typeparam>
        /// <param name="authenticationHandlerFactory">A <typeparamref name="TAuthenticationHandler"/> instance factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory)
            where TAuthenticationHandler : AuthenticationHandlerBase;

        /// <summary>
        /// Provide your own <see cref="AuthenticationHandlerBase"/> generic implementation
        /// </summary>
        /// <param name="authenticationHandlerType">Your own <see cref="AuthenticationHandlerBase"/> generic implementation</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler(Type authenticationHandlerType);

        /// <summary>
        /// Provide your own settings management service
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting constant token)</typeparam>
        /// <param name="getTokenExpression">The get only token expression</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression);

        /// <summary>
        /// Provide your own settings management service
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <param name="getTokenExpression">The get token expression</param>
        /// <param name="setTokenExpression">The set token expression</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression);

        /// <summary>
        /// Provide your own token management services
        /// </summary>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="refreshTokenExpression">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TTokenService>(
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="getTokenExpression">The get token expression</param>
        /// <param name="setTokenExpression">The set token expression</param>
        /// <param name="refreshTokenExpression">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TAuthService">Your auth management service (saving/getting/refreshing token)</typeparam>
        /// <param name="getTokenExpression">The get token expression</param>
        /// <param name="setTokenExpression">The set token expression</param>
        /// <param name="refreshTokenExpression">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TAuthService>(
            Expression<Func<TAuthService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression);

        /// <summary>
        /// Provide your own settings management service with its token source
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting token)</typeparam>
        /// <param name="tokenPropertyExpression">The token property to get from and set to</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, string>> tokenPropertyExpression);

        /// <summary>
        /// Provide your own settings management and token management services
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (saving/getting token)</typeparam>
        /// <typeparam name="TTokenService">Your token management service (refreshing token)</typeparam>
        /// <param name="tokenPropertyExpression">The token property to get from and set to</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Expression<Func<TSettingsService, string>> tokenPropertyExpression,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Provide your own auth management service
        /// </summary>
        /// <typeparam name="TAuthService">Your auth management service (saving/getting/refreshing token)</typeparam>
        /// <param name="tokenPropertyExpression">The token property to get from and set to</param>
        /// <param name="refreshTokenMethod">The method called to refresh the token</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithAuthenticationHandler<TAuthService>(
            Expression<Func<TAuthService, string>> tokenPropertyExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenMethod);

        /// <summary>
        /// Add some headers to the request
        /// </summary>
        /// <param name="headersFactory">Headers to add to the request</param>
        /// <param name="strategy">The duplicate strategy if there's another one already (default: Add)</param>
        /// <param name="scope">Tells Apizr if you want to refresh or not headers values at request time (default: Api = no refresh)</param>
        /// <param name="mode">Set headers right the way or store it for further attribute key match use (default: Set)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithHeaders(Func<IServiceProvider, IList<string>> headersFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set);

        /// <summary>
        /// Add some headers to the request loaded from service properties
        /// </summary>
        /// <typeparam name="TSettingsService">Your settings management service (getting headers)</typeparam>
        /// <param name="headerProperties">The header properties to get from</param>
        /// <param name="strategy">The duplicate strategy if there's another one already (default: Add)</param>
        /// <param name="scope">Tells Apizr if you want to refresh or not headers values at request time (default: Api = no refresh)</param>
        /// <param name="mode">Set headers right the way or store it for further attribute key match use (default: Set)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithHeaders<TSettingsService>(Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set);

        /// <summary>
        /// Set a timeout to the operation (overall request tries)
        /// </summary>
        /// <param name="timeoutFactory">The operation timeout factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithOperationTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory);

        /// <summary>
        /// Set a timeout to the request (each request try)
        /// </summary>
        /// <param name="timeoutFactory">The request timeout factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithRequestTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory);

        /// <summary>
        /// Set some resilience properties to the resilience context
        /// </summary>
        /// <param name="key">The resilience property's key</param>
        /// <param name="valueFactory">The resilience property's value factory</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, Func<IServiceProvider, TValue> valueFactory);

        /// <summary>
        /// Catch potential exceptions
        /// </summary>
        /// <param name="onException">The exception callback returning handled boolean flag</param>
        /// <param name="letThrowOnHandledException">Let throw potential exception even if it's handled (default: true)</param>
        /// <param name="strategy">The duplicate strategy if there's another callback already (default: Replace)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithExCatching(Func<IServiceProvider, ApizrException, bool> onException, bool letThrowOnHandledException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Catch potential exceptions
        /// </summary>
        /// <param name="onException">The exception callback returning handled boolean flag</param>
        /// <param name="letThrowOnHandledException">Let throw potential exception even if it's handled (default: true)</param>
        /// <param name="strategy">The duplicate strategy if there's another callback already (default: Replace)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithExCatching<TResult>(Func<IServiceProvider, ApizrException<TResult>, bool> onException, bool letThrowOnHandledException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Catch potential exceptions
        /// </summary>
        /// <param name="onException">The exception callback returning a handled boolean flag Task</param>
        /// <param name="letThrowOnHandledException">Let throw potential exception even if it's handled (default: true)</param>
        /// <param name="strategy">The duplicate strategy if there's another callback already (default: Replace)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithExCatching(Func<IServiceProvider, ApizrException, Task<bool>> onException, bool letThrowOnHandledException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Catch potential exceptions
        /// </summary>
        /// <param name="onException">The exception callback returning a handled boolean flag Task</param>
        /// <param name="letThrowOnHandledException">Let throw potential exception even if it's handled (default: true)</param>
        /// <param name="strategy">The duplicate strategy if there's another callback already (default: Replace)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithExCatching<TResult>(Func<IServiceProvider, ApizrException<TResult>, Task<bool>> onException, bool letThrowOnHandledException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Catch potential exceptions
        /// </summary>
        /// <param name="exceptionHandlerFactory">The exception handler called back and returning handled boolean flag Task</param>
        /// <param name="letThrowOnHandledException">Let throw potential exception even if it's handled (default: true)</param>
        /// <param name="strategy">The duplicate strategy if there's another callback already (default: Replace)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithExCatching<THandler>(Func<IServiceProvider, THandler> exceptionHandlerFactory, bool letThrowOnHandledException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace) where THandler : IApizrExceptionHandler;

        /// <summary>
        /// Catch potential exceptions
        /// </summary>
        /// <param name="letThrowOnHandledException">Let throw potential exception even if it's handled (default: true)</param>
        /// <param name="strategy">The duplicate strategy if there's another callback already (default: Replace)</param>
        /// <returns></returns>
        TApizrExtendedSharedOptionsBuilder WithExCatching<THandler>(bool letThrowOnHandledException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace) where THandler : IApizrExceptionHandler;

    }
}
