using Microsoft.Extensions.Logging;

namespace Apizr
{
    public class Constants
    {
        public const string InterfaceTypeKey = "Refit.InterfaceType";
        public const string PollyExecutionContextKey = "PollyExecutionContext";
        public const string PriorityKey = "Priority";
        public const LogLevel LowLogLevel = LogLevel.Trace;
        public const LogLevel MediumLogLevel = LogLevel.Information;
        public const LogLevel HighLogLevel = LogLevel.Critical;
    }
}
