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
        /// Trace traffic with verbosity
        /// </summary>
        /// <param name="verbosity">Tracing verbosity (default: all)</param>
        public TraceAttribute(HttpMessageParts verbosity = HttpMessageParts.All)
        {
            Verbosity = verbosity;
        }

        /// <summary>
        /// Traffic tracing verbosity
        /// </summary>
        public HttpMessageParts Verbosity { get; }
    }
}
