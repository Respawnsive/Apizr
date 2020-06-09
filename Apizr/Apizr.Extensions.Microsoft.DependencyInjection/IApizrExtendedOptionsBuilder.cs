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
    public interface IApizrExtendedOptionsBuilder : IApizrOptionsBuilder<ApizrExtendedOptions>
    {
        new IApizrExtendedOptions ApizrOptions { get; }

        IApizrExtendedOptionsBuilder ConfigureHttpClientBuilder(Action<IHttpClientBuilder> httpClientBuilder);
        IApizrExtendedOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory);
        IApizrExtendedOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase;
        IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Expression<Func<TSettingsService, string>> tokenProperty, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);
        IApizrExtendedOptionsBuilder WithAuthenticationHandler<TSettingsService>(Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory);
        IApizrExtendedOptionsBuilder WithPolicyRegistry(Func<IPolicyRegistry<string>> policyRegistryFactory);
        IApizrExtendedOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory);
        IApizrExtendedOptionsBuilder WithConnectivityProvider<TConnectivityProvider>() where TConnectivityProvider : class, IConnectivityProvider;
        IApizrExtendedOptionsBuilder WithConnectivityProvider(Type connectivityProviderType);
        IApizrExtendedOptionsBuilder WithCacheProvider<TCacheProvider>() where TCacheProvider : class, ICacheProvider;
        IApizrExtendedOptionsBuilder WithCacheProvider(Type cacheProviderType);
    }
}
