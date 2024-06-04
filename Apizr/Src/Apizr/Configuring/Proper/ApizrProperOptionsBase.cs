using System;
using System.Linq;
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
        /// <param name="assemblyResiliencePipelineKeys">Global resilience pipelines</param>
        /// <param name="webApiResiliencePipelineKeys">Specific resilience pipelines</param>
        /// <param name="shouldRedactHeaderValue">Headers to redact value</param>
        protected ApizrProperOptionsBase(IApizrGlobalSharedRegistrationOptionsBase sharedOptions, 
            Type webApiType,
            string[] assemblyResiliencePipelineKeys,
            string[] webApiResiliencePipelineKeys,
            Func<string, bool> shouldRedactHeaderValue = null) : base(sharedOptions)
        {
            WebApiType = webApiType;

            if(assemblyResiliencePipelineKeys?.Length > 0)
                ResiliencePipelineKeys[ApizrConfigurationSource.CommonAttributes] = assemblyResiliencePipelineKeys;

            if (webApiResiliencePipelineKeys?.Length > 0)
                ResiliencePipelineKeys[ApizrConfigurationSource.ProperAttributes] = webApiResiliencePipelineKeys;

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
