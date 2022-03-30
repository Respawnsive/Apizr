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

        public LogUpdateAttribute(params LogLevel[] logLevels) : base(logLevels)
        {
        }

        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels) : base(trafficVerbosity, logLevels)
        {
        }

        public LogUpdateAttribute(HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(httpTracerMode, logLevels)
        {
        }

        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(trafficVerbosity, httpTracerMode, logLevels)
        {
        }
    }
}
