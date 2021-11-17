using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr.Tests.Helpers
{
    public class FailingRequestHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return request.Options.TryGetValue(new HttpRequestOptionsKey<HttpStatusCode>(nameof(HttpStatusCode)),
                out var httpStatusCode) && httpStatusCode != HttpStatusCode.OK
                ? Task.FromResult(new HttpResponseMessage(httpStatusCode))
                : base.SendAsync(request, cancellationToken);
        }
    }
}
