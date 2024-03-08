using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Resiliencing
{
    /// <summary>
    /// Polly logging extensions
    /// </summary>
    public static class ResilienceContextExtensions
    {
        /// <summary>
        /// Passing your <see cref="ILogger"/> mapping implementation to Polly context
        /// </summary>
        /// <param name="context">Polly context</param>
        /// <param name="logger">Your <see cref="ILogger"/> mapping implementation</param>
        /// <param name="logLevels"></param>
        /// <param name="verbosity"></param>
        /// <param name="tracerMode"></param>
        /// <returns></returns>
        public static ResilienceContext WithLogger(this ResilienceContext context, ILogger logger, LogLevel[] logLevels, HttpMessageParts verbosity, HttpTracerMode tracerMode)
        {
            context.Properties.Set(new ResiliencePropertyKey<ILogger>(nameof(ILogger)), logger);
            context.Properties.Set(new ResiliencePropertyKey<LogLevel[]>(nameof(LogLevel)), logLevels);
            context.Properties.Set(new ResiliencePropertyKey<HttpMessageParts>(nameof(HttpMessageParts)), verbosity);
            context.Properties.Set(new ResiliencePropertyKey<HttpTracerMode>(nameof(HttpTracerMode)), tracerMode);

            return context;
        }

        /// <summary>
        /// Trying to get your <see cref="ILogger"/> mapping implementation from Polly context
        /// </summary>
        /// <param name="context">Polly context</param>
        /// <param name="logger">Your <see cref="ILogger"/> mapping implementation</param>
        /// <param name="logLevels"></param>
        /// <param name="verbosity"></param>
        /// <param name="tracerMode"></param>
        /// <returns></returns>
        public static bool TryGetLogger(this ResilienceContext context, out ILogger logger, out LogLevel[] logLevels, out HttpMessageParts verbosity, out HttpTracerMode tracerMode)
        {
            if (context != null &&
                context.Properties.TryGetValue(new ResiliencePropertyKey<ILogger>(nameof(ILogger)), out var loggerValue) &&
                context.Properties.TryGetValue(new ResiliencePropertyKey<LogLevel[]>(nameof(LogLevel)), out var logLevelsValue) &&
                context.Properties.TryGetValue(new ResiliencePropertyKey<HttpMessageParts>(nameof(HttpMessageParts)), out var verbosityValue) &&
                context.Properties.TryGetValue(new ResiliencePropertyKey<HttpTracerMode>(nameof(HttpTracerMode)), out var tracerModeValue))
            {
                logger = loggerValue;
                logLevels = logLevelsValue;
                verbosity = verbosityValue;
                tracerMode = tracerModeValue;

                return true;
            }

            logger = null;
            logLevels = [LogLevel.None];
            verbosity = HttpMessageParts.None;
            tracerMode = HttpTracerMode.ExceptionsOnly;

            return false;
        }
    }
}
