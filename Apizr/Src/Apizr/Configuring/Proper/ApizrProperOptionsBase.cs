using System;
using System.Linq;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Shared;
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
        /// <param name="commonResiliencePipelineAttributes">Global resilience pipelines</param>
        /// <param name="properResiliencePipelineAttributes">Specific resilience pipelines</param>
        /// <param name="commonCacheAttribute">Global caching options</param>
        /// <param name="properCacheAttribute">Specific caching options</param>
        /// <param name="shouldRedactHeaderValue">Headers to redact value</param>
        protected ApizrProperOptionsBase(IApizrGlobalSharedRegistrationOptionsBase sharedOptions, 
            Type webApiType,
            ResiliencePipelineAttributeBase[] commonResiliencePipelineAttributes,
            ResiliencePipelineAttributeBase[] properResiliencePipelineAttributes,
            CacheAttribute commonCacheAttribute,
            CacheAttribute properCacheAttribute,
            Func<string, bool> shouldRedactHeaderValue = null) : base(sharedOptions)
        {
            WebApiType = webApiType;

            if(commonResiliencePipelineAttributes?.Length > 0)
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
        }

        /// <inheritdoc />
        public Type WebApiType { get; }

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }
    }
}
