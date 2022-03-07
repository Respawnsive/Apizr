using System.Linq;
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
        
        private LogLevel[] _logLevels;
        public LogLevel[] LogLevels
        {
            get => _logLevels;
            protected set => _logLevels = value?.Any() == true
                ? value
                : new[]
                {
                    Constants.LowLogLevel, 
                    Constants.MediumLogLevel, 
                    Constants.HighLogLevel
                };
        }
    }
}