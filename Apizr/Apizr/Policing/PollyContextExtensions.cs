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
        /// <returns></returns>
        public static Context WithLogger(this Context context, ILogger logger)
        {
            context[nameof(ILogger)] = logger;
            return context;
        }

        /// <summary>
        /// Trying to get your <see cref="ILogger"/> mapping implementation from Polly context
        /// </summary>
        /// <param name="context">Polly context</param>
        /// <param name="logger">Your <see cref="ILogger"/> mapping implementation</param>
        /// <returns></returns>
        public static bool TryGetLogger(this Context context, out ILogger logger)
        {
            if (context.TryGetValue(nameof(ILogger), out var loggerObject) && loggerObject is ILogger theLogger)
            {
                logger = theLogger;
                return true;
            }

            logger = null;
            return false;
        }
    }
}
