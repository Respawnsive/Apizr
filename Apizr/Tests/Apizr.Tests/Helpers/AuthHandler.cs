using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Configuring.Manager;
using Apizr.Extending.Authenticating;
using Microsoft.Extensions.Logging;

namespace Apizr.Tests.Helpers
{
    internal class AuthHandler<TWebApi> : AuthenticationHandlerBase<TWebApi>
    {
        private readonly TokenService _tokenService;

        /// <inheritdoc />
        public AuthHandler(ILogger<TWebApi> logger, IApizrManagerOptions<TWebApi> apizrOptions, TokenService tokenService) : base(logger, apizrOptions)
        {
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        internal AuthHandler(ILogger logger, IApizrManagerOptionsBase apizrOptions, TokenService tokenService) : base(logger, new ApizrManagerOptions<TWebApi>(apizrOptions))
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
