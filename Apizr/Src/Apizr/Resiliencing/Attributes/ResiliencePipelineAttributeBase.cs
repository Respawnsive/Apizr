using System;

namespace Apizr.Resiliencing.Attributes
{
    /// <summary>
    /// Tells Apizr to apply some strategies to all method when decorating an assembly or an interface or a specific one when decorating a method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    public abstract class ResiliencePipelineAttributeBase : Attribute
    {
        /// <summary>
        /// Apply strategies with keys
        /// </summary>
        /// <param name="registryKeys">Strategy registry keys</param>
        protected ResiliencePipelineAttributeBase(params string[] registryKeys)
        {
            RegistryKeys = registryKeys;
        }

        /// <summary>
        /// Strategy registry keys
        /// </summary>
        public string[] RegistryKeys { get; }
    }
}
