using System;
using System.Net.Http;
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
        /// <returns></returns>
        Task<string> GetTokenAsync();

        /// <summary>
        /// The method called to set local token
        /// </summary>
        /// <param name="token">The token to set</param>
        Task SetTokenAsync(string token);

        /// <summary>
        /// The method called to refresh token when rejected or empty
        /// </summary>
        /// <param name="request">The request to authenticate</param>
        /// <returns></returns>
        Task<string> RefreshTokenAsync(HttpRequestMessage request);
    }
}