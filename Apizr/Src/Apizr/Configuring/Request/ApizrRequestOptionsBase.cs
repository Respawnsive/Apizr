using System;
using System.Collections.Generic;
using System.Linq;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Apizr.Resiliencing.Attributes;
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
            ResiliencePipelineAttributeBase requestResiliencePipelineAttribute,
            CacheAttributeBase requestCacheAttribute,
            ApizrRequestMethod requestMethod,
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
            if(requestResiliencePipelineAttribute?.RegistryKeys.Length > 0)
                ResiliencePipelineOptions[ApizrConfigurationSource.RequestAttribute] = [requestResiliencePipelineAttribute];
            if(requestCacheAttribute != null)
                CacheOptions[ApizrConfigurationSource.RequestAttribute] = requestCacheAttribute;
            if(requestMethod != null)
                RequestMethod = requestMethod;
        }

        /// <inheritdoc />
        public ResilienceContext ResilienceContext { get; internal set; }

        /// <inheritdoc />
        public ApizrRequestMethod RequestMethod { get; }
    }
}
