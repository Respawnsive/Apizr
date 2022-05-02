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
            BaseAddress = sharedOptions.BaseAddress;
            HttpTracerMode = sharedOptions.HttpTracerMode;
            TrafficVerbosity = sharedOptions.TrafficVerbosity;
            LogLevels = sharedOptions.LogLevels;
            WebApiType = webApiType;
            PolicyRegistryKeys =
                assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiPolicyRegistryKeys ?? Array.Empty<string>();
        }

        public Type WebApiType { get; }
        public string[] PolicyRegistryKeys { get; }
        public ILogger Logger { get; protected set; }
    }
}
