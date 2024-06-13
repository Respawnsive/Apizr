﻿using System;

namespace Apizr.Resiliencing.Attributes.Rest
{
    /// <summary>
    /// Tells Apizr to apply some policies to Head http method
    /// You have to provide a strategy registry to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface)]
    public class HeadResiliencePipelineAttribute : ResiliencePipelineAttributeBase
    {
        /// <inheritdoc />
        public HeadResiliencePipelineAttribute(params string[] registryKeys) : base(registryKeys)
        {
        }
    }
}
