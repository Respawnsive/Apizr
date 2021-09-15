using System;
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

        public LogUpdateAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        public LogUpdateAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }

        public LogUpdateAttribute(HttpTracerMode httpTracerMode, LogLevel logLevel) : base(httpTracerMode, logLevel)
        {
        }

        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, LogLevel logLevel) : base(trafficVerbosity, httpTracerMode, logLevel)
        {
        }
    }
}
