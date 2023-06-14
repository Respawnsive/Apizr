using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Microsoft.Extensions.Logging;

namespace Apizr.Authenticating
{
    /// <summary>
    /// The authentication handler implementation refreshing token
    /// </summary>
    public class AuthenticationHandler : AuthenticationHandlerBase
    {
        private readonly Func<HttpRequestMessage, Task<string>> _refreshToken;

        /// <summary>
        /// The authentication handler constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <param name="refreshTokenFactory">The refresh token factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationHandler(ILogger logger, IApizrManagerOptionsBase apizrOptions, Func<HttpRequestMessage, Task<string>> refreshTokenFactory) : base(logger, apizrOptions)
        {
            _refreshToken = refreshTokenFactory ?? throw new ArgumentNullException(nameof(refreshTokenFactory));
        }

        /// <inheritdoc />
        public override string GetToken() => null;

        /// <inheritdoc />
        public override void SetToken(string token) { }

        /// <inheritdoc />
        public override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _refreshToken(request);
    }

    /// <summary>
    /// The authentication handler implementation refreshing and saving token
    /// </summary>
    /// <typeparam name="TSettingsService">The settings service type</typeparam>
    public class AuthenticationHandler<TSettingsService> : AuthenticationHandlerBase
    {
        private readonly Func<TSettingsService> _settingsServiceFactory;
        private readonly Expression<Func<TSettingsService, string>> _settingsTokenProperty;
        private readonly Func<HttpRequestMessage, Task<string>> _refreshTokenFactory;

        /// <summary>
        /// The authentication handler constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <param name="settingsServiceFactory">The settings service factory</param>
        /// <param name="settingsTokenProperty">The settings service's token property expression</param>
        /// <param name="refreshTokenFactory">The refresh token factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationHandler(ILogger logger, IApizrManagerOptionsBase apizrOptions, Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> settingsTokenProperty,
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory) : base(logger, apizrOptions)
        {
            _settingsServiceFactory = settingsServiceFactory ?? throw new ArgumentNullException(nameof(settingsServiceFactory));
            _settingsTokenProperty = settingsTokenProperty ?? throw new ArgumentNullException(nameof(settingsTokenProperty));
            _refreshTokenFactory = refreshTokenFactory ?? throw new ArgumentNullException(nameof(refreshTokenFactory));
        }

        /// <inheritdoc />
        public override string GetToken() => _settingsTokenProperty.Compile().Invoke(_settingsServiceFactory.Invoke());

        /// <inheritdoc />
        public override void SetToken(string token)
        {
            if (_settingsTokenProperty.Body is MemberExpression {Member: PropertyInfo {CanWrite: true} propertyInfo} && propertyInfo.GetSetMethod(true).IsPublic)
                propertyInfo.SetValue(_settingsServiceFactory.Invoke(), token, null);
        }

        /// <inheritdoc />
        public override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _refreshTokenFactory(request);
    }

    /// <summary>
    /// The authentication handler implementation refreshing and saving token
    /// </summary>
    /// <typeparam name="TSettingsService">The settings service type</typeparam>
    /// <typeparam name="TTokenService">The refresh token service type</typeparam>
    public class AuthenticationHandler<TSettingsService, TTokenService> : AuthenticationHandlerBase
    {
        private readonly Func<TSettingsService> _settingsServiceFactory;
        private readonly Expression<Func<TSettingsService, string>> _settingsTokenProperty;
        private readonly Func<TTokenService> _tokenServiceFactory;
        private readonly Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> _refreshTokenMethod;

        /// <summary>
        /// The authentication handler constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <param name="settingsServiceFactory">The settings service factory</param>
        /// <param name="settingsTokenProperty">The settings service's token property expression</param>
        /// <param name="tokenServiceFactory">The refresh token service factory</param>
        /// <param name="refreshTokenMethod">The refresh token method to be called</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationHandler(ILogger logger, IApizrManagerOptionsBase apizrOptions, Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> settingsTokenProperty, Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod) : base(logger, apizrOptions)
        {
            _settingsServiceFactory = settingsServiceFactory ?? throw new ArgumentNullException(nameof(settingsServiceFactory));
            _settingsTokenProperty = settingsTokenProperty ?? throw new ArgumentNullException(nameof(settingsTokenProperty));
            _tokenServiceFactory = tokenServiceFactory ?? throw new ArgumentNullException(nameof(tokenServiceFactory));
            _refreshTokenMethod = refreshTokenMethod ?? throw new ArgumentNullException(nameof(refreshTokenMethod));
        }

        /// <inheritdoc />
        public override string GetToken() => _settingsTokenProperty.Compile().Invoke(_settingsServiceFactory.Invoke());

        /// <inheritdoc />
        public override void SetToken(string token)
        {
            if (_settingsTokenProperty.Body is MemberExpression {Member: PropertyInfo {CanWrite: true} propertyInfo } && propertyInfo.GetSetMethod(true).IsPublic)
                propertyInfo.SetValue(_settingsServiceFactory.Invoke(), token, null);
        }

        /// <inheritdoc />
        public override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _refreshTokenMethod.Compile().Invoke(_tokenServiceFactory.Invoke(), request);
    }
}
