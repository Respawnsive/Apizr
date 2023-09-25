using System;
using System.Collections.Generic;
using System.Linq;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request
{
    /// <inheritdoc cref="IApizrRequestOptionsBase" />
    public abstract class ApizrRequestOptionsBase : ApizrGlobalSharedOptionsBase, IApizrRequestOptionsBase
    {
        /// <inheritdoc />
        protected ApizrRequestOptionsBase(IApizrGlobalSharedRegistrationOptionsBase sharedOptions,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            TimeSpan? timeout,
            params LogLevel[] logLevels) : base(sharedOptions)
        {
            Context = sharedOptions?.ContextFactory?.Invoke();
            if (httpTracerMode != null)
                HttpTracerMode = httpTracerMode.Value;
            if (trafficVerbosity != null)
                TrafficVerbosity = trafficVerbosity.Value;
            if(logLevels?.Any() == true)
                LogLevels = logLevels;
            if(timeout.HasValue)
                Timeout = timeout.Value; // The request one, not the HttpClient one

            Headers = new List<string>(); // The request ones, not the HttpClient ones
        }

        /// <inheritdoc />
        public Context Context { get; internal set; }
    }
}
