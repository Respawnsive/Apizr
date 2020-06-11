using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ApizrOptions : IApizrOptions
    {
        public ApizrOptions(Type webApiType, Uri baseAddress, DecompressionMethods? decompressionMethods,
            HttpMessageParts? httpTracerVerbosity, string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys)
        {
            WebApiType = webApiType;
            BaseAddress = baseAddress;
            DecompressionMethods = decompressionMethods ?? DecompressionMethods.None;
            HttpTracerVerbosity = httpTracerVerbosity ?? HttpMessageParts.None;
            PolicyRegistryKeys =
                assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiPolicyRegistryKeys ?? Array.Empty<string>();
            PolicyRegistryFactory = () => new PolicyRegistry();
            RefitSettingsFactory = () => new RefitSettings();
            ConnectivityHandlerFactory = () => new VoidConnectivityHandler();
            CacheProviderFactory = () => new VoidCacheProvider();
            LogHandlerFactory = () => new DefaultLogHandler();
            DelegatingHandlersFactories = new List<Func<DelegatingHandler>>();
        }

        public Type WebApiType { get; }
        public Uri BaseAddress { get; }
        public DecompressionMethods DecompressionMethods { get; }
        public HttpMessageParts HttpTracerVerbosity { get; }
        public string[] PolicyRegistryKeys { get; }
        public Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; set;  }
        public Func<RefitSettings> RefitSettingsFactory { get; set; }
        public Func<IConnectivityHandler> ConnectivityHandlerFactory { get; set; }
        public Func<ICacheProvider> CacheProviderFactory { get; set; }
        public Func<ILogHandler> LogHandlerFactory { get; set; }
        public IList<Func<DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}