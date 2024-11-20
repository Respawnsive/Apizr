using Apizr.Authenticating;
using Apizr.Configuring.Manager;

namespace Apizr.Sample.MAUI.Infrastructure
{
    public class AuthenticationHandler<TWebApi> : AuthenticationHandlerBase
    {
        private readonly ISecureStorage _secureStorage;
        private readonly Lazy<IApizrManager<IHttpBinService>> _httpBinManager;

        /// <inheritdoc />
        public AuthenticationHandler(
            IApizrManagerOptions<TWebApi> apizrOptions,
            ISecureStorage secureStorage, 
            Lazy<IApizrManager<IHttpBinService>> httpBinManager) : base(apizrOptions)
        {
            _secureStorage = secureStorage;
            _httpBinManager = httpBinManager;
        }

        /// <inheritdoc />
        public override Task<string> GetTokenAsync(HttpRequestMessage request, CancellationToken ct = default) =>
            _secureStorage.GetAsync("AccessToken");

        /// <inheritdoc />
        public override Task SetTokenAsync(HttpRequestMessage request, string token, CancellationToken ct = default) =>
            _secureStorage.SetAsync("AccessToken", token);

        /// <inheritdoc />
        public override Task<string> RefreshTokenAsync(HttpRequestMessage request, string token, CancellationToken ct = default)
        {
            var httpBinManager = _httpBinManager.Value;
            // Try to refresh token here

            return Task.FromResult("tokenValue");
        }
    }
}
