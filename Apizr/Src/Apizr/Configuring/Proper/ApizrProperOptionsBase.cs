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
        /// <param name="assemblyResiliencePipelineRegistryKeys">Global resilience pipelines</param>
        /// <param name="webApiResiliencePipelineRegistryKeys">Specific resilience pipelines</param>
        /// <param name="shouldRedactHeaderValue">Headers to redact value</param>
        protected ApizrProperOptionsBase(IApizrGlobalSharedRegistrationOptionsBase sharedOptions, 
            Type webApiType,
            string[] assemblyResiliencePipelineRegistryKeys,
            string[] webApiResiliencePipelineRegistryKeys,
            Func<string, bool> shouldRedactHeaderValue = null) : base(sharedOptions)
        {
            WebApiType = webApiType;
            ResiliencePipelineRegistryKeys =
                assemblyResiliencePipelineRegistryKeys?.Union(webApiResiliencePipelineRegistryKeys ?? []).ToArray() ??
                webApiResiliencePipelineRegistryKeys ?? [];

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
        public string[] ResiliencePipelineRegistryKeys { get; }

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }
    }
}
