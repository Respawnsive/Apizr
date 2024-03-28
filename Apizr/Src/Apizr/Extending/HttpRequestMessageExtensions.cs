using Apizr.Configuring.Manager;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Apizr.Logging;
using Apizr.Resiliencing;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending
{
    internal static class HttpRequestMessageExtensions
    {
        private static readonly Func<string, bool> ShouldRedactHeaderValue = (header) => false;

        internal static CancellationTokenSource ProcessApizrOptions(this HttpRequestMessage request, CancellationToken cancellationToken, IApizrManagerOptionsBase apizrOptions, out CancellationToken optionsCancellationToken)
        {
            CancellationTokenSource cts = null;

            var options = request.GetApizrRequestOptions();
            if (options != null)
            {
                // Get a configured logger instance
                var context = request.GetOrBuildApizrResilienceContext(cancellationToken);
                if (!context.TryGetLogger(out var logger, out var logLevels, out _, out _))
                {
                    logger = apizrOptions.Logger;
                    logLevels = apizrOptions.LogLevels;
                }

                var headersSetCount = 0;

                // Handling headers
                if (options.Headers.Count > 0)
                {
                    // Cloned and adjusted from Refit
                    // We could have content headers, so we need to make
                    // sure we have an HttpContent object to add them to,
                    // provided the HttpClient will allow it for the method
                    if (request.Content == null && !Constants.BodylessMethods.Contains(request.Method))
                        request.Content = new ByteArrayContent(Array.Empty<byte>());

                    foreach (var header in options.Headers)
                    {
                        if (string.IsNullOrWhiteSpace(header)) 
                            continue;

                        var headerSet = request.TrySetHeader(header, out var key, out _);
                        if (!headerSet)
                            logger.Log(logLevels.Low(), "{0}: Header {1} can't be set.", context.OperationKey, header);
                        else
                            headersSetCount++;
                    }
                }

                // Handling header store
                if (request.Headers.Any() && options.HeadersStore?.Count > 0)
                {
                    var matchingHeaders = options.HeadersStore.Where(storedHeader =>
                        TryGetHeaderKeyValue(storedHeader, out var storedHeaderkey, out _) &&
                        request.Headers.Any(requestHeader =>
                            storedHeaderkey == requestHeader.Key && 
                            (requestHeader.Value == null || 
                             requestHeader.Value.All(string.IsNullOrWhiteSpace))))
                        .ToList();

                    foreach (var matchingHeader in matchingHeaders)
                    {
                        var headerSet = request.TrySetHeader(matchingHeader, out var key, out _);
                        if (!headerSet)
                            logger.Log(logLevels.Low(), "{0}: Header {1} can't be set.", context.OperationKey, matchingHeader);
                        else
                            headersSetCount++;
                    }
                }

                if (headersSetCount > 0)
                {
                    logger.Log(logLevels.Low(),
                        headersSetCount < options.Headers.Count
                            ? "{0}: Some provided header values have been set."
                            : "{0}: All provided header values have been set.", context.OperationKey);

                    var shouldRedactHeaderValue = options.ShouldRedactHeaderValue ?? ShouldRedactHeaderValue;

                    var headersLogValue = new ApizrHttpHeadersLogValue(ApizrHttpHeadersLogValue.Kind.Request,
                        request.Headers, request.Content?.Headers, shouldRedactHeaderValue);

                    logger.Log(logLevels.Low(), headersLogValue.ToString());
                }

                // Handling cancellation
                if (!options.HandlersParameters.TryGetValue(Constants.ApizrOptionsProcessedKey,
                        out var optionsProcessedValue) || optionsProcessedValue is false)
                {
                    // Set options as processed yet
                    options.HandlersParameters[Constants.ApizrOptionsProcessedKey] = true;

                    // Set optionCancellationToken out param
                    optionsCancellationToken = options.CancellationToken;

                    // Merge cancellation tokens
                    cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, optionsCancellationToken);
                }
                else
                {
                    optionsCancellationToken = cancellationToken;
                }
            }
            else
            {
                optionsCancellationToken = cancellationToken;
            }

            return cts;
        }

        internal static bool TrySetHeader(this HttpClient httpClient, string header, out string key, out string value)
        {
            if (TryGetHeaderKeyValue(header, out key, out value))
                return httpClient.DefaultRequestHeaders.TrySetHeader(key, value);

            return false;
        }

        internal static bool TrySetHeader(this HttpRequestMessage request, string header, out string key, out string value)
        {
            if (TryGetHeaderKeyValue(header, out key, out value))
            {
                var added = request.Headers.TrySetHeader(key, value);

                if (request.Content != null) 
                    added = request.Content.Headers.TrySetHeader(key, value, added);

                return added;
            }

            return false;
        }

        /// <summary>
        /// Cloned from Refit repository
        /// </summary>
        internal static bool TrySetHeader(this HttpHeaders headers, string key, string value, bool removeOnly = false)
        {
            // Clear any existing version of this header that might be set, because
            // we want to allow removal/redefinition of headers.
            // We also don't want to double up content headers which may have been
            // set for us automatically.

            // NB: We have to enumerate the header names to check existence because
            // Contains throws if it's the wrong header type for the collection.
            if (headers.Any(x => x.Key == key))
                headers.Remove(key);

            // We don't even bother trying to add the header as a content header
            // if we just added it to the other collection, neither if its value is null
            if (value == null || removeOnly) 
                return true;

            return headers.TryAddWithoutValidation(key, value);
        }

        internal static bool TryGetHeaderKeyValue(string header, out string key, out string value)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                key = value = null;
            }
            else
            {
                var parts = header.Split(':');
                key = parts[0].Trim();
                value = parts.Length > 1 ?
                    string.Join(":", parts.Skip(1)).Trim() : null; 
            }

            return !string.IsNullOrWhiteSpace(key);
        }
    }
}
