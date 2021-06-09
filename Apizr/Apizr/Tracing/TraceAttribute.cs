using System;
using HttpTracer;
using Microsoft.Extensions.Logging;

namespace Apizr.Tracing
{
    /// <summary>
    /// Tells Apizr to trace request/response HTTP(s) traffic for the decorated web api interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class)]
    public class TraceAttribute : Attribute
    {
        /// <summary>
        /// Trace http traffic and log Apizr execution steps with verbosity
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: all)</param>
        /// <param name="trafficLogLevel">Log level to apply while writing http traces (default: Trace)</param>
        public TraceAttribute(HttpMessageParts trafficVerbosity = HttpMessageParts.All, LogLevel trafficLogLevel = LogLevel.Trace)
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
