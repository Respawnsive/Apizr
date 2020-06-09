using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
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
            ConnectivityProviderFactory = () => new VoidConnectivityProvider();
            CacheProviderFactory = () => new VoidCacheProvider();
            DelegatingHandlersFactories = new List<Func<DelegatingHandler>>();
        }

        public Type WebApiType { get; }
        public Uri BaseAddress { get; }
        public DecompressionMethods DecompressionMethods { get; }
        public HttpMessageParts HttpTracerVerbosity { get; }
        public string[] PolicyRegistryKeys { get; }
        public Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; set;  }
        public Func<RefitSettings> RefitSettingsFactory { get; set; }
        public Func<IConnectivityProvider> ConnectivityProviderFactory { get; set; }
        public Func<ICacheProvider> CacheProviderFactory { get; set; }
        public IList<Func<DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}