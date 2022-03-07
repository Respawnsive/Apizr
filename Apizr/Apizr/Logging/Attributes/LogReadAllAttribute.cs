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

        public LogReadAllAttribute(params LogLevel[] logLevels) : base(logLevels)
        {
        }

        public LogReadAllAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        public LogReadAllAttribute(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels) : base(trafficVerbosity, logLevels)
        {
        }

        public LogReadAllAttribute(HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(httpTracerMode, logLevels)
        {
        }

        public LogReadAllAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(trafficVerbosity, httpTracerMode, logLevels)
        {
        }
    }
}
