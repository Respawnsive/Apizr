using System;
using HttpTracer;

namespace Apizr.Logging
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class TraceAttribute : Attribute
    {
        public TraceAttribute(HttpMessageParts verbosity = HttpMessageParts.All)
        {
            this.Verbosity = verbosity;
        }

        public HttpMessageParts Verbosity { get; }
    }
}
