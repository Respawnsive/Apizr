using System;
using HttpTracer;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class)]
    public class LogAttribute : Attribute
    {
        /// <summary>
        /// Trace http traffic and log Apizr execution steps with verbosity
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: all)</param>
        /// <param name="trafficLogLevel">Log level to apply while writing http traces (default: Trace)</param>
        public LogAttribute(HttpMessageParts trafficVerbosity = HttpMessageParts.All, LogLevel trafficLogLevel = LogLevel.Information)
        {
            TrafficVerbosity = trafficVerbosity;
            TrafficLogLevel = trafficLogLevel;
        }

        /// <summary>
        /// Http traffic tracing verbosity
        /// </summary>
        public HttpMessageParts TrafficVerbosity { get; }

        public LogLevel TrafficLogLevel { get; }
    }
}
