﻿using System;
using Apizr.Caching;
using Apizr.Configuring.Shared;
using Apizr.Connecting;
using Apizr.Mapping;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Refit;

namespace Apizr.Configuring.Common
{
    /// <summary>
    /// Options available at common level for static registrations
    /// </summary>
    public interface IApizrCommonOptions : IApizrCommonOptionsBase, IApizrSharedRegistrationOptions
    {
        /// <summary>
        /// The LoggerFactory factory (I know, I know...)
        /// </summary>
        Func<ILoggerFactory> LoggerFactoryFactory { get; set; }

        /// <summary>
        /// Resilience pipeline registry factory
        /// </summary>
        Func<ResiliencePipelineRegistry<string>> ResiliencePipelineRegistryFactory { get; }

        /// <summary>
        /// Refit settings factory
        /// </summary>
        Func<RefitSettings> RefitSettingsFactory { get; }

        /// <summary>
        /// Connectivity handler factory
        /// </summary>
        Func<IConnectivityHandler> ConnectivityHandlerFactory { get; }

        /// <summary>
        /// Cache handler factory
        /// </summary>
        Func<ICacheHandler> CacheHandlerFactory { get; }

        /// <summary>
        /// Mapping handler factory
        /// </summary>
        Func<IMappingHandler> MappingHandlerFactory { get; }
    }
}
