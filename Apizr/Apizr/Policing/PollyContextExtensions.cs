using Apizr.Logging;
using Polly;

namespace Apizr.Policing
{
    public static class PollyContextExtensions
    {
        public static Context WithLogHandler(this Context context, ILogHandler logHandler)
        {
            context[nameof(ILogHandler)] = logHandler;
            return context;
        }

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
