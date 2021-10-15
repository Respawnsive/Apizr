using System;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Shared;

namespace Apizr.Extending.Configuring.Proper
{
    public class ApizrExtendedProperOptions : ApizrProperOptionsBase, IApizrExtendedProperOptions
    {
        public ApizrExtendedProperOptions(IApizrSharedOptionsBase sharedOptions, 
            Type webApiType, 
            string[] assemblyPolicyRegistryKeys, 
            string[] webApiPolicyRegistryKeys) : base(sharedOptions, 
            webApiType, 
            assemblyPolicyRegistryKeys, 
            webApiPolicyRegistryKeys)
        {
        }
    }
}
