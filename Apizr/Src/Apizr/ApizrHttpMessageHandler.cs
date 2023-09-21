using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Apizr.Extending;

namespace Apizr
{
    internal sealed class ApizrHttpMessageHandler : DelegatingHandler
    {
        private readonly IApizrManagerOptionsBase _apizrOptions;

        public ApizrHttpMessageHandler(HttpMessageHandler innerHandler, IApizrManagerOptionsBase apizrOptions) :base(innerHandler)
        {
            _apizrOptions = apizrOptions;
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
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
