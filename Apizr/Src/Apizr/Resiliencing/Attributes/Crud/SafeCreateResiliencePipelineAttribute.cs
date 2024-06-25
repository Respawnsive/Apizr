using System;
using Apizr.Configuring;

namespace Apizr.Resiliencing.Attributes.Crud
{
    /// <summary>
    /// Tells Apizr to apply some policies to Create method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class SafeCreateResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public SafeCreateResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
            RequestMethod = ApizrRequestMethod.CrudSafeCreate;
        }
    }
}
