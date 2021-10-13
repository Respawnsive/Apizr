using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    public abstract class ApizrSharedOptionsBase : IApizrSharedOptionsBase
    {
        protected ApizrSharedOptionsBase()
        {
            
        }

        public HttpTracerMode HttpTracerMode { get; protected set; }
        public HttpMessageParts TrafficVerbosity { get; protected set; }
        public LogLevel LogLevel { get; protected set; }
    }
}