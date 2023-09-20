using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Apizr.Extending;

namespace Apizr
{
    public class ApizrHttpClient : HttpClient
    {
        private readonly IApizrManagerOptionsBase _apizrOptions;

        public ApizrHttpClient(HttpMessageHandler handler, bool disposeHandler, IApizrManagerOptionsBase apizrOptions)
            : base(handler, disposeHandler)
        {
            _apizrOptions = apizrOptions;
        }

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var cts = request.ProcessApizrOptions(cancellationToken, _apizrOptions, out var optionsCancellationToken))
            {
                try
                {
                    return await base.SendAsync(request, cts?.Token ?? cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                    when (!cancellationToken.IsCancellationRequested && // Not a Refit cancellation
                          !optionsCancellationToken.IsCancellationRequested) // Neither a user one
                {
                    throw new TimeoutException("Request timed out");
                }
            }
        }
    }
}
