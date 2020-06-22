using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
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
    public class ApizrExtendedOptionsBuilder : IApizrExtendedOptionsBuilder
    {
        protected readonly ApizrExtendedOptions Options;

        public ApizrExtendedOptionsBuilder(ApizrExtendedOptions apizrOptions)
        {
            Options = apizrOptions;
        }

        public IApizrExtendedOptions ApizrOptions => Options;

        public IApizrExtendedOptionsBuilder WithBaseAddress(string baseAddress)
        {
            if (Uri.TryCreate(baseAddress, UriKind.RelativeOrAbsolute, out var baseUri))
                Options.BaseAddress = baseUri;

            return this;
        }

        public IApizrExtendedOptionsBuilder WithDecompressionMethods(DecompressionMethods decompressionMethods)
        {
            Options.DecompressionMethods = decompressionMethods;

            return this;
        }

        public IApizrExtendedOptionsBuilder ConfigureHttpClientBuilder(Action<IHttpClientBuilder> httpClientBuilder)
        {
            Options.HttpClientBuilder = httpClientBuilder;

            return this;
        }

        public IApizrExtendedOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            var authenticationHandler = new Func<IServiceProvider, DelegatingHandler>(serviceProvider =>
                new AuthenticationHandler(serviceProvider.GetRequiredService<ILogHandler>(), refreshTokenFactory));
            Options.DelegatingHandlersExtendedFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrExtendedOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<IServiceProvider, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase
        {
            Options.DelegatingHandlersExtendedFactories.Add(authenticationHandlerFactory);

            return this;
        }

        public IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Expression<Func<TSettingsService, string>> tokenProperty,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
        {
            Options.DelegatingHandlersExtendedFactories.Add(serviceProvider =>
                new AuthenticationHandler<TSettingsService, TTokenService>(
                    serviceProvider.GetRequiredService<ILogHandler>(),
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty,
                    serviceProvider.GetRequiredService<TTokenService>, refreshTokenMethod));

            return this;
        }

        public IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService>(Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            Options.DelegatingHandlersExtendedFactories.Add(serviceProvider =>
                new AuthenticationHandler<TSettingsService>(
                    serviceProvider.GetRequiredService<ILogHandler>(),
                    serviceProvider.GetRequiredService<TSettingsService>, tokenProperty, refreshTokenFactory));

            return this;
        }

        public IApizrExtendedOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler)
            => AddDelegatingHandler(_ => delegatingHandler);

        public IApizrExtendedOptionsBuilder AddDelegatingHandler(Func<IServiceProvider, DelegatingHandler> delegatingHandlerFactory)
        {
            Options.DelegatingHandlersExtendedFactories.Add(delegatingHandlerFactory);

            return this;
        }

        public IApizrExtendedOptionsBuilder WithRefitSettings(RefitSettings refitSettings)
            => WithRefitSettings(_ => refitSettings);

        public IApizrExtendedOptionsBuilder WithRefitSettings(
            Func<IServiceProvider, RefitSettings> refitSettingsFactory)
        {
            Options.RefitSettingsFactory = refitSettingsFactory;

            return this;
        }

        public IApizrExtendedOptionsBuilder WithConnectivityHandler<TConnectivityHandler>()
            where TConnectivityHandler : class, IConnectivityHandler
            => WithConnectivityHandler(typeof(TConnectivityHandler));

        public IApizrExtendedOptionsBuilder WithConnectivityHandler(Type connectivityHandlerType)
        {
            if (!typeof(IConnectivityHandler).IsAssignableFrom(connectivityHandlerType))
                throw new ArgumentException(
                    $"Your connectivity handler class must inherit from {nameof(IConnectivityHandler)} interface or derived");

            Options.ConnectivityHandlerType = connectivityHandlerType;

            return this;
        }

        public IApizrExtendedOptionsBuilder WithCacheHandler<TCacheHandler>()
            where TCacheHandler : class, ICacheHandler
            => WithCacheHandler(typeof(TCacheHandler));

        public IApizrExtendedOptionsBuilder WithCacheHandler(Type cacheHandlerType)
        {
            if (!typeof(ICacheHandler).IsAssignableFrom(cacheHandlerType))
                throw new ArgumentException(
                    $"Your cache handler class must inherit from {nameof(ICacheHandler)} interface or derived");

            Options.CacheHandlerType = cacheHandlerType;

            return this;
        }

        public IApizrExtendedOptionsBuilder WithLogHandler<TLogHandler>() where TLogHandler : class, ILogHandler
            => WithLogHandler(typeof(TLogHandler));

        public IApizrExtendedOptionsBuilder WithLogHandler(Type logHandlerType)
        {
            if (!typeof(ILogHandler).IsAssignableFrom(logHandlerType))
                throw new ArgumentException(
                    $"Your log handler class must inherit from {nameof(ILogHandler)} interface or derived");

            Options.LogHandlerType = logHandlerType;

            return this;
        }
    }
}
