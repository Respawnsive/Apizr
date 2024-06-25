using System;

namespace Apizr.Resiliencing.Attributes
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class ResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public ResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }

    }
}
