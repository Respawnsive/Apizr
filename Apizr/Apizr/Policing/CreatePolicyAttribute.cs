using System;

namespace Apizr.Policing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CreatePolicyAttribute : PolicyAttributeBase
    {
        public CreatePolicyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
