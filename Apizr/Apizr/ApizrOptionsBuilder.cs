using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Refit;

namespace Apizr
{
    public class ApizrOptionsBuilder : ApizrOptionsBuilderBase<ApizrOptions>, IApizrOptionsBuilder
    {
        internal ApizrOptionsBuilder(ApizrOptions apizrOptions) : base(apizrOptions)
        {
        }

        public IApizrOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory)
        {
            Options.RefitSettingsFactory = refitSettingsFactory;

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<TAuthenticationHandler> authenticationHandler) where TAuthenticationHandler : AuthenticationHandlerBase
        {
            Options.AuthenticationHandlerFactory = authenticationHandler;

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            Options.AuthenticationHandlerFactory = () =>
                new AuthenticationHandler(refreshToken);

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            Options.AuthenticationHandlerFactory = () =>
                new AuthenticationHandler<TSettingsService>(settingsServiceFactory, tokenProperty, refreshToken);

            return this;
        }

        public IApizrOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Func<TSettingsService> settingsServiceFactory, Expression<Func<TSettingsService, string>> tokenProperty,
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
        {
            Options.AuthenticationHandlerFactory = () =>
                new AuthenticationHandler<TSettingsService, TTokenService>(
                    settingsServiceFactory, tokenProperty,
                    tokenServiceFactory, refreshTokenMethod);

            return this;
        }
    }
}
