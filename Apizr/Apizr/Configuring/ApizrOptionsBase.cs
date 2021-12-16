using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Shared;
using Refit;

namespace Apizr.Configuring
{
    public abstract class ApizrOptionsBase : ApizrSharedOptionsBase, IApizrOptionsBase
    {
        protected ApizrOptionsBase(IApizrCommonOptionsBase commonOptions, IApizrProperOptionsBase properOptions)
        {
            WebApiType = properOptions.WebApiType;
            BaseAddress = properOptions.BaseAddress;
            HttpTracerMode = properOptions.HttpTracerMode;
            PolicyRegistryKeys = properOptions.PolicyRegistryKeys;
        }

        public Type WebApiType { get; }
        public Uri BaseAddress { get; protected set; }
        public string[] PolicyRegistryKeys { get; }
        public RefitSettings RefitSettings { get; protected set; }
    }
}