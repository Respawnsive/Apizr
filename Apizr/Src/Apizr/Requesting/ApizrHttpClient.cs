using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Apizr.Extending;

namespace Apizr.Requesting
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
                    when (!optionsCancellationToken.IsCancellationRequested) // Not a user cancellation
                {
                    throw new TimeoutException("Request timed out");
                }
            }
        }
    }
}
