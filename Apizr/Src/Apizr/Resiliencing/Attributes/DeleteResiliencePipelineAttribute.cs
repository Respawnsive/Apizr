using System;

namespace Apizr.Resiliencing.Attributes
{
    /// <summary>
    /// Tells Apizr to apply some resilience strategies to Delete method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DeleteResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public DeleteResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
