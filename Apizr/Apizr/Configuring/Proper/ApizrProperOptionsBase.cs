using System;
using System.Linq;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Proper
{
    /// <inheritdoc cref="IApizrProperOptionsBase" />
    public abstract class ApizrProperOptionsBase : ApizrSharedOptionsBase, IApizrProperOptionsBase
    {
        /// <summary>
        /// The proper options constructor
        /// </summary>
        /// <param name="sharedOptions">The shared options</param>
        /// <param name="webApiType">The web api type</param>
        /// <param name="assemblyPolicyRegistryKeys">Global policies</param>
        /// <param name="webApiPolicyRegistryKeys">Specific policies</param>
        protected ApizrProperOptionsBase(IApizrSharedOptionsBase sharedOptions, 
            Type webApiType,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys)
        {
            BaseUri = sharedOptions.BaseUri;
            BaseAddress = sharedOptions.BaseAddress;
            BasePath = sharedOptions.BasePath;
            HttpTracerMode = sharedOptions.HttpTracerMode;
            TrafficVerbosity = sharedOptions.TrafficVerbosity;
            LogLevels = sharedOptions.LogLevels;
            WebApiType = webApiType;
            PolicyRegistryKeys =
                assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiPolicyRegistryKeys ?? Array.Empty<string>();
        }

        /// <inheritdoc />
        public Type WebApiType { get; }

        /// <inheritdoc />
        public string[] PolicyRegistryKeys { get; }

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }
    }
}
