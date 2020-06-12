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
    public interface IApizrOptions
    {
        Type WebApiType { get; }
        Uri BaseAddress { get; }
        DecompressionMethods DecompressionMethods { get; }
        HttpMessageParts HttpTracerVerbosity { get; }
        string[] PolicyRegistryKeys { get; }
        Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; }
        Func<RefitSettings> RefitSettingsFactory { get; }
        Func<IConnectivityHandler> ConnectivityHandlerFactory { get; }
        Func<ICacheProvider> CacheProviderFactory { get; }
        Func<ILogHandler> LogHandlerFactory { get; }
        IList<Func<ILogHandler, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}
