using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Configuring
{
    /// <inheritdoc cref="IApizrOptionsBase" />
    public abstract class ApizrOptionsBase : ApizrSharedOptionsBase, IApizrOptionsBase
    {
        /// <summary>
        /// The options constructor
        /// </summary>
        /// <param name="commonOptions">The common options</param>
        /// <param name="properOptions">The proper options</param>
        protected ApizrOptionsBase(IApizrCommonOptionsBase commonOptions, IApizrProperOptionsBase properOptions)
        {
            WebApiType = properOptions.WebApiType;
            BaseUri = properOptions.BaseUri;
            BaseAddress = properOptions.BaseAddress;
            BasePath = properOptions.BasePath;
            HttpTracerMode = properOptions.HttpTracerMode;
            PolicyRegistryKeys = properOptions.PolicyRegistryKeys;
        }

        /// <inheritdoc />
        public Type WebApiType { get; }

        /// <inheritdoc />
        public string[] PolicyRegistryKeys { get; }

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }

        /// <inheritdoc />
        public RefitSettings RefitSettings { get; protected set; }
    }
}