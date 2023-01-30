using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Policing
{
    /// <summary>
    /// Polly logging extensions
    /// </summary>
    public static class PollyContextExtensions
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
        public static Context WithLogger(this Context context, ILogger logger, LogLevel[] logLevels, HttpMessageParts verbosity, HttpTracerMode tracerMode)
        {
            context[nameof(ILogger)] = logger;
            context[nameof(LogLevel)] = logLevels;
            context[nameof(HttpMessageParts)] = verbosity;
            context[nameof(HttpTracerMode)] = tracerMode;
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
        public static bool TryGetLogger(this Context context, out ILogger logger, out LogLevel[] logLevels, out HttpMessageParts verbosity, out HttpTracerMode tracerMode)
        {
            if (context != null && 
                context.TryGetValue(nameof(ILogger), out var loggerObject) && loggerObject is ILogger loggerValue &&
                context.TryGetValue(nameof(LogLevel), out var logLevelsObject) && logLevelsObject is LogLevel[] logLevelsValue &&
                context.TryGetValue(nameof(HttpMessageParts), out var verbosityObject) && verbosityObject is HttpMessageParts verbosityValue &&
                context.TryGetValue(nameof(HttpTracerMode), out var tracerModeObject) && tracerModeObject is HttpTracerMode tracerModeValue)
            {
                logger = loggerValue;
                logLevels = logLevelsValue;
                verbosity = verbosityValue;
                tracerMode = tracerModeValue;
                return true;
            }

            logger = null;
            logLevels = new[] {LogLevel.None};
            verbosity = HttpMessageParts.None;
            tracerMode = HttpTracerMode.ExceptionsOnly;
            return false;
        }
    }
}
