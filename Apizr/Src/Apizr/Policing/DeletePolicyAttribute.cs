using System;

namespace Apizr.Policing
{
    /// <summary>
    /// Tells Apizr to apply some policies to Delete method
    /// You have to provide a policy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DeletePolicyAttribute : PolicyAttributeBase
    {
        /// <inheritdoc />
        public DeletePolicyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
