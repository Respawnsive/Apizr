using System;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogAttribute : LogAttributeBase
    {
        /// <inheritdoc />
        public LogAttribute()
        {
        }

        /// <inheritdoc />
        public LogAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        /// <inheritdoc />
        public LogAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        /// <inheritdoc />
        public LogAttribute(params LogLevel[] logLevels) : base(logLevels)
        {
        }

        /// <inheritdoc />
        public LogAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        /// <inheritdoc />
        public LogAttribute(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels) : base(trafficVerbosity, logLevels)
        {
        }

        /// <inheritdoc />
        public LogAttribute(HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(httpTracerMode, logLevels)
        {
        }

        /// <inheritdoc />
        public LogAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(trafficVerbosity, httpTracerMode, logLevels)
        {
        }
    }
}
