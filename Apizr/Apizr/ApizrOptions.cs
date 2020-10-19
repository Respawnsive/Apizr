using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using HttpTracer;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public class ApizrOptions : ApizrOptionsBase, IApizrOptions
    {
        public ApizrOptions(Type webApiType, Uri baseAddress,
            HttpMessageParts? httpTracerVerbosity, bool? isPriorityManagementEnabled,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys) : base(webApiType, httpTracerVerbosity,
            isPriorityManagementEnabled,
            assemblyPolicyRegistryKeys, webApiPolicyRegistryKeys)
        {
            BaseAddressFactory = () => baseAddress;
            HttpClientHandlerFactory = () => new HttpClientHandler();
            PolicyRegistryFactory = () => new PolicyRegistry();
            RefitSettingsFactory = () => new RefitSettings();
            ConnectivityHandlerFactory = () => new VoidConnectivityHandler();
            CacheHandlerFactory = () => new VoidCacheHandler();
            LogHandlerFactory = () => new DefaultLogHandler();
            MappingHandlerFactory = () => new VoidMappingHandler();
            DelegatingHandlersFactories = new List<Func<ILogHandler, DelegatingHandler>>();
        }

        public Func<Uri> BaseAddressFactory { get; set; }
        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }
        public Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; set;  }
        public Func<RefitSettings> RefitSettingsFactory { get; set; }
        public Func<IConnectivityHandler> ConnectivityHandlerFactory { get; set; }
        public Func<ICacheHandler> CacheHandlerFactory { get; set; }
        public Func<ILogHandler> LogHandlerFactory { get; set; }
        public Func<IMappingHandler> MappingHandlerFactory { get; set; }
        public IList<Func<ILogHandler, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}