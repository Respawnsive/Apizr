using Apizr.Configuring.Manager;
using Apizr.Helping;
using Apizr.Policing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Apizr.Extending
{
    internal static class HttpRequestMessageExtensions
    {
        internal static CancellationTokenSource ProcessApizrOptions(this HttpRequestMessage request, CancellationToken cancellationToken, IApizrManagerOptionsBase apizrOptions, out CancellationToken optionsCancellationToken)
        {
            CancellationTokenSource cts = null;

            var options = request.GetApizrRequestOptions();
            if (options != null && (!options.HandlersParameters.TryGetValue(Constants.ApizrOptionsProcessedKey,
                    out var optionsProcessedValue) || optionsProcessedValue is false))
            {
                // Set options as processed yet
                options.HandlersParameters[Constants.ApizrOptionsProcessedKey] = true;

                // Get a configured logger instance
                var context = request.GetOrBuildApizrPolicyExecutionContext();
                if (!context.TryGetLogger(out var logger, out var logLevels, out _, out _))
                {
                    logger = apizrOptions.Logger;
                    logLevels = apizrOptions.LogLevels;
                }

                // Set optionCancellationToken out param
                optionsCancellationToken = options.CancellationToken;

                // Merge cancellation tokens
                cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, optionsCancellationToken);

                // Set a timeout if provided
                if (options.Timeout.HasValue)
                {
                    if (options.Timeout.Value > TimeSpan.Zero)
                    {
                        cts.CancelAfter(options.Timeout.Value);
                        logger.Log(logLevels.Low(), "{0}: Timeout has been set with your provided {1} value.", context.OperationKey, options.Timeout);
                    }
                    else
                    {
                        logger.Log(logLevels.Low(),
                            "{0}: You provided a timeout value which is not a positive TimeSpan (or Timeout.InfiniteTimeSpan to indicate no timeout). Default value will be applied.",
                            context.OperationKey);
                    }
                }

                if (options.Headers?.Count > 0)
                {
                    // Cloned and adjusted from Refit
                    // We could have content headers, so we need to make
                    // sure we have an HttpContent object to add them to,
                    // provided the HttpClient will allow it for the method
                    if (request.Content == null && !Constants.BodylessMethods.Contains(request.Method))
                        request.Content = new ByteArrayContent(Array.Empty<byte>());

                    foreach (var header in options.Headers)
                    {
                        if (string.IsNullOrWhiteSpace(header)) continue;

                        // NB: Silverlight doesn't have an overload for String.Split()
                        // with a count parameter, but header values can contain
                        // ':' so we have to re-join all but the first part to get the
                        // value.
                        var parts = header.Split(':');
                        var headerKey = parts[0].Trim();
                        var headerValue = parts.Length > 1 ?
                            string.Join(":", parts.Skip(1)).Trim() : null;

                        request.SetHeader(headerKey, headerValue);
                        logger.Log(logLevels.Low(), "{0}: Header {1} has been set with your provided {2} value.", context.OperationKey, headerKey, headerValue);
                    }
                }
            }
            else
            {
                optionsCancellationToken = cancellationToken;
            }

            return cts;
        }
    }
}
