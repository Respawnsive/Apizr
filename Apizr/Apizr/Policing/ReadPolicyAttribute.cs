using System;

namespace Apizr.Policing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadPolicyAttribute : PolicyAttributeBase
    {
        public ReadPolicyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
