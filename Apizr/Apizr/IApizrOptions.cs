using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using HttpTracer;
using Polly.Registry;
using Refit;

namespace Apizr
{
    /// <summary>
    /// Options
    /// </summary>
    public interface IApizrOptions
    {
        /// <summary>
        /// Web api interface type
        /// </summary>
        Type WebApiType { get; }

        /// <summary>
        /// Web api base address
        /// </summary>
        Uri BaseAddress { get; }

        /// <summary>
        /// HttpClient decompression methods
        /// </summary>
        DecompressionMethods DecompressionMethods { get; }

        /// <summary>
        /// Request tracing verbosity
        /// </summary>
        HttpMessageParts HttpTracerVerbosity { get; }

        /// <summary>
        /// Policy keys from the registry
        /// </summary>
        string[] PolicyRegistryKeys { get; }

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
        /// Cache provider factory
        /// </summary>
        Func<ICacheProvider> CacheProviderFactory { get; }

        /// <summary>
        /// Log handler factory
        /// </summary>
        Func<ILogHandler> LogHandlerFactory { get; }

        /// <summary>
        /// Delegating handlers factories
        /// </summary>
        IList<Func<ILogHandler, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}
