using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading;
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
        private readonly Func<HttpRequestMessage, CancellationToken, Task<string>> _getTokenFactory;
        private readonly Func<HttpRequestMessage, string, CancellationToken, Task> _setTokenFactory;
        private readonly Func<HttpRequestMessage, string, CancellationToken, Task<string>> _refreshTokenFactory;

        /// <summary>
        /// The authentication handler constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <param name="getTokenFactory">The get token factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationHandler(ILogger logger,
            IApizrManagerOptionsBase apizrOptions,
            Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory) : base(logger, apizrOptions)
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
            Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task> setTokenFactory) : base(logger, apizrOptions)
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
            Func<HttpRequestMessage, string, CancellationToken, Task<string>> refreshTokenFactory) : base(logger, apizrOptions)
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
            Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task> setTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task<string>> refreshTokenFactory) : base(logger, apizrOptions)
        {
            _getTokenFactory = getTokenFactory ?? throw new ArgumentNullException(nameof(getTokenFactory));
            _setTokenFactory = setTokenFactory ?? throw new ArgumentNullException(nameof(setTokenFactory));
            _refreshTokenFactory = refreshTokenFactory ?? throw new ArgumentNullException(nameof(refreshTokenFactory));
        }

        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <inheritdoc />
        public override Task<string> GetTokenAsync(HttpRequestMessage request, CancellationToken ct = default) =>
            _getTokenFactory?.Invoke(request, ct) ?? Task.FromResult<string>(null);

        /// <inheritdoc />
        public override Task SetTokenAsync(HttpRequestMessage request, string token, CancellationToken ct = default) =>
            _setTokenFactory?.Invoke(request, token, ct) ?? Task.CompletedTask;

        /// <inheritdoc />
        public override Task<string> RefreshTokenAsync(HttpRequestMessage request, string token,
            CancellationToken ct = default) =>
            _refreshTokenFactory?.Invoke(request, token, ct) ?? GetTokenAsync(request, ct);
    }
}
