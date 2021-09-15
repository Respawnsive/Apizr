using System;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogDeleteAttribute : LogAttributeBase
    {
        public LogDeleteAttribute()
        {
        }

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        public LogDeleteAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        public LogDeleteAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }

        public LogDeleteAttribute(HttpTracerMode httpTracerMode, LogLevel logLevel) : base(httpTracerMode, logLevel)
        {
        }

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, LogLevel logLevel) : base(trafficVerbosity, httpTracerMode, logLevel)
        {
        }
    }
}
