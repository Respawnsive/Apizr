using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Policing;

namespace Apizr
{
    public class ApizrHttpClient : HttpClient
    {
        public ApizrHttpClient()
        {
        }

        public ApizrHttpClient(HttpMessageHandler handler)
            : this(handler, true)
        {
        }

        public ApizrHttpClient(HttpMessageHandler handler, bool disposeHandler)
            : base(handler, disposeHandler)
        {}

        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = request.GetApizrRequestOptions();
            CancellationTokenSource cts = null;
            if (options?.CancellationToken != null)
                cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, options.CancellationToken);

            return base.SendAsync(request, cts?.Token ?? cancellationToken);
        }
    }
}
