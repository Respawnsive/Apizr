using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
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
            ConnectivityHandlerType = typeof(VoidConnectivityHandler);
            CacheProviderType = typeof(VoidCacheProvider);
            LogHandlerType = typeof(DefaultLogHandler);
            DelegatingHandlersExtendedFactories = new List<Func<IServiceProvider, DelegatingHandler>>();
        }

        public Type ApizrManagerType { get; }
        public Type ConnectivityHandlerType { get; set; }
        public Type CacheProviderType { get; set; }
        public Type LogHandlerType { get; set; }
        public Action<IHttpClientBuilder> HttpClientBuilder { get; set; }
        public IList<Func<IServiceProvider, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
    }
}
