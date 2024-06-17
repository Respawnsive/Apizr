using System;
using Apizr.Configuring;

namespace Apizr.Resiliencing.Attributes.Http
{
    /// <summary>
    /// Tells Apizr to apply some policies to Get http method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface)]
    public class GetResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public GetResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
            RequestMethod = ApizrRequestMethod.HttpGet;
        }
    }
}
