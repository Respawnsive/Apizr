using System;

namespace Apizr.Resiliencing.Attributes
{
    /// <summary>
    /// Tells Apizr to apply some resilience strategies to ReadAll method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ReadAllResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public ReadAllResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
