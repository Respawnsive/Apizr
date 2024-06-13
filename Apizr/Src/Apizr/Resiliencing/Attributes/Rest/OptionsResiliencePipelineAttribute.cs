using System;

namespace Apizr.Resiliencing.Attributes.Rest
{
    /// <summary>
    /// Tells Apizr to apply some policies to Options http method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface)]
    public class OptionsResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public OptionsResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
