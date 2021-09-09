using System;
using HttpTracer;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogCreateAttribute : LogAttributeBase
    {
        public LogCreateAttribute()
        {
        }

        public LogCreateAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        public LogCreateAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogCreateAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }
    }
}
