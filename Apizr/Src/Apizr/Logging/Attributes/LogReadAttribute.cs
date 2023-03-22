using System;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic on Read method
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogReadAttribute : LogAttributeBase
    {
        /// <inheritdoc />
        public LogReadAttribute()
        {
        }

        /// <inheritdoc />
        public LogReadAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        /// <inheritdoc />
        public LogReadAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        /// <inheritdoc />
        public LogReadAttribute(params LogLevel[] logLevels) : base(logLevels)
        {
        }

        /// <inheritdoc />
        public LogReadAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        /// <inheritdoc />
        public LogReadAttribute(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels) : base(trafficVerbosity, logLevels)
        {
        }

        /// <inheritdoc />
        public LogReadAttribute(HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(httpTracerMode, logLevels)
        {
        }

        /// <inheritdoc />
        public LogReadAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(trafficVerbosity, httpTracerMode, logLevels)
        {
        }
    }
}
