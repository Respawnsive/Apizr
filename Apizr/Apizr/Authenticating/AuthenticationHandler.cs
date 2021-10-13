using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Apizr.Configuring;
using Microsoft.Extensions.Logging;

namespace Apizr.Authenticating
{
    public class AuthenticationHandler : AuthenticationHandlerBase
    {
        private readonly Func<HttpRequestMessage, Task<string>> _refreshToken;

        public AuthenticationHandler(ILogger logger, IApizrOptionsBase apizrOptions, Func<HttpRequestMessage, Task<string>> refreshToken) : base(logger, apizrOptions)
        {
            _refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        public override string GetToken() => null;

        public override void SetToken(string token) { }

        public override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _refreshToken(request);
    }

    public class AuthenticationHandler<TSettingsService> : AuthenticationHandlerBase
    {
        private readonly Func<TSettingsService> _settingsServiceFactory;
        private readonly Expression<Func<TSettingsService, string>> _settingsTokenProperty;
        private readonly Func<HttpRequestMessage, Task<string>> _refreshToken;

        public AuthenticationHandler(ILogger logger, IApizrOptionsBase apizrOptions, Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> settingsTokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshToken) : base(logger, apizrOptions)
        {
            _settingsServiceFactory = settingsServiceFactory ?? throw new ArgumentNullException(nameof(settingsServiceFactory));
            _settingsTokenProperty = settingsTokenProperty ?? throw new ArgumentNullException(nameof(settingsTokenProperty));
            _refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        public override string GetToken() => _settingsTokenProperty.Compile()(_settingsServiceFactory.Invoke());

        public override void SetToken(string token)
        {
            if (_settingsTokenProperty.Body is MemberExpression propertyBody)
            {
                if (propertyBody.Member is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(_settingsServiceFactory.Invoke(), token, null);
                }
            }
        }

        public override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _refreshToken(request);
    }

    public class AuthenticationHandler<TSettingsService, TTokenService> : AuthenticationHandlerBase
    {
        private readonly Func<TSettingsService> _settingsServiceFactory;
        private readonly Expression<Func<TSettingsService, string>> _settingsTokenProperty;
        private readonly Func<TTokenService> _tokenServiceFactory;
        private readonly Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> _refreshTokenMethod;

        public AuthenticationHandler(ILogger logger, IApizrOptionsBase apizrOptions, Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> settingsTokenProperty, Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod) : base(logger, apizrOptions)
        {
            _settingsServiceFactory = settingsServiceFactory ?? throw new ArgumentNullException(nameof(settingsServiceFactory));
            _settingsTokenProperty = settingsTokenProperty ?? throw new ArgumentNullException(nameof(settingsTokenProperty));
            _tokenServiceFactory = tokenServiceFactory ?? throw new ArgumentNullException(nameof(tokenServiceFactory));
            _refreshTokenMethod = refreshTokenMethod ?? throw new ArgumentNullException(nameof(refreshTokenMethod));
        }

        public override string GetToken() => _settingsTokenProperty.Compile()(_settingsServiceFactory.Invoke());

        public override void SetToken(string token)
        {
            if (_settingsTokenProperty.Body is MemberExpression propertyBody)
            {
                if (propertyBody.Member is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(_settingsServiceFactory.Invoke(), token, null);
                }
            }
        }

        public override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _refreshTokenMethod.Compile()(_tokenServiceFactory.Invoke(), request);
    }
}
