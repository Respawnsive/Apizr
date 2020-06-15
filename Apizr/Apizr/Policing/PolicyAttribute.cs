using System;

namespace Apizr.Policing
{
    /// <summary>
    /// Tells Apizr to apply some policies to all method when decorating an assembly or an interface or a specific one when decorating a method
    /// You have to provide a policy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Method)]
    public class PolicyAttribute : Attribute
    {
        /// <summary>
        /// Apply policies with keys
        /// </summary>
        /// <param name="registryKeys">Policy registry keys</param>
        public PolicyAttribute(params string[] registryKeys)
        {
            RegistryKeys = registryKeys;
        }

        /// <summary>
        /// Policy registry keys
        /// </summary>
        public string[] RegistryKeys { get; }
    }
}
