using System;
using System.Net.Http;
using Apizr.Extending;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Resiliencing
{
    /// <summary>
    /// Policy logging helper
    /// </summary>
    public static class LoggedStrategies
    {
        /// <summary>
        /// Tells Apizr to log retries
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeSpan"></param>
        /// <param name="retryCount"></param>
        /// <param name="context"></param>
        public static void OnLoggedRetry(DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount,
            ResilienceContext context)
            => OnLoggedRetry(result, timeSpan, retryCount, context, null);

        /// <summary>
        /// Tells Apizr to log retries plus some more actions
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeSpan"></param>
        /// <param name="retryCount"></param>
        /// <param name="context"></param>
        /// <param name="onRetry"></param>
        public static void OnLoggedRetry(DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount, ResilienceContext context, Action<DelegateResult<HttpResponseMessage>, TimeSpan, int, ResilienceContext> onRetry)
        {
            if (context.TryGetLogger(out var logger, out var logLevels, out var verbosity, out var tracerMode))
            {
                if (result.Exception != null)
                {
                    logger.Log(logLevels.High(), $"An exception occurred on retry {retryCount} for {context.PolicyKey}: {result.Exception}");
                }
                else
                {
                    logger.Log(logLevels.High(), $"A non success code {(int) result.Result.StatusCode} was received on retry {retryCount} for {context.PolicyKey}");
                }
            }

            onRetry?.Invoke(result, timeSpan, retryCount, context);
        }
    }
}
