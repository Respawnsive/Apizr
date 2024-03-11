using System.Collections.Generic;
using System;
using System.Linq;
using Apizr.Configuring.Shared.Context;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at every level for both static and extended registrations
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
            OperationTimeout = sharedOptions?.OperationTimeout;
            RequestTimeout = sharedOptions?.RequestTimeout;
            Headers = sharedOptions?.Headers ?? new List<string>();
            ShouldRedactHeaderValue = sharedOptions?.ShouldRedactHeaderValue;
            _contextOptionsBuilder = sharedOptions?.ContextOptionsBuilder;
            _resiliencePropertiesFactories = sharedOptions?.ResiliencePropertiesFactories?.ToDictionary(kpv => kpv.Key, kpv => kpv.Value) ?? 
                                    new Dictionary<string, Func<object>>();
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
        public TimeSpan? OperationTimeout { get; internal set; }

        /// <inheritdoc />
        public TimeSpan? RequestTimeout { get; internal set; }

        /// <inheritdoc />
        public Func<string, bool> ShouldRedactHeaderValue { get; internal set; }

        private Action<IApizrResilienceContextOptionsBuilder> _contextOptionsBuilder;
        /// <inheritdoc />
        Action<IApizrResilienceContextOptionsBuilder> IApizrGlobalSharedOptionsBase.ContextOptionsBuilder
        {
            get => _contextOptionsBuilder;
            set => _contextOptionsBuilder = value;
        }

        private readonly IDictionary<string, Func<object>> _resiliencePropertiesFactories;
        /// <inheritdoc />
        IDictionary<string, Func<object>> IApizrGlobalSharedOptionsBase.ResiliencePropertiesFactories => _resiliencePropertiesFactories;
    }
}