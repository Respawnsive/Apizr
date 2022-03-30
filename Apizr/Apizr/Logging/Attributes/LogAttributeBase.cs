using System;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    public abstract class LogAttributeBase : Attribute
    {
        /// <summary>
        /// Trace All http traffic and log Apizr execution steps at default log levels ([Low] Trace, [Medium] Information and [High] Critical)
        /// </summary>
        protected LogAttributeBase()
        {

        }

        /// <summary>
        /// Trace http traffic at specified verbosity and log Apizr execution steps at default log levels ([Low] Trace, [Medium] Information and [High] Critical)
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: all)</param>
        protected LogAttributeBase(HttpMessageParts trafficVerbosity)
        {
            TrafficVerbosity = trafficVerbosity;
        }

        /// <summary>
        /// Trace All http traffic and log Apizr execution steps at default log levels ([Low] Trace, [Medium] Information and [High] Critical)
        /// </summary>
        /// <param name="httpTracerMode">Http traffic tracing mode (default: Everything)</param>
        protected LogAttributeBase(HttpTracerMode httpTracerMode)
        {
            HttpTracerMode = httpTracerMode;
        }

        /// <summary>
        /// Trace All http traffic and log Apizr execution steps at specified log levels
        /// </summary>
        /// <param name="logLevels">Log levels to apply while writing (default: [Low] Trace, [Medium] Information and [High] Critical)</param>
        protected LogAttributeBase(params LogLevel[] logLevels)
        {
            LogLevels = logLevels;
        }

        /// <summary>
        /// Trace http traffic and log Apizr execution steps at default log levels ([Low] Trace, [Medium] Information and [High] Critical)
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: all)</param>
        /// <param name="httpTracerMode">Http traffic tracing mode (default: Everything)</param>
        protected LogAttributeBase(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode)
        {
            TrafficVerbosity = trafficVerbosity;
            HttpTracerMode = httpTracerMode;
        }

        /// <summary>
        /// Trace All http traffic and log Apizr execution steps at specified log levels
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: all)</param>
        /// <param name="logLevels">Log levels to apply while writing (default: [Low] Trace, [Medium] Information and [High] Critical)</param>
        protected LogAttributeBase(HttpMessageParts trafficVerbosity, params LogLevel[] logLevels)
        {
            TrafficVerbosity = trafficVerbosity;
            LogLevels = logLevels;
        }

        /// <summary>
        /// Trace All http traffic and log Apizr execution steps at specified log levels
        /// </summary>
        /// <param name="httpTracerMode">Http traffic tracing mode (default: Everything)</param>
        /// <param name="logLevels">Log levels to apply while writing (default: [Low] Trace, [Medium] Information and [High] Critical)</param>
        protected LogAttributeBase(HttpTracerMode httpTracerMode, params LogLevel[] logLevels)
        {
            HttpTracerMode = httpTracerMode;
            LogLevels = logLevels;
        }

        /// <summary>
        /// Trace All http traffic and log Apizr execution steps at specified log levels
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: all)</param>
        /// <param name="httpTracerMode">Http traffic tracing mode (default: Everything)</param>
        /// <param name="logLevels">Log levels to apply while writing (default: [Low] Trace, [Medium] Information and [High] Critical)</param>
        protected LogAttributeBase(HttpMessageParts trafficVerbosity, HttpTracerMode httpTracerMode, params LogLevel[] logLevels)
        {
            TrafficVerbosity = trafficVerbosity;
            HttpTracerMode = httpTracerMode;
            LogLevels = logLevels;
        }

        /// <summary>
        /// Http traffic tracing mode (default: Everything)
        /// </summary>
        public HttpTracerMode HttpTracerMode { get; set; } = HttpTracerMode.Everything;

        /// <summary>
        /// Http traffic tracing verbosity (default: All)
        /// </summary>
        public HttpMessageParts TrafficVerbosity { get; } = HttpMessageParts.All;

        /// <summary>
        /// Log levels to apply while writing (default: [Low] Trace, [Medium] Information and [High] Critical)
        /// </summary>
        public LogLevel[] LogLevels { get; } = { Constants.LowLogLevel, Constants.MediumLogLevel, Constants.HighLogLevel };
    }
}
