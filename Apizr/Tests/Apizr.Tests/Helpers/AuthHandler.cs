using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Configuring.Manager;

namespace Apizr.Tests.Helpers
{
    internal class AuthHandler<TWebApi> : AuthenticationHandlerBase
    {
        private readonly TokenService _tokenService;

        /// <inheritdoc />
        public AuthHandler(IApizrManagerOptions<TWebApi> apizrOptions, TokenService tokenService) : base(apizrOptions)
        {
            _tokenService = tokenService;
        }

        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <inheritdoc />
        public override Task<string> GetTokenAsync(HttpRequestMessage request, CancellationToken ct = default) => _tokenService.GetTokenAsync(request, ct);

        /// <inheritdoc />
        public override Task SetTokenAsync(HttpRequestMessage request, string token, CancellationToken ct = default) => _tokenService.SetTokenAsync(request, token, ct);

        /// <inheritdoc />
        public override Task<string> RefreshTokenAsync(HttpRequestMessage request, string token,
            CancellationToken ct = default) => _tokenService.RefreshTokenAsync(request, token, ct);
    }

    internal class AuthHandler : AuthenticationHandlerBase
    {
        private readonly TokenService _tokenService;

        /// <inheritdoc />
        public AuthHandler(IApizrManagerOptionsBase apizrOptions, TokenService tokenService) : base(apizrOptions)
        {
            _tokenService = tokenService;
        }

        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <inheritdoc />
        public override Task<string> GetTokenAsync(HttpRequestMessage request, CancellationToken ct = default) => _tokenService.GetTokenAsync(request, ct);

        /// <inheritdoc />
        public override Task SetTokenAsync(HttpRequestMessage request, string token, CancellationToken ct = default) => _tokenService.SetTokenAsync(request, token, ct);

        /// <inheritdoc />
        public override Task<string> RefreshTokenAsync(HttpRequestMessage request, string token,
            CancellationToken ct = default) => _tokenService.RefreshTokenAsync(request, token, ct);
    }
}
