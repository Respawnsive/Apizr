﻿using System;
using System.Collections.Generic;
using System.Linq;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Configuration;
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
            ResiliencePipelineKeys = properOptions.ResiliencePipelineKeys.ToArray();
            Logger = properOptions.Logger;
            RefitSettings = commonOptions.RefitSettings;
            ApizrConfigurationSection = commonOptions.ApizrConfigurationSection;
        }

        /// <inheritdoc />
        public Type WebApiType { get; }

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }

        /// <inheritdoc />
        public RefitSettings RefitSettings { get; protected set; }

        /// <inheritdoc />
        public IConfigurationSection ApizrConfigurationSection { get; }
    }
}