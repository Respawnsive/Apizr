using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Apizr.Extending;
using Microsoft.Extensions.Logging;

namespace Apizr.Authenticating
{
    /// <summary>
    /// The authentication handler implementation refreshing token
    /// </summary>
    public class AuthenticationHandler : AuthenticationHandlerBase
    {
        private readonly Func<Task<string>> _getTokenFactory;
        private readonly Func<string, Task> _setTokenFactory;
        private readonly Func<HttpRequestMessage, Task<string>> _refreshTokenFactory;

        /// <summary>
        /// The authentication handler constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <param name="getTokenFactory">The get token factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationHandler(ILogger logger,
            IApizrManagerOptionsBase apizrOptions,
            Func<Task<string>> getTokenFactory) : base(logger, apizrOptions)
        {
            _getTokenFactory = getTokenFactory ?? throw new ArgumentNullException(nameof(getTokenFactory));
        }

        /// <summary>
        /// The authentication handler constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <param name="getTokenFactory">The get token factory</param>
        /// <param name="setTokenFactory">The set token factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationHandler(ILogger logger,
            IApizrManagerOptionsBase apizrOptions,
            Func<Task<string>> getTokenFactory,
            Func<string, Task> setTokenFactory) : base(logger, apizrOptions)
        {
            _getTokenFactory = getTokenFactory ?? throw new ArgumentNullException(nameof(getTokenFactory));
            _setTokenFactory = setTokenFactory ?? throw new ArgumentNullException(nameof(setTokenFactory));
        }

        /// <summary>
        /// The authentication handler constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <param name="refreshTokenFactory">The refresh token factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationHandler(ILogger logger, 
            IApizrManagerOptionsBase apizrOptions, 
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory) : base(logger, apizrOptions)
        {
            _refreshTokenFactory = refreshTokenFactory ?? throw new ArgumentNullException(nameof(refreshTokenFactory));
        }

        /// <summary>
        /// The authentication handler constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <param name="refreshTokenFactory">The refresh token factory</param>
        /// <param name="getTokenFactory">The get token factory</param>
        /// <param name="setTokenFactory">The set token factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationHandler(ILogger logger,
            IApizrManagerOptionsBase apizrOptions,
            Func<Task<string>> getTokenFactory,
            Func<string, Task> setTokenFactory,
            Func<HttpRequestMessage, Task<string>> refreshTokenFactory) : base(logger, apizrOptions)
        {
            _getTokenFactory = getTokenFactory ?? throw new ArgumentNullException(nameof(getTokenFactory));
            _setTokenFactory = setTokenFactory ?? throw new ArgumentNullException(nameof(setTokenFactory));
            _refreshTokenFactory = refreshTokenFactory ?? throw new ArgumentNullException(nameof(refreshTokenFactory));
        }

        /// <inheritdoc />
        public override Task<string> GetTokenAsync() => _getTokenFactory?.Invoke() ?? Task.FromResult<string>(null);

        /// <inheritdoc />
        public override Task SetTokenAsync(string token) => _setTokenFactory?.Invoke(token) ?? Task.CompletedTask;

        /// <inheritdoc />
        public override Task<string> RefreshTokenAsync(HttpRequestMessage request) =>
            _refreshTokenFactory?.Invoke(request) ?? GetTokenAsync();
    }
}
