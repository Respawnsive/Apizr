using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Policing
{
    /// <summary>
    /// Policy logging helper
    /// </summary>
    public static class LoggedPolicies
    {
        /// <summary>
        /// Tells Apizr to log retries
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeSpan"></param>
        /// <param name="retryCount"></param>
        /// <param name="context"></param>
        public static void OnLoggedRetry(DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount,
            Context context)
            => OnLoggedRetry(result, timeSpan, retryCount, context, null);

        /// <summary>
        /// Tells Apizr to log retries plus some more actions
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeSpan"></param>
        /// <param name="retryCount"></param>
        /// <param name="context"></param>
        /// <param name="onRetry"></param>
        public static void OnLoggedRetry(DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount, Context context, Action<DelegateResult<HttpResponseMessage>, TimeSpan, int, Context> onRetry)
        {
            if (context.TryGetLogger(out var logger))
            {
                if (result.Exception != null)
                {
                    logger.LogError($"An exception occurred on retry {retryCount} for {context.PolicyKey}: {result.Exception}");
                }
                else
                {
                    logger.LogError(
                        $"A non success code {(int) result.Result.StatusCode} was received on retry {retryCount} for {context.PolicyKey}");
                }
            }

            onRetry?.Invoke(result, timeSpan, retryCount, context);
        }
    }
}
