using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Configuring
{
    public abstract class ApizrOptionsBase : ApizrSharedOptionsBase, IApizrOptionsBase
    {
        protected ApizrOptionsBase(IApizrCommonOptionsBase commonOptions, IApizrProperOptionsBase properOptions)
        {
            WebApiType = properOptions.WebApiType;
            BaseUri = properOptions.BaseUri;
            BaseAddress = properOptions.BaseAddress;
            BasePath = properOptions.BasePath;
            HttpTracerMode = properOptions.HttpTracerMode;
            PolicyRegistryKeys = properOptions.PolicyRegistryKeys;
        }

        public Type WebApiType { get; }
        public string[] PolicyRegistryKeys { get; }
        public ILogger Logger { get; protected set; }
        public RefitSettings RefitSettings { get; protected set; }
    }
}