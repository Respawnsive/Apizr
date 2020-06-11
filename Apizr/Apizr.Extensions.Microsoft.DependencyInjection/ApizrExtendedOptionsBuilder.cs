using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Connecting;
using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public class ApizrExtendedOptionsBuilder : ApizrOptionsBuilderBase<ApizrExtendedOptions>, IApizrExtendedOptionsBuilder
    {
        public ApizrExtendedOptionsBuilder(ApizrExtendedOptions apizrOptions) : base(apizrOptions)
        {
        }

        public new IApizrExtendedOptions ApizrOptions => Options;

        public IApizrExtendedOptionsBuilder ConfigureHttpClientBuilder(Action<IHttpClientBuilder> httpClientBuilder)
        {
            Options.HttpClientBuilder = httpClientBuilder;

            return this;
        }

        public IApizrExtendedOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            var authenticationHandler = new Func<IServiceProvider, DelegatingHandler>(_ =>
                new AuthenticationHandler(refreshTokenFactory));
            Options.DelegatingHandlersExtendedFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrExtendedOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase
        {
            var authenticationHandlerExtendedFactory = new Func<IServiceProvider, TAuthenticationHandler>(_ => authenticationHandlerFactory.Invoke());
            Options.DelegatingHandlersExtendedFactories.Add(authenticationHandlerExtendedFactory);

            return this;
        }

        public IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Expression<Func<TSettingsService, string>> tokenProperty,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
        {
            Options.DelegatingHandlersExtendedFactories.Add(provider =>
                new AuthenticationHandler<TSettingsService, TTokenService>(
                    provider.GetRequiredService<TSettingsService>, tokenProperty,
                    provider.GetRequiredService<TTokenService>, refreshTokenMethod));

            return this;
        }

        public IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService>(Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            Options.DelegatingHandlersExtendedFactories.Add(provider =>
                new AuthenticationHandler<TSettingsService>(
                    provider.GetRequiredService<TSettingsService>, tokenProperty, refreshTokenFactory));

            return this;
        }

        public IApizrExtendedOptionsBuilder WithPolicyRegistry(Func<IPolicyRegistry<string>> policyRegistryFactory)
        {
            Options.PolicyRegistryFactory = policyRegistryFactory;

            return this;
        }

        public IApizrExtendedOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory)
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

        public IApizrExtendedOptionsBuilder WithCacheProvider<TCacheProvider>()
            where TCacheProvider : class, ICacheProvider
            => WithCacheProvider(typeof(TCacheProvider));

        public IApizrExtendedOptionsBuilder WithCacheProvider(Type cacheProviderType)
        {
            if (!typeof(ICacheProvider).IsAssignableFrom(cacheProviderType))
                throw new ArgumentException(
                    $"Your cache provider class must inherit from {nameof(ICacheProvider)} interface or derived");

            Options.CacheProviderType = cacheProviderType;

            return this;
        }
    }
}
