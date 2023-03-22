using System;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic on Update method
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogUpdateAttribute : LogAttributeBase
    {
        /// <inheritdoc />
        public LogUpdateAttribute()
        {
        }

        /// <inheritdoc />
        public LogUpdateAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        /// <inheritdoc />
        public LogUpdateAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        /// <inheritdoc />
        public LogUpdateAttribute(params LogLevel[] logLevels) : base(logLevels)
        {
        }

        /// <inheritdoc />
        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        /// <inheritdoc />
        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels) : base(trafficVerbosity, logLevels)
        {
        }

        /// <inheritdoc />
        public LogUpdateAttribute(HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(httpTracerMode, logLevels)
        {
        }

        /// <inheritdoc />
        public LogUpdateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(trafficVerbosity, httpTracerMode, logLevels)
        {
        }
    }
}
