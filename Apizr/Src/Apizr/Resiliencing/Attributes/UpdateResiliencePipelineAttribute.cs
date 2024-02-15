using System;

namespace Apizr.Resiliencing.Attributes
{
    /// <summary>
    /// Tells Apizr to apply some resilience strategies to Update method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public UpdateResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
