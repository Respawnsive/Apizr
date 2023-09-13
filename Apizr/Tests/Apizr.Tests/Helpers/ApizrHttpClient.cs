using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Policing;

namespace Apizr.Tests.Helpers
{
    public class ApizrHttpClient : HttpClient
    {
        public ApizrHttpClient(HttpMessageHandler handler) : base(handler) { }

        /// <inheritdoc />
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = request.GetApizrRequestOptions();
            CancellationTokenSource cts = null;
            if (options != null)
                cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, options.CancellationToken);

            return base.SendAsync(request, cts?.Token ?? cancellationToken);
        }
    }
}
