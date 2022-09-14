using System;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic on Delete method
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogDeleteAttribute : LogAttributeBase
    {
        /// <inheritdoc />
        public LogDeleteAttribute()
        {
        }

        /// <inheritdoc />
        public LogDeleteAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        /// <inheritdoc />
        public LogDeleteAttribute(HttpTracerMode httpTracerMode) : base(httpTracerMode)
        {
        }

        /// <inheritdoc />
        public LogDeleteAttribute(params LogLevel[] logLevels) : base(logLevels)
        {
        }

        /// <inheritdoc />
        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode) : base(trafficVerbosity, httpTracerMode)
        {
        }
        /// <inheritdoc />

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels) : base(trafficVerbosity, logLevels)
        {
        }

        /// <inheritdoc />
        public LogDeleteAttribute(HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(httpTracerMode, logLevels)
        {
        }

        /// <inheritdoc />
        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels) : base(trafficVerbosity, httpTracerMode, logLevels)
        {
        }
    }
}
