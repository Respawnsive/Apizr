using System.Net.Http;
using System.Threading.Tasks;

namespace Apizr.Authenticating
{
    public interface IAuthenticationHandler
    {
        /// <summary>
        /// The method called to get token from settings
        /// </summary>
        /// <returns></returns>
        string GetToken();

        /// <summary>
        /// The method called to save token into settings
        /// </summary>
        /// <param name="token">The token to save</param>
        void SetToken(string token);

        /// <summary>
        /// The method called to refresh token when rejected or empty
        /// </summary>
        /// <param name="request">The request to authenticate</param>
        /// <returns></returns>
        Task<string> RefreshTokenAsync(HttpRequestMessage request);
    }
}