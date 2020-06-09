using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
using HttpTracer;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr
{
    public class ApizrExtendedOptions : ApizrOptions, IApizrExtendedOptions
    {
        public ApizrExtendedOptions(Type webApiType, Type apizrManagerType, Uri baseAddress,
            DecompressionMethods? decompressionMethods,
            HttpMessageParts? httpTracerVerbosity, string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys) : base(webApiType, baseAddress, decompressionMethods,
            httpTracerVerbosity, assemblyPolicyRegistryKeys, webApiPolicyRegistryKeys)
        {
            ApizrManagerType = apizrManagerType;
            ConnectivityProviderType = typeof(VoidConnectivityProvider);
            CacheProviderType = typeof(VoidCacheProvider);
            DelegatingHandlersExtendedFactories = new List<Func<IServiceProvider, DelegatingHandler>>();
        }

        public Type ApizrManagerType { get; }
        public Type ConnectivityProviderType { get; set; }
        public Type CacheProviderType { get; set; }
        public Action<IHttpClientBuilder> HttpClientBuilder { get; set; }
        public IList<Func<IServiceProvider, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
    }
}
