using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Resiliencing;
using Polly;

namespace Apizr.Tests.Helpers
{
    public class WatchingRequestHandler : DelegatingHandler
    {
        public ResilienceContext Context { get; set; }

        public IApizrRequestOptions Options { get; set; }

        public HttpRequestHeaders Headers { get; set; }

        public int Attempts { get; set; }


        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Context = request.GetApizrResilienceContext();
            Options = request.GetApizrRequestOptions();
            Headers = request.Headers;
            Attempts++;

            if (Options?.CancellationToken != CancellationToken.None)
                await Task.Delay(5000, cancellationToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}