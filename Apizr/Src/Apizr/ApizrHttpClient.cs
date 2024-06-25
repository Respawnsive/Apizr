using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Apizr.Extending;
using Polly.Timeout;

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
            using (var cts = request.ProcessRequest(cancellationToken, _apizrOptions, out var optionsCancellationToken))
            {
                try
                {
                    return await base.SendAsync(request, cts?.Token ?? cancellationToken).ConfigureAwait(false);
                }
                catch (TimeoutException ex) 
                    when(ex.InnerException is OperationCanceledException cancelEx) // Actually a user cancellation (iOS)
                {
                    throw cancelEx;
                }
                catch (WebException ex)
                    when (optionsCancellationToken.IsCancellationRequested) // Actually a user cancellation (Android)
                {
                    throw new OperationCanceledException(ex.Message, ex);
                }
                catch (OperationCanceledException ex)
                    when (!optionsCancellationToken.IsCancellationRequested) // Not a user cancellation
                {
                    throw new TimeoutRejectedException("Request timed out", ex);
                }
            }
        }
    }
}
