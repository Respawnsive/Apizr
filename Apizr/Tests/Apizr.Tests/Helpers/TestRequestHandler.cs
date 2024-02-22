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
    public class TestRequestHandler : DelegatingHandler
    {
        public int Attempts { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Attempts++;

            if (request.Options.TryGetValue(new HttpRequestOptionsKey<HttpStatusCode>(nameof(HttpStatusCode)),
                    out var httpStatusCode) && httpStatusCode != HttpStatusCode.OK)
                return await Task.FromResult(new HttpResponseMessage(httpStatusCode));

            if (request.Options.TryGetValue(new HttpRequestOptionsKey<TimeSpan>(nameof(Task.Delay)),
                    out var delay) && delay > TimeSpan.Zero)
                await Task.Delay(delay, cancellationToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
