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

        public LogDeleteAttribute(params LogLevel[] logLevels) : base(logLevels)
        {
        }

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels) : base(trafficVerbosity, logLevels)
        {
        }

        public LogDeleteAttribute(HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(httpTracerMode, logLevels)
        {
        }

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(trafficVerbosity, httpTracerMode, logLevels)
        {
        }
    }
}
