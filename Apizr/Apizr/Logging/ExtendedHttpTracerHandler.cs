// Copied from https://github.com/BSiLabs/HttpTracer/blob/fdba9af621a005626bcad74de9651248e56b6872/src/HttpTracer/HttpTracerHandler.cs
// but reshaped with Microsoft.Extensions.Logging and some more features

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Policing;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    public class ExtendedHttpTracerHandler : DelegatingHandler
    {
        private readonly IApizrOptionsBase _apizrOptions;
        
        /// <summary>
        /// Duration string format. Defaults to "Duration: {0:ss\\:fffffff}"
        /// </summary>
        /// <remarks>
        /// <para>
        /// Receives a <see cref="TimeSpan"/> at the [0] index.
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
        /// <param name="logger">Microsoft extended logger</param>
        /// <param name="apizrOptions">Apizr options</param>
        public ExtendedHttpTracerHandler(IApizrOptionsBase apizrOptions) : this(null, apizrOptions) { }

        /// <summary> Constructs the <see cref="ExtendedHttpTracerHandler"/> with a custom <see cref="ILogger"/> and a custom <see cref="HttpMessageHandler"/></summary>
        /// <param name="handler">User defined <see cref="HttpMessageHandler"/></param>
        /// <param name="logger">Microsoft extended logger</param>
        /// <param name="apizrOptions">Apizr options</param>
        public ExtendedHttpTracerHandler(HttpMessageHandler handler, IApizrOptionsBase apizrOptions)
        {
            InnerHandler = handler ?? new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            _apizrOptions = apizrOptions;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var context = request.GetOrBuildPolicyExecutionContext();
            if (!context.TryGetLogger(out var logger, out var logLevel, out var verbosity, out var tracerMode))
            {
                logger = _apizrOptions.Logger;
                logLevel = _apizrOptions.LogLevel;
                verbosity = _apizrOptions.TrafficVerbosity;
                tracerMode = _apizrOptions.HttpTracerMode;
            }

            try
            {
                if(tracerMode == HttpTracerMode.Everything)
                    await LogHttpRequest(request, logger, logLevel, verbosity).ConfigureAwait(false);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                stopwatch.Stop();

                if (tracerMode >= HttpTracerMode.ErrorsAndExceptionsOnly)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (tracerMode == HttpTracerMode.ErrorsAndExceptionsOnly)
                            await LogHttpRequest(request, logger, logLevel, verbosity).ConfigureAwait(false);

                        await LogHttpErrorRequest(request, logger, logLevel, verbosity);
                    }

                    await LogHttpResponse(response, stopwatch.Elapsed, logger, logLevel, verbosity).ConfigureAwait(false);  
                }

                return response;
            }
            catch (Exception ex)
            {
                if (tracerMode == HttpTracerMode.ExceptionsOnly)
                    await LogHttpRequest(request, logger, logLevel, verbosity).ConfigureAwait(false);

                LogHttpException(request, ex, logger, logLevel);
                throw;
            }
        }

        private async Task LogHttpErrorRequest(HttpRequestMessage request, ILogger logger, LogLevel logLevel, HttpMessageParts verbosity)
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
                logger.Log(logLevel, sb.ToString());
        }

        protected virtual async Task LogHttpRequest(HttpRequestMessage request, ILogger logger, LogLevel logLevel, HttpMessageParts verbosity)
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
                logger.Log(logLevel, sb.ToString());
        }

        protected virtual async Task LogHttpResponse(HttpResponseMessage response, TimeSpan duration, ILogger logger, LogLevel logLevel, HttpMessageParts verbosity)
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
                logger.Log(logLevel, sb.ToString());
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

        protected void LogHttpException(HttpRequestMessage request, Exception ex, ILogger logger, LogLevel logLevel)
        {
            var httpExceptionString = $@"{LogMessageIndicatorPrefix} HTTP EXCEPTION: [{request.Method}]{LogMessageIndicatorSuffix}
{request.Method} {request.RequestUri}
{ex}";
            logger.Log(logLevel, httpExceptionString);
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
                var cookieHeader = httpClientHandler.CookieContainer.GetCookieHeader(request.RequestUri);
                if (!string.IsNullOrWhiteSpace(cookieHeader))
                {
                    httpRequestHeaders += $"{Environment.NewLine}Cookie: {cookieHeader}";
                }
            }
            return httpRequestHeaders;
        }

        protected Task<string> GetRequestBody(HttpRequestMessage request) => request?.Content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty);

        protected Task<string> GetResponseBody(HttpResponseMessage response) => response?.Content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty);
    }
}
