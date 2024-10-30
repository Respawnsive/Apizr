using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Authenticating;

namespace Apizr.Tests.Helpers
{
    internal class TokenService : IAuthenticationHandler
    {
        private string _token;

        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <inheritdoc />
        public Task<string> GetTokenAsync(HttpRequestMessage request, CancellationToken ct = default) => Task.FromResult(_token);

        /// <inheritdoc />
        public Task SetTokenAsync(HttpRequestMessage request, string token, CancellationToken ct = default)
        {
            _token = token;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<string> RefreshTokenAsync(HttpRequestMessage request, string token, CancellationToken ct = default) =>
            Task.FromResult(Guid.NewGuid().ToString("N"));
    }
}
