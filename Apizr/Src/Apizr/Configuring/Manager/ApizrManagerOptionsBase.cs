using System;
using System.Linq;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Configuring.Manager
{
    /// <inheritdoc cref="IApizrManagerOptionsBase" />
    public abstract class ApizrManagerOptionsBase : ApizrGlobalSharedRegistrationOptionsBase, IApizrManagerOptionsBase
    {
        /// <summary>
        /// The options constructor
        /// </summary>
        /// <param name="commonOptions">The common options</param>
        /// <param name="properOptions">The proper options</param>
        protected ApizrManagerOptionsBase(IApizrCommonOptionsBase commonOptions, IApizrProperOptionsBase properOptions) : base(properOptions)
        {
            WebApiType = properOptions.WebApiType;
            ResiliencePipelineRegistryKeys = properOptions.ResiliencePipelineRegistryKeys.ToArray();
            Logger = properOptions.Logger;
            RefitSettings = commonOptions.RefitSettings;
        }

        /// <inheritdoc />
        public Type WebApiType { get; }

        /// <inheritdoc />
        public string[] ResiliencePipelineRegistryKeys { get; }

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }

        /// <inheritdoc />
        public RefitSettings RefitSettings { get; protected set; }
    }
}