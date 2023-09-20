using System.Collections.Generic;
using System;
using System.Linq;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level for both static and extended registrations
    /// </summary>
    public abstract class ApizrGlobalSharedOptionsBase : IApizrGlobalSharedOptionsBase
    {
        protected ApizrGlobalSharedOptionsBase(IApizrGlobalSharedOptionsBase sharedOptions = null)
        {
            HttpTracerMode = sharedOptions?.HttpTracerMode ?? default;
            TrafficVerbosity = sharedOptions?.TrafficVerbosity ?? default;
            LogLevels = sharedOptions?.LogLevels.ToArray();
            OnException = sharedOptions?.OnException;
            LetThrowOnExceptionWithEmptyCache = sharedOptions?.LetThrowOnExceptionWithEmptyCache ?? true;
            HandlersParameters = sharedOptions?.HandlersParameters?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                                 new Dictionary<string, object>();
            Headers = sharedOptions?.Headers ?? new List<string>();
        }

        /// <inheritdoc />
        public HttpTracerMode HttpTracerMode { get; internal set; }

        /// <inheritdoc />
        public HttpMessageParts TrafficVerbosity { get; internal set; }
        
        private LogLevel[] _logLevels;
        /// <inheritdoc />
        public LogLevel[] LogLevels
        {
            get => _logLevels;
            internal set => _logLevels = value?.Any() == true
                ? value
                : new[]
                {
                    Constants.LowLogLevel, 
                    Constants.MediumLogLevel, 
                    Constants.HighLogLevel
                };
        }

        /// <inheritdoc />
        public Action<ApizrException> OnException { get; internal set; }

        /// <inheritdoc />
        public bool LetThrowOnExceptionWithEmptyCache { get; internal set; }

        /// <inheritdoc />
        public IDictionary<string, object> HandlersParameters { get; protected set; }

        /// <inheritdoc />
        public IList<string> Headers { get; internal set; }

        /// <inheritdoc />
        public TimeSpan? Timeout { get; internal set; }
    }
}