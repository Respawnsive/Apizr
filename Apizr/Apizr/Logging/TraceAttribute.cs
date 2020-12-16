using System;
using HttpTracer;

namespace Apizr.Logging
{
    /// <summary>
    /// Tells Apizr to trace request/response HTTP(s) traffic for the decorated web api interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class TraceAttribute : Attribute
    {
        /// <summary>
        /// Trace http traffic and Apizr executions with verbosity
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: all)</param>
        /// <param name="apizrVerbosity">Apizr execution steps verbosity (default: high)</param>
        public TraceAttribute(HttpMessageParts trafficVerbosity = HttpMessageParts.All, ApizrLogLevel apizrVerbosity = ApizrLogLevel.High)
        {
            TrafficVerbosity = trafficVerbosity;
            ApizrVerbosity = apizrVerbosity;
        }

        /// <summary>
        /// Http traffic tracing verbosity
        /// </summary>
        public HttpMessageParts TrafficVerbosity { get; }

        /// <summary>
        /// Apizr executions tracing verbosity
        /// </summary>
        public ApizrLogLevel ApizrVerbosity { get; }
    }
}
