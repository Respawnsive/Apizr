// Copied from https://github.com/BSiLabs/HttpTracer/blob/fdba9af621a005626bcad74de9651248e56b6872/src/HttpTracer/HttpTracerHandler.cs
// but reshaped with Microsoft.Extensions.Logging and some more features

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Policing;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    /// <summary>
    /// The Http tracer delegating handler
    /// </summary>
    public class ExtendedHttpTracerHandler : DelegatingHandler
    {
        private readonly IApizrManagerOptionsBase _apizrOptions;
        
        /// <summary>
        /// Duration string format. Defaults to "Duration: {0:ss\\:fffffff}"
        /// </summary>
        /// <remarks>
        /// <para>
        /// Receives a <typeparamref name="TimeSpan"/> at the [0] index.
        /// </para>
        /// <para>
        /// See <a href="https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings">https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings</a> for more details on TimeSpan formatting.
        /// </para>
        /// </remarks>
        public static string DefaultDurationFormat { get; set; } = "Duration: {0:ss\\:fffffff}";
        
        // ReSharper disable InconsistentNaming
        private const string MessageIndicator = " ==================== ";
        // ReSharper restore InconsistentNaming
        public static string LogMessageIndicatorPrefix = MessageIndicator;
        public static string LogMessageIndicatorSuffix = MessageIndicator;

        /// <summary> Constructs the <see cref="ExtendedHttpTracerHandler"/> with a custom <see cref="ILogger"/> and a custom <see cref="HttpMessageHandler"/></summary>
        /// <param name="apizrOptions">Apizr options</param>
        public ExtendedHttpTracerHandler(IApizrManagerOptionsBase apizrOptions) : this(null, apizrOptions) { }

        /// <summary> Constructs the <see cref="ExtendedHttpTracerHandler"/> with a custom <see cref="ILogger"/> and a custom <see cref="HttpMessageHandler"/></summary>
        /// <param name="handler">User defined <see cref="HttpMessageHandler"/></param>
        /// <param name="apizrOptions">Apizr options</param>
        public ExtendedHttpTracerHandler(HttpMessageHandler handler, IApizrManagerOptionsBase apizrOptions)
        {
            InnerHandler = handler ?? new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            _apizrOptions = apizrOptions;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IApizrRequestOptions requestOptions = null;
            var context = request.GetOrBuildApizrPolicyExecutionContext();
            if (!context.TryGetLogger(out var logger, out var logLevels, out var verbosity, out var tracerMode))
            {
                if (request.TryGetOptions(out requestOptions))
                {
                    logLevels = requestOptions.LogLevels;
                    verbosity = requestOptions.TrafficVerbosity;
                    tracerMode = requestOptions.HttpTracerMode;
                }
                else
                {
                    logLevels = _apizrOptions.LogLevels;
                    verbosity = _apizrOptions.TrafficVerbosity;
                    tracerMode = _apizrOptions.HttpTracerMode;
                }
                logger = _apizrOptions.Logger;
            }

            // Ignore some message parts if asked to
            if ((requestOptions != null || request.TryGetOptions(out requestOptions)) &&
                requestOptions.HandlersParameters.TryGetValue(Constants.ApizrIgnoreMessagePartsKey,
                    out var ignoreMessagePartsProperty) &&
                ignoreMessagePartsProperty is HttpMessageParts ignoreMessageParts)
            {
                foreach (var ignoreMessagePart in Enum.GetValues(ignoreMessageParts.GetType()).Cast<Enum>()
                             .Where(ignoreMessageParts.HasFlag).Cast<HttpMessageParts>())
                    verbosity &= ~ignoreMessagePart;
            }

            try
            {
                if(verbosity.HasRequestFlags() && tracerMode == HttpTracerMode.Everything)
                    await LogHttpRequest(request, logger, logLevels, verbosity).ConfigureAwait(false);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                stopwatch.Stop();

                if (tracerMode >= HttpTracerMode.ErrorsAndExceptionsOnly)
                {
                    if (!response.IsSuccessStatusCode && verbosity.HasRequestFlags())
                    {
                        if (tracerMode == HttpTracerMode.ErrorsAndExceptionsOnly)
                            await LogHttpRequest(request, logger, logLevels, verbosity).ConfigureAwait(false);

                        await LogHttpErrorRequest(request, logger, logLevels, verbosity).ConfigureAwait(false);
                    }

                    if(verbosity.HasResponseFlags())
                        await LogHttpResponse(response, stopwatch.Elapsed, logger, logLevels, verbosity).ConfigureAwait(false);  
                }

                return response;
            }
            catch (Exception ex)
            {
                if (verbosity.HasRequestFlags())
                {
                    if (tracerMode == HttpTracerMode.ExceptionsOnly)
                        await LogHttpRequest(request, logger, logLevels, verbosity).ConfigureAwait(false);

                    LogHttpException(request, ex, logger, logLevels); 
                }
                throw;
            }
        }

        private async Task LogHttpErrorRequest(HttpRequestMessage request, ILogger logger, LogLevel[] logLevels, HttpMessageParts verbosity)
        {
            var sb = new StringBuilder();
            var httpErrorRequestPrefix =
                $"{LogMessageIndicatorPrefix}HTTP ERROR REQUEST: [{request?.Method}]{LogMessageIndicatorSuffix}";
            sb.AppendLine(httpErrorRequestPrefix);

            var httpErrorRequestHeaders = GetRequestHeaders(request, verbosity);
            sb.AppendLine(httpErrorRequestHeaders);

            var httpErrorRequestBody = await GetRequestBody(request);
            sb.AppendLine(httpErrorRequestBody);

            if (sb.Length > 0)
                logger.Log(logLevels.High(), sb.ToString());
        }

        /// <summary>
        /// Logs Http request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="logger">The logger</param>
        /// <param name="logLevels">The log levels</param>
        /// <param name="verbosity">The verbosity</param>
        /// <returns></returns>
        protected virtual async Task LogHttpRequest(HttpRequestMessage request, ILogger logger, LogLevel[] logLevels, HttpMessageParts verbosity)
        {
            var sb = new StringBuilder();
            if (verbosity.HasFlag(HttpMessageParts.RequestHeaders) || verbosity.HasFlag(HttpMessageParts.RequestBody))
            {
                var httpRequestPrefix =
                    $"{LogMessageIndicatorPrefix}HTTP REQUEST: [{request?.Method}]{LogMessageIndicatorSuffix}";
                sb.AppendLine(httpRequestPrefix);

                var httpRequestMethodUri = $@"{request?.Method} {request?.RequestUri}";
                sb.AppendLine(httpRequestMethodUri);
            }

            if (verbosity.HasFlag(HttpMessageParts.RequestHeaders))
            {
                var httpErrorRequestHeaders = GetRequestHeaders(request, verbosity);
                sb.AppendLine(httpErrorRequestHeaders);
            }

            if (verbosity.HasFlag(HttpMessageParts.RequestBody))
            {
                var httpErrorRequestBody = await GetRequestBody(request);
                sb.AppendLine(httpErrorRequestBody);
            }

            if (sb.Length > 0)
                logger.Log(logLevels.Low(), sb.ToString());
        }

        /// <summary>
        /// Logs Http response
        /// </summary>
        /// <param name="response">The response</param>
        /// <param name="duration">The duration</param>
        /// <param name="logger">The logger</param>
        /// <param name="logLevels">The log levels</param>
        /// <param name="verbosity">The verbosity</param>
        /// <returns></returns>
        protected virtual async Task LogHttpResponse(HttpResponseMessage response, TimeSpan duration, ILogger logger, LogLevel[] logLevels, HttpMessageParts verbosity)
        {
            var sb = new StringBuilder();
            if (verbosity.HasFlag(HttpMessageParts.ResponseHeaders) || verbosity.HasFlag(HttpMessageParts.ResponseBody))
            {
                var responseResult = GetResponseLogHeading(response);

                var httpResponsePrefix =
                    $@"{LogMessageIndicatorPrefix}HTTP RESPONSE: [{responseResult}]{LogMessageIndicatorSuffix}";
                sb.AppendLine(httpResponsePrefix);

                var httpRequestMethodUri = $@"{response?.RequestMessage?.Method} {response?.RequestMessage?.RequestUri}";
                sb.AppendLine(httpRequestMethodUri);
            }

            if (verbosity.HasFlag(HttpMessageParts.ResponseHeaders))
            {
                var httpResponseHeaders = $@"{response}";
                sb.AppendLine(httpResponseHeaders);
            }

            if (verbosity.HasFlag(HttpMessageParts.ResponseBody))
            {
                var httpResponseContent = await GetResponseBody(response).ConfigureAwait(false);
                sb.AppendLine(httpResponseContent);
            }

            if (verbosity.HasFlag(HttpMessageParts.ResponseHeaders) || verbosity.HasFlag(HttpMessageParts.ResponseBody))
            {
                var httpResponsePostfix = string.Format(DefaultDurationFormat, duration);
                sb.AppendLine(httpResponsePostfix);
            }

            if (sb.Length > 0)
                logger.Log(logLevels.Low(), sb.ToString());
        }

        private string GetResponseLogHeading(HttpResponseMessage response)
        {
            const string succeeded = "SUCCEEDED";
            const string failed = "FAILED";

            string responseResult;
            if (response == null)
                responseResult = failed;
            else
                responseResult = response.IsSuccessStatusCode
                    ? $"{succeeded}: {(int)response.StatusCode} {response.StatusCode}"
                    : $"{failed}: {(int)response.StatusCode} {response.StatusCode}";
            return responseResult;
        }

        /// <summary>
        /// Logs Http exceptions
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="ex">The exception</param>
        /// <param name="logger">The logger</param>
        /// <param name="logLevels">The log levels</param>
        protected void LogHttpException(HttpRequestMessage request, Exception ex, ILogger logger, LogLevel[] logLevels)
        {
            var httpExceptionString = $@"{LogMessageIndicatorPrefix} HTTP EXCEPTION: [{request.Method}]{LogMessageIndicatorSuffix}
{request.Method} {request.RequestUri}
{ex}";
            logger.Log(logLevels.High(), httpExceptionString);
        }

        private string GetRequestHeaders(HttpRequestMessage request, HttpMessageParts verbosity)
        {
            string httpRequestHeaders = string.Empty;

            if (request is null)
                return httpRequestHeaders;

            if (verbosity.HasFlag(HttpMessageParts.RequestHeaders))
                httpRequestHeaders = $@"{request.Headers.ToString().TrimEnd().TrimEnd('}').TrimStart('{')}";

            if (verbosity.HasFlag(HttpMessageParts.RequestCookies)
                && InnerHandler is HttpClientHandler httpClientHandler)
            {
                if (!httpClientHandler.UseCookies) return httpRequestHeaders;
                var cookieHeader = httpClientHandler.CookieContainer?.GetCookieHeader(request.RequestUri);
                if (!string.IsNullOrWhiteSpace(cookieHeader))
                {
                    httpRequestHeaders += $"{Environment.NewLine}Cookie: {cookieHeader}";
                }
            }
            return httpRequestHeaders;
        }

        /// <summary>
        /// Get the request body
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns></returns>
        protected Task<string> GetRequestBody(HttpRequestMessage request) => request?.Content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty);

        /// <summary>
        /// Get the response body
        /// </summary>
        /// <param name="response">The response</param>
        /// <returns></returns>
        protected Task<string> GetResponseBody(HttpResponseMessage response) => response?.Content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty);
    }
}
