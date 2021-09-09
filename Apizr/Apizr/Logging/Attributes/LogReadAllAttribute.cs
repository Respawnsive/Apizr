using System;
using HttpTracer;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogReadAllAttribute : LogAttributeBase
    {
        public LogReadAllAttribute()
        {
        }

        public LogReadAllAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        public LogReadAllAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogReadAllAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }
    }
}
