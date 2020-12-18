using System;

namespace Apizr.Policing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadAllPolicyAttribute : PolicyAttributeBase
    {
        public ReadAllPolicyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
