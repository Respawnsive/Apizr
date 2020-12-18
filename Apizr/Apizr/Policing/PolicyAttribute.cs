using System;

namespace Apizr.Policing
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class PolicyAttribute : PolicyAttributeBase
    {
        public PolicyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }

    }
}
