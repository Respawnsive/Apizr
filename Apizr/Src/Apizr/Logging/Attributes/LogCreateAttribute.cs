using System;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic on Create method
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogCreateAttribute : LogAttributeBase
    {
        /// <inheritdoc />
        public LogCreateAttribute()
        {
        }

        /// <inheritdoc />
        public LogCreateAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        /// <inheritdoc />
        public LogCreateAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        /// <inheritdoc />
        public LogCreateAttribute(params LogLevel[] logLevels) : base(logLevels)
        {
        }

        /// <inheritdoc />
        public LogCreateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }

        /// <inheritdoc />
        public LogCreateAttribute(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels) : base(trafficVerbosity, logLevels)
        {
        }

        /// <inheritdoc />
        public LogCreateAttribute(HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(httpTracerMode, logLevels)
        {
        }

        /// <inheritdoc />
        public LogCreateAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(trafficVerbosity, httpTracerMode, logLevels)
        {
        }
    }
}
