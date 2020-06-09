using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Connecting;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public class ApizrOptionsBuilder : ApizrOptionsBuilderBase<ApizrOptions>, IApizrOptionsBuilder
    {
        internal ApizrOptionsBuilder(ApizrOptions apizrOptions) : base(apizrOptions)
        {
        }

        public IApizrOptionsBuilder WithRefitSettings(RefitSettings refitSettings)
            => WithRefitSettings(() => refitSettings);

        public IApizrOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory)
        {
            Options.RefitSettingsFactory = refitSettingsFactory;

            return this;
        }

        public IApizrOptionsBuilder WithPolicyRegistry(IReadOnlyPolicyRegistry<string> policyRegistry)
            => WithPolicyRegistry(() => policyRegistry);

        public IApizrOptionsBuilder WithPolicyRegistry(Func<IReadOnlyPolicyRegistry<string>> policyRegistryFactory)
        {
            Options.PolicyRegistryFactory = policyRegistryFactory;

            return this;
        }

        public IApizrOptionsBuilder WithConnectivityProvider(IConnectivityProvider connectivityProvider)
            => WithConnectivityProvider(() => connectivityProvider);

        public IApizrOptionsBuilder WithConnectivityProvider(Func<IConnectivityProvider> connectivityProviderFactory)
        {
            Options.ConnectivityProviderFactory = connectivityProviderFactory;

            return this;
        }

        public IApizrOptionsBuilder WithCacheProvider(ICacheProvider cacheProvider)
            => WithCacheProvider(() => cacheProvider);

        public IApizrOptionsBuilder WithCacheProvider(Func<ICacheProvider> cacheProviderFactory)
        {
            Options.CacheProviderFactory = cacheProviderFactory;

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            TAuthenticationHandler authenticationHandler) where TAuthenticationHandler : AuthenticationHandlerBase
            => WithAuthenticationHandler(() => authenticationHandler);

        public IApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<TAuthenticationHandler> authenticationHandler) where TAuthenticationHandler : AuthenticationHandlerBase
        {
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            var authenticationHandler = new Func<DelegatingHandler>(() =>
                new AuthenticationHandler(refreshToken));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshToken)
            => WithAuthenticationHandler(() => settingsService, tokenProperty, refreshToken);

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            var authenticationHandler = new Func<DelegatingHandler>(() =>
                new AuthenticationHandler<TSettingsService>(settingsServiceFactory, tokenProperty, refreshToken));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty, TTokenService tokenService,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
            => WithAuthenticationHandler(() => settingsService, tokenProperty, () => tokenService, refreshTokenMethod);

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty,
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
        {
            var authenticationHandler = new Func<DelegatingHandler>(() =>
                new AuthenticationHandler<TSettingsService, TTokenService>(
                    settingsServiceFactory, tokenProperty,
                    tokenServiceFactory, refreshTokenMethod));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }
    }
}
