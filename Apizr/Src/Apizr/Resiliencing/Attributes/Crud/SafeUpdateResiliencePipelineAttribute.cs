using System;
using Apizr.Configuring;

namespace Apizr.Resiliencing.Attributes.Crud
{
    /// <summary>
    /// Tells Apizr to apply some resilience strategies to Update method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class SafeUpdateResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public SafeUpdateResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
            RequestMethod = ApizrRequestMethod.CrudSafeUpdate;
        }
    }
}
