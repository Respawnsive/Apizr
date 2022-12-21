using Apizr.Policing;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr
{
    internal sealed class ApizrHttpMessageHandler : DelegatingHandler
    {
        public ApizrHttpMessageHandler(HttpMessageHandler innerHandler):base(innerHandler)
        {
            
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = request.GetApizrRequestOptions();
            cancellationToken = options.CancellationToken;

            return base.SendAsync(request, cancellationToken);
        }
    }
}
