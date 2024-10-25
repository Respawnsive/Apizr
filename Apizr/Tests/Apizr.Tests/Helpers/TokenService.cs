using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Apizr.Authenticating;

namespace Apizr.Tests.Helpers
{
    internal class TokenService : IAuthenticationHandler
    {
        private string _token;

        /// <inheritdoc />
        public Task<string> GetTokenAsync() => Task.FromResult(_token);

        /// <inheritdoc />
        public Task SetTokenAsync(string token)
        {
            _token = token;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<string> RefreshTokenAsync(HttpRequestMessage request) =>
            Task.FromResult(Guid.NewGuid().ToString("N"));
    }
}
