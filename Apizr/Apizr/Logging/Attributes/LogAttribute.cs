using System;
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

        public LogAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        public LogAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        public LogAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }

        public LogAttribute(HttpTracerMode httpTracerMode, LogLevel logLevel) : base(httpTracerMode, logLevel)
        {
        }

        public LogAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, LogLevel logLevel) : base(trafficVerbosity, httpTracerMode, logLevel)
        {
        }
    }
}
