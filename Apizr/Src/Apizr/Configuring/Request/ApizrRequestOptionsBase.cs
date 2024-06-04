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
            string[] requestResiliencePipelineKeys,
            params LogLevel[] logLevels) : base(sharedOptions)
        {
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
            if(requestResiliencePipelineKeys?.Length > 0)
                ResiliencePipelineKeys[ApizrConfigurationSource.RequestAttributes] = requestResiliencePipelineKeys;
        }

        /// <inheritdoc />
        public ResilienceContext ResilienceContext { get; internal set; }
    }
}
