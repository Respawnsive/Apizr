using System;

namespace Apizr.Policing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdatePolicyAttribute : PolicyAttributeBase
    {
        public UpdatePolicyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
