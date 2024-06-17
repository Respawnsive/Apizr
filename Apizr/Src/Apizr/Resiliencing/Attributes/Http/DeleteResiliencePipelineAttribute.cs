using System;
using Apizr.Configuring;

namespace Apizr.Resiliencing.Attributes.Http
{
    /// <summary>
    /// Tells Apizr to apply some policies to Delete http method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface)]
    public class DeleteResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public DeleteResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
            RequestMethod = ApizrRequestMethod.HttpDelete;
        }
    }
}
