using System;
using HttpTracer;

namespace Apizr.Logging
{
    /// <summary>
    /// Tells Apizr to trace request/response HTTP(s) traffic and log execution steps for the decorated web api interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class)]
    public class LogItAttribute : Attribute
    {
        /// <summary>
        /// Trace http traffic and log Apizr execution steps with verbosity
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: all)</param>
        public LogItAttribute(HttpMessageParts trafficVerbosity = HttpMessageParts.All)
        {
            TrafficVerbosity = trafficVerbosity;
        }

        /// <summary>
        /// Http traffic tracing verbosity
        /// </summary>
        public HttpMessageParts TrafficVerbosity { get; }
    }
}
