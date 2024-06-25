using System;
using Apizr.Configuring;

namespace Apizr.Resiliencing.Attributes.Http
{
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
    /// <summary>
    /// Tells Apizr to apply some policies to Patch http method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface)]
    public class PatchResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public PatchResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
            RequestMethod = ApizrRequestMethod.HttpPatch;
        }
    }
#endif
}
