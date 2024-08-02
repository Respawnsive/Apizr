using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Request;
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
            CrudApiEntityType = properOptions.CrudApiEntityType;
            TypeInfo = properOptions.TypeInfo;
            IsCrudApi = properOptions.IsCrudApi;
            Logger = properOptions.Logger;
            RefitSettings = commonOptions.RefitSettings;
            ApizrConfigurationSection = commonOptions.ApizrConfigurationSection;
            RequestOptionsBuilders = properOptions.RequestOptionsBuilders?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];
        }

        /// <inheritdoc />
        public Type WebApiType { get; }

        /// <inheritdoc />
        public Type CrudApiEntityType { get; }

        /// <inheritdoc />
        public TypeInfo TypeInfo { get; }

        /// <inheritdoc />
        public bool IsCrudApi { get; }

        /// <inheritdoc />
        public ILogger Logger { get; protected set; }

        /// <inheritdoc />
        public IDictionary<string, Action<IApizrRequestOptionsBuilder>> RequestOptionsBuilders { get; }

        /// <inheritdoc />
        public RefitSettings RefitSettings { get; protected set; }

        /// <inheritdoc />
        public IConfigurationSection ApizrConfigurationSection { get; }
    }
}