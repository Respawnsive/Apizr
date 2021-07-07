using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Mapping;
using HttpTracer;
using Microsoft.Extensions.Logging;
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
        /// Http traffic tracing verbosity factory
        /// </summary>
        Func<HttpMessageParts> TrafficVerbosityFactory { get; }

        /// <summary>
        /// Http traffic tracing log level factory
        /// </summary>
        Func<LogLevel> TrafficLogLevelFactory { get; }

        /// <summary>
        /// Logger factory
        /// </summary>
        Func<ILogger> LoggerFactory { get; set; }

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
        /// Mapping handler factory
        /// </summary>
        Func<IMappingHandler> MappingHandlerFactory { get; }

        /// <summary>
        /// Delegating handlers factories
        /// </summary>
        IList<Func<ILogger, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }

    public interface IApizrOptions<TWebApi> : IApizrOptionsBase
    {
        ILogger<TWebApi> Logger { get; }
    }
}
