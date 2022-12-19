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
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Context = request.GetApizrPolicyExecutionContext();
            Options = request.GetApizrRequestOptions();

            if (Options.CancellationToken != cancellationToken)
                await Task.Delay(5000, cancellationToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}