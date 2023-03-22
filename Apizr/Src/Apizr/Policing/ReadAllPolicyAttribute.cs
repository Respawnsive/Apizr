using System;

namespace Apizr.Policing
{
    /// <summary>
    /// Tells Apizr to apply some policies to ReadAll method
    /// You have to provide a policy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadAllPolicyAttribute : PolicyAttributeBase
    {
        /// <inheritdoc />
        public ReadAllPolicyAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
