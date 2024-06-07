using System;
using System.Linq;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Shared;
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
        /// <param name="commonResiliencePipelineKeys">Global resilience pipelines</param>
        /// <param name="properResiliencePipelineKeys">Specific resilience pipelines</param>
        /// <param name="commonCacheAttribute">Global caching options</param>
        /// <param name="properCacheAttribute">Specific caching options</param>
        /// <param name="shouldRedactHeaderValue">Headers to redact value</param>
        protected ApizrProperOptionsBase(IApizrGlobalSharedRegistrationOptionsBase sharedOptions, 
            Type webApiType,
            string[] commonResiliencePipelineKeys,
            string[] properResiliencePipelineKeys,
            CacheAttribute commonCacheAttribute,
            CacheAttribute properCacheAttribute,
            Func<string, bool> shouldRedactHeaderValue = null) : base(sharedOptions)
        {
            WebApiType = webApiType;

            if(commonResiliencePipelineKeys?.Length > 0)
                ResiliencePipelineKeys[ApizrConfigurationSource.CommonAttribute] = commonResiliencePipelineKeys;

            if (properResiliencePipelineKeys?.Length > 0)
                ResiliencePipelineKeys[ApizrConfigurationSource.ProperAttribute] = properResiliencePipelineKeys;

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
