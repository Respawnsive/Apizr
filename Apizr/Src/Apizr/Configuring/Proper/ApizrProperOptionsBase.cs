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
        protected ApizrProperOptionsBase(IApizrGlobalSharedRegistrationOptionsBase sharedOptions, 
            Type webApiType,
            string[] assemblyResiliencePipelineRegistryKeys,
            string[] webApiResiliencePipelineRegistryKeys) : base(sharedOptions)
        {
            WebApiType = webApiType;
            ResiliencePipelineRegistryKeys =
                assemblyResiliencePipelineRegistryKeys?.Union(webApiResiliencePipelineRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiResiliencePipelineRegistryKeys ?? Array.Empty<string>();
        }

        /// <inheritdoc />
        public Type WebApiType { get; }

        /// <inheritdoc />
        public string[] ResiliencePipelineRegistryKeys { get; }

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }
    }
}
