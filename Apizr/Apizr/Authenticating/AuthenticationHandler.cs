using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Apizr.Authenticating
{
    public class AuthenticationHandler : AuthenticationHandlerBase
    {
        private readonly Func<HttpRequestMessage, Task<string>> _refreshToken;

        public AuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            _refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        protected override string GetToken() => null;

        protected override void SetToken(string token) { }

        protected override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _refreshToken(request);
    }

    public class AuthenticationHandler<TSettingsService> : AuthenticationHandlerBase
    {
        private readonly TSettingsService _settingsService;
        private readonly Expression<Func<TSettingsService, string>> _settingsTokenProperty;
        private readonly Func<HttpRequestMessage, Task<string>> _refreshToken;

        public AuthenticationHandler(TSettingsService settingsService, Expression<Func<TSettingsService, string>> settingsTokenProperty, Func<HttpRequestMessage, Task<string>> refreshToken)
        {
            if(settingsService == null)
                throw new ArgumentNullException(nameof(settingsService));

            _settingsService = settingsService;
            _settingsTokenProperty = settingsTokenProperty ?? throw new ArgumentNullException(nameof(settingsTokenProperty));
            _refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        protected override string GetToken() => _settingsTokenProperty.Compile()(_settingsService);

        protected override void SetToken(string token)
        {
            if (_settingsTokenProperty.Body is MemberExpression propertyBody)
            {
                if (propertyBody.Member is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(_settingsService, token, null);
                }
            }
        }

        protected override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _refreshToken(request);
    }

    public class AuthenticationHandler<TSettingsService, TTokenService> : AuthenticationHandlerBase
    {
        private readonly TSettingsService _settingsService;
        private readonly Expression<Func<TSettingsService, string>> _settingsTokenProperty;
        private readonly TTokenService _tokenService;
        private readonly Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> _refreshTokenMethod;

        public AuthenticationHandler(TSettingsService settingsService, Expression<Func<TSettingsService, string>> settingsTokenProperty, TTokenService tokenService, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
        {
            if (settingsService == null)
                throw new ArgumentNullException(nameof(settingsService));

            if (tokenService == null)
                throw new ArgumentNullException(nameof(tokenService));

            _settingsService = settingsService;
            _settingsTokenProperty = settingsTokenProperty ?? throw new ArgumentNullException(nameof(settingsTokenProperty));
            _tokenService = tokenService;
            _refreshTokenMethod = refreshTokenMethod ?? throw new ArgumentNullException(nameof(refreshTokenMethod));
        }

        protected override string GetToken() => _settingsTokenProperty.Compile()(_settingsService);

        protected override void SetToken(string token)
        {
            if (_settingsTokenProperty.Body is MemberExpression propertyBody)
            {
                if (propertyBody.Member is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(_settingsService, token, null);
                }
            }
        }

        protected override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _refreshTokenMethod.Compile()(_tokenService, request);
    }
}
