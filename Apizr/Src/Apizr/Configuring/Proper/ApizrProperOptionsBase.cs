using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Request;
using Apizr.Configuring.Shared;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Requesting;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Proper
{
    /// <inheritdoc cref="IApizrProperOptionsBase" />
    public abstract class ApizrProperOptionsBase : ApizrGlobalSharedRegistrationOptionsBase, IApizrProperOptionsBase
    {
        /// <summary>
        /// The proper options constructor
        /// </summary>
        /// <param name="sharedOptions">The shared options</param>
        /// <param name="webApiType">The web api type</param>
        /// <param name="crudApiEntityType">The crud api entity type if any</param>
        /// <param name="typeInfo">The type info</param>
        /// <param name="baseAddress">The web api base address</param>
        /// <param name="basePath">The web api base path</param>
        /// <param name="httpTracerMode">The http tracer mode</param>
        /// <param name="trafficVerbosity">The traffic verbosity</param>
        /// <param name="commonResiliencePipelineAttributes">Global resilience pipelines</param>
        /// <param name="properResiliencePipelineAttributes">Specific resilience pipelines</param>
        /// <param name="commonCacheAttribute">Global caching options</param>
        /// <param name="properCacheAttribute">Specific caching options</param>
        /// <param name="shouldRedactHeaderValue">Headers to redact value</param>
        /// <param name="logLevels">The log levels</param>
        protected ApizrProperOptionsBase(IApizrGlobalSharedRegistrationOptionsBase sharedOptions, 
            Type webApiType,
            Type crudApiEntityType,
            TypeInfo typeInfo,
            string baseAddress,
            string basePath,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            ResiliencePipelineAttributeBase[] commonResiliencePipelineAttributes,
            ResiliencePipelineAttributeBase[] properResiliencePipelineAttributes,
            CacheAttribute commonCacheAttribute,
            CacheAttribute properCacheAttribute,
            Func<string, bool> shouldRedactHeaderValue = null,
            params LogLevel[] logLevels) : base(sharedOptions)
        {
            WebApiType = webApiType;
            CrudApiEntityType = crudApiEntityType;
            TypeInfo = typeInfo;

            if(!string.IsNullOrWhiteSpace(baseAddress))
                BaseAddress = baseAddress;

            if (!string.IsNullOrWhiteSpace(basePath)) 
                BasePath = basePath;

            if (commonResiliencePipelineAttributes?.Length > 0)
                ResiliencePipelineOptions[ApizrConfigurationSource.CommonAttribute] = commonResiliencePipelineAttributes;

            if(properResiliencePipelineAttributes?.Length > 0)
                ResiliencePipelineOptions[ApizrConfigurationSource.ProperAttribute] = properResiliencePipelineAttributes;

            if(commonCacheAttribute != null)
                CacheOptions[ApizrConfigurationSource.CommonAttribute] = commonCacheAttribute;

            if (properCacheAttribute != null)
                CacheOptions[ApizrConfigurationSource.ProperAttribute] = properCacheAttribute;

            if (ShouldRedactHeaderValue == null)
            {
                ShouldRedactHeaderValue = shouldRedactHeaderValue;
            }
            else if (shouldRedactHeaderValue != null)
            {
                var previous = ShouldRedactHeaderValue;
                ShouldRedactHeaderValue = header => previous(header) || shouldRedactHeaderValue(header);
            }

            RequestOptionsBuilders = new Dictionary<string, Action<IApizrRequestOptionsBuilder>>();
            RequestNames = typeInfo.GetMethods().Select(method => method.Name).ToList();
            
            if(httpTracerMode.HasValue)
                HttpTracerMode = httpTracerMode.Value;

            if (trafficVerbosity.HasValue)
                TrafficVerbosity = trafficVerbosity.Value;

            LogLevels = logLevels;
        }

        /// <inheritdoc />
        public Type WebApiType { get; }

        /// <inheritdoc />
        public Type CrudApiEntityType { get; }

        /// <inheritdoc />
        public TypeInfo TypeInfo { get; }

        /// <inheritdoc />
        public IReadOnlyList<string> RequestNames { get; }

        /// <inheritdoc />
        public bool IsCrudApi => CrudApiEntityType != null;

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }

        /// <inheritdoc />
        public IDictionary<string, Action<IApizrRequestOptionsBuilder>> RequestOptionsBuilders { get; }
    }
}
