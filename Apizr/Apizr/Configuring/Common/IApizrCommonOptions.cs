using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Configuring.Shared;
using Apizr.Connecting;
using Apizr.Mapping;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Refit;

namespace Apizr.Configuring.Common
{
    public interface IApizrCommonOptions : IApizrCommonOptionsBase, IApizrSharedOptions
    {
        /// <summary>
        /// Logger factory
        /// </summary>
        Func<ILoggerFactory> LoggerFactory { get; set; }

        /// <summary>
        /// Policy registry factory
        /// </summary>
        Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; }

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
