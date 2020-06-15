using Apizr.Logging;
using Polly;

namespace Apizr.Policing
{
    /// <summary>
    /// Polly logging extensions
    /// </summary>
    public static class PollyContextExtensions
    {
        /// <summary>
        /// Passing your <see cref="ILogHandler"/> mapping implementation to Polly context
        /// </summary>
        /// <param name="context">Polly context</param>
        /// <param name="logHandler">Your <see cref="ILogHandler"/> mapping implementation</param>
        /// <returns></returns>
        public static Context WithLogHandler(this Context context, ILogHandler logHandler)
        {
            context[nameof(ILogHandler)] = logHandler;
            return context;
        }

        /// <summary>
        /// Trying to get your <see cref="ILogHandler"/> mapping implementation from Polly context
        /// </summary>
        /// <param name="context">Polly context</param>
        /// <param name="logHandler">Your <see cref="ILogHandler"/> mapping implementation</param>
        /// <returns></returns>
        public static bool TryGetLogHandler(this Context context, out ILogHandler logHandler)
        {
            if (context.TryGetValue(nameof(ILogHandler), out var logHandlerObject) && logHandlerObject is ILogHandler theLogHandler)
            {
                logHandler = theLogHandler;
                return true;
            }

            logHandler = null;
            return false;
        }
    }
}
