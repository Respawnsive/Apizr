using System;
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

        public LogReadAllAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        public LogReadAllAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogReadAllAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        public LogReadAllAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }

        public LogReadAllAttribute(HttpTracerMode httpTracerMode, LogLevel logLevel) : base(httpTracerMode, logLevel)
        {
        }

        public LogReadAllAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, LogLevel logLevel) : base(trafficVerbosity, httpTracerMode, logLevel)
        {
        }
    }
}
