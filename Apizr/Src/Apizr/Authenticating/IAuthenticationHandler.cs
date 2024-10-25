using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr.Authenticating
{
    /// <summary>
    /// The authentication handler definition
    /// </summary>
    public interface IAuthenticationHandler
    {
        /// <summary>
        /// The method called to get local token
        /// </summary>
        /// <param name="request">The request to authenticate</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>The saved token</returns>
        Task<string> GetTokenAsync(HttpRequestMessage request, CancellationToken ct = default);

        /// <summary>
        /// The method called to set local token
        /// </summary>
        /// <param name="request">The request to authenticate</param>
        /// <param name="token">The token to save</param>
        /// <param name="ct">The cancellation token</param>
        Task SetTokenAsync(HttpRequestMessage request, string token, CancellationToken ct = default);

        /// <summary>
        /// The method called to refresh token when rejected or empty
        /// </summary>
        /// <param name="request">The request to authenticate</param>
        /// <param name="token">The former token</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>The refreshed token</returns>
        Task<string> RefreshTokenAsync(HttpRequestMessage request, string token, CancellationToken ct = default);
    }
}