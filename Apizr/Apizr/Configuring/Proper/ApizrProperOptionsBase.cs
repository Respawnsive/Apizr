using System;
using System.Linq;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Proper
{
    public abstract class ApizrProperOptionsBase : ApizrSharedOptionsBase, IApizrProperOptionsBase
    {
        protected ApizrProperOptionsBase(IApizrSharedOptionsBase sharedOptions, 
            Type webApiType,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys)
        {
            HttpTracerMode = sharedOptions.HttpTracerMode;
            TrafficVerbosity = sharedOptions.TrafficVerbosity;
            LogLevel = sharedOptions.LogLevel;
            WebApiType = webApiType;
            PolicyRegistryKeys =
                assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiPolicyRegistryKeys ?? Array.Empty<string>();
        }

        public Type WebApiType { get; }
        public Uri BaseAddress { get; protected set; }
        public string[] PolicyRegistryKeys { get; }
        public ILogger Logger { get; protected set; }
    }
}
