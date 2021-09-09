using System;
using HttpTracer;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogAttribute : LogAttributeBase
    {
        public LogAttribute()
        {
        }

        public LogAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        public LogAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }
    }
}
