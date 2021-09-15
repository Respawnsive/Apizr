using System;
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

        public LogCreateAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        public LogCreateAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogCreateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        public LogCreateAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }

        public LogCreateAttribute(HttpTracerMode httpTracerMode, LogLevel logLevel) : base(httpTracerMode, logLevel)
        {
        }

        public LogCreateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, LogLevel logLevel) : base(trafficVerbosity, httpTracerMode, logLevel)
        {
        }
    }
}
