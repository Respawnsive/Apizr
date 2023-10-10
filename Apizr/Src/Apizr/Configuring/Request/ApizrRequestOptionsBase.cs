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
            TimeSpan? operationTimeout,
            TimeSpan? requestTimeout,
            params LogLevel[] logLevels) : base(sharedOptions)
        {
            var context = sharedOptions?.ContextFactory?.Invoke();
            if (context?.Count > 0)
                Context = context;
            if (httpTracerMode != null)
                HttpTracerMode = httpTracerMode.Value;
            if (trafficVerbosity != null)
                TrafficVerbosity = trafficVerbosity.Value;
            if(logLevels?.Any() == true)
                LogLevels = logLevels;
            if(operationTimeout.HasValue)
                OperationTimeout = operationTimeout.Value;
            if (requestTimeout.HasValue)
                RequestTimeout = requestTimeout.Value;
        }

        /// <inheritdoc />
        public Context Context { get; internal set; }
    }
}
