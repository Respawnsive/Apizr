using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Shiny.WebApi.Authenticating
{
    public class AuthenticationHandler : AuthenticationHandlerBase
    {
        readonly Func<HttpRequestMessage, Task<string?>> refreshToken;

        public AuthenticationHandler(Func<HttpRequestMessage, Task<string?>> refreshToken)
        {
            this.refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        protected override string? GetToken() => null;

        protected override void SetToken(string? token) { }

        protected override Task<string?> RefreshTokenAsync(HttpRequestMessage request) => this.refreshToken(request);
    }

    public class AuthenticationHandler<TSettingsService> : AuthenticationHandlerBase
    {
        readonly TSettingsService settingsService;
        readonly Expression<Func<TSettingsService, string?>> settingsTokenProperty;
        readonly Func<HttpRequestMessage, Task<string?>> refreshToken;

        public AuthenticationHandler(TSettingsService settingsService, Expression<Func<TSettingsService, string?>> settingsTokenProperty, Func<HttpRequestMessage, Task<string?>> refreshToken)
        {
            this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            this.settingsTokenProperty = settingsTokenProperty ?? throw new ArgumentNullException(nameof(settingsTokenProperty));
            this.refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        protected override string? GetToken() => this.settingsTokenProperty.Compile()(this.settingsService);

        protected override void SetToken(string? token)
        {
            if (this.settingsTokenProperty.Body is MemberExpression propertyBody)
            {
                if (propertyBody.Member is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(this.settingsService, token, null);
                }
            }
        }

        protected override Task<string?> RefreshTokenAsync(HttpRequestMessage request) => this.refreshToken(request);
    }

    public class AuthenticationHandler<TSettingsService, TTokenService> : AuthenticationHandlerBase
    {
        readonly TSettingsService settingsService;
        readonly Expression<Func<TSettingsService, string?>> settingsTokenProperty;
        readonly TTokenService tokenService;
        readonly Expression<Func<TTokenService, HttpRequestMessage, Task<string?>>> refreshTokenMethod;

        public AuthenticationHandler(TSettingsService settingsService, Expression<Func<TSettingsService, string?>> settingsTokenProperty, TTokenService tokenService, Expression<Func<TTokenService, HttpRequestMessage, Task<string?>>> refreshTokenMethod)
        {
            this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            this.settingsTokenProperty = settingsTokenProperty ?? throw new ArgumentNullException(nameof(settingsTokenProperty));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this.refreshTokenMethod = refreshTokenMethod ?? throw new ArgumentNullException(nameof(refreshTokenMethod));
        }

        protected override string? GetToken() => this.settingsTokenProperty.Compile()(this.settingsService);

        protected override void SetToken(string? token)
        {
            if (this.settingsTokenProperty.Body is MemberExpression propertyBody)
            {
                if (propertyBody.Member is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(this.settingsService, token, null);
                }
            }
        }

        protected override Task<string?> RefreshTokenAsync(HttpRequestMessage request) => this.refreshTokenMethod.Compile()(this.tokenService, request);
    }
}
