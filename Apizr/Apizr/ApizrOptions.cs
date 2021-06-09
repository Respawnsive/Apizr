using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Mapping;
using HttpTracer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public class ApizrOptions : ApizrOptionsBase, IApizrOptions
    {

        public ApizrOptions(Type webApiType, Uri baseAddress,
            HttpMessageParts? httpTracerVerbosity,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys) : base(webApiType,
            assemblyPolicyRegistryKeys, webApiPolicyRegistryKeys)
        {
            BaseAddressFactory = () => baseAddress;
            TrafficVerbosityFactory = () => httpTracerVerbosity ?? HttpMessageParts.None;
            HttpClientHandlerFactory = () => new HttpClientHandler();
            PolicyRegistryFactory = () => new PolicyRegistry();
            RefitSettingsFactory = () => new RefitSettings();
            ConnectivityHandlerFactory = () => new VoidConnectivityHandler();
            CacheHandlerFactory = () => new VoidCacheHandler();
            LoggerFactory = () => NullLogger.Instance;
            MappingHandlerFactory = () => new VoidMappingHandler();
            DelegatingHandlersFactories = new List<Func<ILogger, IApizrOptionsBase, DelegatingHandler>>();
        }

        private Func<Uri> _baseAddressFactory;
        public Func<Uri> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = () => BaseAddress = value.Invoke();
        }

        private Func<HttpMessageParts> _httpTracerVerbosityFactory;
        public Func<HttpMessageParts> TrafficVerbosityFactory
        {
            get => _httpTracerVerbosityFactory;
            set => _httpTracerVerbosityFactory = () => TrafficVerbosity = value.Invoke();
        }

        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }
        public Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; set;  }
        public Func<RefitSettings> RefitSettingsFactory { get; set; }
        public Func<IConnectivityHandler> ConnectivityHandlerFactory { get; set; }
        public Func<ICacheHandler> CacheHandlerFactory { get; set; }
        public Func<ILogger> LoggerFactory { get; set; }
        public Func<IMappingHandler> MappingHandlerFactory { get; set; }
        public IList<Func<ILogger, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }

    public class ApizrOptions<TWebApi> : IApizrOptions<TWebApi>
    {
        private readonly IApizrOptionsBase _apizrOptions;

        public ApizrOptions(IApizrOptionsBase apizrOptions)
        {
            _apizrOptions = apizrOptions;
        }

        public Type WebApiType => _apizrOptions.WebApiType;
        public Uri BaseAddress => _apizrOptions.BaseAddress;
        public HttpMessageParts TrafficVerbosity => _apizrOptions.TrafficVerbosity;
        public string[] PolicyRegistryKeys => _apizrOptions.PolicyRegistryKeys;
    }
}