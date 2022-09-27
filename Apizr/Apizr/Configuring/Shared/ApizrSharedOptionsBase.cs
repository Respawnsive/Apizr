using System;
using System.Linq;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level for both static and extended registrations
    /// </summary>
    public abstract class ApizrSharedOptionsBase : IApizrSharedOptionsBase
    {
        /// <inheritdoc />
        public Uri BaseUri { get; protected set; }

        /// <inheritdoc />
        public string BaseAddress { get; protected set; }

        /// <inheritdoc />
        public string BasePath { get; protected set; }

        /// <inheritdoc />
        public HttpTracerMode HttpTracerMode { get; protected set; }

        /// <inheritdoc />
        public HttpMessageParts TrafficVerbosity { get; protected set; }
        
        private LogLevel[] _logLevels;
        /// <inheritdoc />
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