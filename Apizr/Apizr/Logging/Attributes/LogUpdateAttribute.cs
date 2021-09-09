using System;
using HttpTracer;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogUpdateAttribute : LogAttributeBase
    {
        public LogUpdateAttribute()
        {
        }

        public LogUpdateAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        public LogUpdateAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }
    }
}
