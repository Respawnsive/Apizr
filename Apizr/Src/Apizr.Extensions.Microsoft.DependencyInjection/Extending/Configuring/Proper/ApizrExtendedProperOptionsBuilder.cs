using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Extending.Configuring.Shared;
using Apizr.Logging;
using Apizr.Resiliencing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Extending.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for extended registrations
    /// </summary>
    public class ApizrExtendedProperOptionsBuilder : IApizrExtendedProperOptionsBuilder, IApizrInternalRegistrationOptionsBuilder
    {
        protected readonly ApizrExtendedProperOptions Options;

        internal ApizrExtendedProperOptionsBuilder(ApizrExtendedProperOptions properOptions)
        {
            Options = properOptions;
        }
        
        /// <inheritdoc />
        IApizrExtendedProperOptions IApizrExtendedProperOptionsBuilder.ApizrOptions => Options;

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(string baseAddress)
            => WithBaseAddress(_ => baseAddress);

        /// <inheritdoc />
        IApizrExtendedProperOptionsBuilder
            IApizrGlobalSharedRegistrationOptionsBuilderBase<IApizrExtendedProperOptions,
                IApizrExtendedProperOptionsBuilder>.WithBaseAddress(string baseAddress, ApizrDuplicateStrategy strategy)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.BaseAddressFactory ??= _ => baseAddress;
                    break;
                default:
                    Options.BaseAddressFactory = _ => baseAddress;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Uri baseAddress)
            => WithBaseAddress(_ => baseAddress);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory)
        {
            Options.BaseUriFactory = serviceProvider =>
                Uri.TryCreate(baseAddressFactory.Invoke(serviceProvider), UriKind.RelativeOrAbsolute, out var baseUri)
                    ? baseUri
                    : null;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory)
        {
            Options.BaseUriFactory = baseAddressFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBasePath(string basePath)
            => WithBasePath(_ => basePath);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBasePath(Func<IServiceProvider, string> basePathFactory)
        {
            Options.BasePathFactory = basePathFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(_ => httpClientHandler);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpClientHandler(Func<IServiceProvider, HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder ConfigureHttpClientBuilder(
            Action<IHttpClientBuilder> httpClientBuilder, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HttpClientBuilder ??= httpClientBuilder;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HttpClientBuilder = httpClientBuilder;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HttpClientBuilder == null)
                    {
                        Options.HttpClientBuilder = httpClientBuilder;
                    }
                    else
                    {
                        Options.HttpClientBuilder += httpClientBuilder.Invoke;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => AddDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler(serviceProvider.GetService<ILogger>(), options, refreshTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory)
            where TAuthenticationHandler : AuthenticationHandlerBase
            => AddDelegatingHandler(authenticationHandlerFactory);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Expression<Func<TSettingsService, string>> tokenProperty,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
            => AddDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService, TTokenService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty,
                    serviceProvider.GetRequiredService<TTokenService>, refreshTokenMethod));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, string>> tokenProperty)
            => AddDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty,
                    _ => Task.FromResult(
                        tokenProperty.Compile()(serviceProvider.GetRequiredService<TSettingsService>()))));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => AddDelegatingHandler((serviceProvider, options) =>
                new AuthenticationHandler<TSettingsService>(
                    serviceProvider.GetService<ILogger>(),
                    options,
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty, refreshTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHeaders(params string[] headers)
            => WithHeaders(_ => headers?.ToList());

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHeaders(Func<IServiceProvider, IList<string>> headersFactory)
        {
            Options.HeadersFactories.Add(headersFactory);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithOperationTimeout(TimeSpan timeout)
            => WithOperationTimeout(_ => timeout);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithOperationTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory)
        {
            Options.OperationTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithRequestTimeout(TimeSpan timeout)
            => WithRequestTimeout(_ => timeout);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithRequestTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory)
        {
            Options.RequestTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder AddDelegatingHandler<THandler>(THandler delegatingHandler) where THandler : DelegatingHandler
            => AddDelegatingHandler((_, _) => delegatingHandler);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder AddDelegatingHandler<THandler>(Func<IServiceProvider, THandler> delegatingHandlerFactory) where THandler : DelegatingHandler
            => AddDelegatingHandler((serviceProvider, _) => delegatingHandlerFactory(serviceProvider));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder AddDelegatingHandler<THandler>(Func<IServiceProvider, IApizrManagerOptionsBase, THandler> delegatingHandlerFactory) where THandler : DelegatingHandler
        {
            Options.DelegatingHandlersExtendedFactories[typeof(THandler)] = delegatingHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching(Action<ApizrException> onException,
            bool letThrowOnExceptionWithEmptyCache = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.OnException ??= onException;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.OnException = onException;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.OnException == null)
                    {
                        Options.OnException = onException;
                    }
                    else
                    {
                        Options.OnException += onException.Invoke;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            Options.LetThrowOnExceptionWithEmptyCache = letThrowOnExceptionWithEmptyCache;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching<TResult>(Action<ApizrException<TResult>> onException,
            bool letThrowOnExceptionWithEmptyCache = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(ex => onException.Invoke((ApizrException<TResult>)ex), letThrowOnExceptionWithEmptyCache,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHandlerParameter(string key, object value)
        {
            Options.HandlersParameters[key] = value;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, TValue value)
            => WithResilienceProperty(key, _ => value);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, Func<IServiceProvider, TValue> valueFactory)
        {
            ((IApizrExtendedSharedOptions)Options).ResiliencePropertiesExtendedFactories[key.Key] = serviceProvider => valueFactory(serviceProvider);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels)
            => WithLogging(_ => httpTracerMode, _ => trafficVerbosity, _ => logLevels);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLogging(
            Func<IServiceProvider, (HttpTracerMode, HttpMessageParts, LogLevel[])> loggingConfigurationFactory)
            => WithLogging(serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item1,
                serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item2,
                serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item3);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLogging(
            Func<IServiceProvider, HttpTracerMode> httpTracerModeFactory,
            Func<IServiceProvider, HttpMessageParts> trafficVerbosityFactory,
            Func<IServiceProvider, LogLevel[]> logLevelsFactory)
        {
            Options.HttpTracerModeFactory = httpTracerModeFactory;
            Options.TrafficVerbosityFactory = trafficVerbosityFactory;
            Options.LogLevelsFactory = logLevelsFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResilienceContextOptions(Action<IApizrResilienceContextOptionsBuilder> contextOptionsBuilder)
        {
            var options = Options as IApizrGlobalSharedOptionsBase;
            if (options.ContextOptionsBuilder == null)
            {
                options.ContextOptionsBuilder = contextOptionsBuilder;
            }
            else
            {
                options.ContextOptionsBuilder += contextOptionsBuilder.Invoke;
            }

            return this;
        }

        #region Internal

        void IApizrInternalOptionsBuilder.SetHandlerParameter(string key, object value) => WithHandlerParameter(key, value);

        void IApizrInternalRegistrationOptionsBuilder.SetPrimaryHttpMessageHandler(Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> primaryHandlerFactory)
        {
            Options.PrimaryHandlerFactory = primaryHandlerFactory;
        }

        /// <inheritdoc />
        void IApizrInternalRegistrationOptionsBuilder.AddDelegatingHandler<THandler>(Func<IApizrManagerOptionsBase, THandler> handlerFactory) 
            => AddDelegatingHandler((_, opt) => handlerFactory.Invoke(opt));

        #endregion
    }
}
