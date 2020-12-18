using System;

namespace Apizr.Policing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DeletePolicyAttribute : PolicyAttributeBase
    {
        public DeletePolicyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
