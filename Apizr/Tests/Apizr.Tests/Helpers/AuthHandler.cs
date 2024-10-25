using System.Net.Http;
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

        /// <inheritdoc />
        public override Task<string> GetTokenAsync() => _tokenService.GetTokenAsync();

        /// <inheritdoc />
        public override Task SetTokenAsync(string token) => _tokenService.SetTokenAsync(token);

        /// <inheritdoc />
        public override Task<string> RefreshTokenAsync(HttpRequestMessage request) => _tokenService.RefreshTokenAsync(request);
    }
}
