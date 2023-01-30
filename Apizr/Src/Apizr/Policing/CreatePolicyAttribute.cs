using System;

namespace Apizr.Policing
{
    /// <summary>
    /// Tells Apizr to apply some policies to Create method
    /// You have to provide a policy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CreatePolicyAttribute : PolicyAttributeBase
    {
        /// <inheritdoc />
        public CreatePolicyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
