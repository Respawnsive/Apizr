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
    public interface IApizrOptionsBuilder<out TApizrOptions> where TApizrOptions : class, IApizrOptions
    {
        IApizrOptions ApizrOptions { get; }
    }

    public interface IApizrOptionsBuilder : IApizrOptionsBuilder<ApizrOptions>
    {
        IApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshToken);
        IApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(TAuthenticationHandler authenticationHandler) where TAuthenticationHandler : AuthenticationHandlerBase;
        IApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<ILogHandler, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase;
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(TSettingsService settingsService, Expression<Func<TSettingsService, string>> tokenProperty, TTokenService tokenService, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<TTokenService> tokenServiceFactory, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService, Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshToken);
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshToken);
        IApizrOptionsBuilder WithPolicyRegistry(IReadOnlyPolicyRegistry<string> policyRegistry);
        IApizrOptionsBuilder WithPolicyRegistry(Func<IReadOnlyPolicyRegistry<string>> policyRegistryFactory);
        IApizrOptionsBuilder WithRefitSettings(RefitSettings refitSettings);
        IApizrOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory);
        IApizrOptionsBuilder WithConnectivityHandler(IConnectivityHandler connectivityHandler);
        IApizrOptionsBuilder WithConnectivityHandler(Func<IConnectivityHandler> connectivityHandlerFactory);
        IApizrOptionsBuilder WithCacheProvider(ICacheProvider cacheProvider);
        IApizrOptionsBuilder WithCacheProvider(Func<ICacheProvider> cacheProviderFactory);
        IApizrOptionsBuilder WithLogHandler(ILogHandler logHandler);
        IApizrOptionsBuilder WithLogHandler(Func<ILogHandler> logHandlerFactory);
    }
}