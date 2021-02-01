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
            HttpMessageParts? httpTracerVerbosity,
            ApizrLogLevel? apizrVerbosity,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys) : base(webApiType,
            assemblyPolicyRegistryKeys, webApiPolicyRegistryKeys)
        {
            BaseAddressFactory = () => baseAddress;
            HttpTracerVerbosityFactory = () => httpTracerVerbosity ?? HttpMessageParts.None;
            ApizrVerbosityFactory = () => apizrVerbosity ?? ApizrLogLevel.None;
            HttpClientHandlerFactory = () => new HttpClientHandler();
            PolicyRegistryFactory = () => new PolicyRegistry();
            RefitSettingsFactory = () => new RefitSettings();
            ConnectivityHandlerFactory = () => new VoidConnectivityHandler();
            CacheHandlerFactory = () => new VoidCacheHandler();
            LogHandlerFactory = () => new DefaultLogHandler();
            MappingHandlerFactory = () => new VoidMappingHandler();
            DelegatingHandlersFactories = new List<Func<ILogHandler, DelegatingHandler>>();
        }

        private Func<Uri> _baseAddressFactory;
        public Func<Uri> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = () => BaseAddress = value.Invoke();
        }

        private Func<HttpMessageParts> _httpTracerVerbosityFactory;
        public Func<HttpMessageParts> HttpTracerVerbosityFactory
        {
            get => _httpTracerVerbosityFactory;
            set => _httpTracerVerbosityFactory = () => HttpTracerVerbosity = value.Invoke();
        }

        private Func<ApizrLogLevel> _apizrVerbosityFactory;
        public Func<ApizrLogLevel> ApizrVerbosityFactory
        {
            get => _apizrVerbosityFactory;
            set => _apizrVerbosityFactory = () => ApizrVerbosity = value.Invoke();
        }

        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }
        public Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; set;  }
        public Func<RefitSettings> RefitSettingsFactory { get; set; }
        public Func<IConnectivityHandler> ConnectivityHandlerFactory { get; set; }
        public Func<ICacheHandler> CacheHandlerFactory { get; set; }
        public Func<ILogHandler> LogHandlerFactory { get; set; }
        public Func<IMappingHandler> MappingHandlerFactory { get; set; }
        public IList<Func<ILogHandler, DelegatingHandler>> DelegatingHandlersFactories { get; }
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
        public HttpMessageParts HttpTracerVerbosity => _apizrOptions.HttpTracerVerbosity;
        public ApizrLogLevel ApizrVerbosity => _apizrOptions.ApizrVerbosity;
        public string[] PolicyRegistryKeys => _apizrOptions.PolicyRegistryKeys;
    }
}