using Microsoft.Extensions.Logging;

namespace Apizr
{
    internal static class Constants
    {
        internal const string InterfaceTypeKey = "Refit.InterfaceType";
        internal const string PollyExecutionContextKey = "PollyExecutionContext";
        internal const string PriorityKey = "Priority";
        internal const LogLevel LowLogLevel = LogLevel.Trace;
        internal const LogLevel MediumLogLevel = LogLevel.Information;
        internal const LogLevel HighLogLevel = LogLevel.Critical;
    }
}
