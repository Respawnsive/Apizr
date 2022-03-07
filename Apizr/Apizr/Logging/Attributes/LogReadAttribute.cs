using System;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogReadAttribute : LogAttributeBase
    {
        public LogReadAttribute()
        {
        }

        public LogReadAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        public LogReadAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        public LogReadAttribute(params LogLevel[] logLevels) : base(logLevels)
        {
        }

        public LogReadAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        public LogReadAttribute(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels) : base(trafficVerbosity, logLevels)
        {
        }

        public LogReadAttribute(HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(httpTracerMode, logLevels)
        {
        }

        public LogReadAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(trafficVerbosity, httpTracerMode, logLevels)
        {
        }
    }
}
