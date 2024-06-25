using System;
using Apizr.Configuring;

namespace Apizr.Resiliencing.Attributes.Crud
{
    /// <summary>
    /// Tells Apizr to apply some resilience strategies to Read method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class ReadResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public ReadResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
            RequestMethod = ApizrRequestMethod.CrudRead;
        }
    }
}
