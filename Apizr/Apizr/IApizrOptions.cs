using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using Polly.Registry;
using Refit;

namespace Apizr
{
    /// <summary>
    /// Options
    /// </summary>
    public interface IApizrOptions : IApizrOptionsBase
    {
        /// <summary>
        /// Base address factory
        /// </summary>
        Func<Uri> BaseAddressFactory { get; }

        /// <summary>
        /// HttpClientHandler factory
        /// </summary>
        Func<HttpClientHandler> HttpClientHandlerFactory { get; }

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
        /// Log handler factory
        /// </summary>
        Func<ILogHandler> LogHandlerFactory { get; }

        /// <summary>
        /// Mapping handler factory
        /// </summary>
        Func<IMappingHandler> MappingHandlerFactory { get; }

        /// <summary>
        /// Delegating handlers factories
        /// </summary>
        IList<Func<ILogHandler, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}
