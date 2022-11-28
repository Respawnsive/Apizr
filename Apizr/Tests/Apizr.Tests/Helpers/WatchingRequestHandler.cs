using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Policing;
using Polly;

namespace Apizr.Tests.Helpers
{
    public class WatchingRequestHandler : DelegatingHandler
    {
        public Context Context { get; set; }

        public IApizrRequestOptions Options { get; set; }


        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Context = request.GetApizrPolicyExecutionContext();
            Options = request.GetApizrRequestOptions();

            return base.SendAsync(request, cancellationToken);
        }
    }
}