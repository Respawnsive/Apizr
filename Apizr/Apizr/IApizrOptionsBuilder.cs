using Apizr.Authenticating;
using Refit;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apizr
{
    public interface IApizrOptionsBuilder<out TApizrOptions> where TApizrOptions : class, IApizrOptions
    {
        IApizrOptions ApizrOptions { get; }
    }

    public interface IApizrOptionsBuilder : IApizrOptionsBuilder<ApizrOptions>
    {
        IApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshToken);
        IApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<TAuthenticationHandler> authenticationHandler) where TAuthenticationHandler : AuthenticationHandlerBase;
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<TTokenService> tokenServiceFactory, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod);
        IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshToken);
        IApizrOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory);
    }
}