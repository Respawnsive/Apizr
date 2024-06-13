using System;

namespace Apizr.Resiliencing.Attributes.Rest
{
    /// <summary>
    /// Tells Apizr to apply some policies to Put http method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface)]
    public class PutResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public PutResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
