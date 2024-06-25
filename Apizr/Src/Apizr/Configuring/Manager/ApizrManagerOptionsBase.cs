using System;
using System.Reflection;
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
            CrudModelType = properOptions.CrudModelType;
            TypeInfo = properOptions.TypeInfo;
            IsCrudApi = properOptions.IsCrudApi;
            Logger = properOptions.Logger;
            RefitSettings = commonOptions.RefitSettings;
            ApizrConfigurationSection = commonOptions.ApizrConfigurationSection;
        }

        /// <inheritdoc />
        public Type WebApiType { get; }

        /// <inheritdoc />
        public Type CrudModelType { get; }

        /// <inheritdoc />
        public TypeInfo TypeInfo { get; }

        /// <inheritdoc />
        public bool IsCrudApi { get; }

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }

        /// <inheritdoc />
        public RefitSettings RefitSettings { get; protected set; }

        /// <inheritdoc />
        public IConfigurationSection ApizrConfigurationSection { get; }
    }
}