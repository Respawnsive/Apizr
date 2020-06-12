using System;
using System.Net.Http;
using Polly;

namespace Apizr.Policing
{
    public static class LoggedPolicies
    {
        public static void OnLoggedRetry(DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount,
            Context context)
            => OnLoggedRetry(result, timeSpan, retryCount, context, null);

        public static void OnLoggedRetry(DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount, Context context, Action<DelegateResult<HttpResponseMessage>, TimeSpan, int, Context> onRetry)
        {
            if (context.TryGetLogHandler(out var logger))
            {
                if (result.Exception != null)
                {
                    logger.Write($"An exception occurred on retry {retryCount} for {context.PolicyKey}",
                        result.Exception.ToString());
                }
                else
                {
                    logger.Write(
                        $"A non success code {(int) result.Result.StatusCode} was received on retry {retryCount} for {context.PolicyKey}");
                }
            }

            onRetry?.Invoke(result, timeSpan, retryCount, context);
        }
    }
}
