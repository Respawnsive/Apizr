using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public class ApizrOptions : ApizrOptionsBase, IApizrOptions
    {

        public ApizrOptions(Type webApiType, Uri baseAddress,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            LogLevel? logLevel,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys) : base(webApiType,
            assemblyPolicyRegistryKeys, webApiPolicyRegistryKeys)
        {
            BaseAddressFactory = () => baseAddress;
            HttpTracerModeFactory = () => httpTracerMode ?? HttpTracerMode.Everything;
            TrafficVerbosityFactory = () => trafficVerbosity ?? HttpMessageParts.None;
            LogLevelFactory = () => logLevel ?? LogLevel.None;
            LoggerFactory = () => new DebugLoggerFactory(LogLevel.Information);
            HttpClientHandlerFactory = () => new HttpClientHandler();
            PolicyRegistryFactory = () => new PolicyRegistry();
            RefitSettingsFactory = () => new RefitSettings();
            ConnectivityHandlerFactory = () => new DefaultConnectivityHandler();
            CacheHandlerFactory = () => new VoidCacheHandler();
            MappingHandlerFactory = () => new VoidMappingHandler();
            DelegatingHandlersFactories = new List<Func<ILogger, IApizrOptionsBase, DelegatingHandler>>();
        }

        private Func<Uri> _baseAddressFactory;
        public Func<Uri> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = () => BaseAddress = value.Invoke();
        }

        private Func<HttpTracerMode> _httpTracerModeFactory;
        public Func<HttpTracerMode> HttpTracerModeFactory
        {
            get => _httpTracerModeFactory;
            set => _httpTracerModeFactory = () => HttpTracerMode = value.Invoke();
        }

        private Func<HttpMessageParts> _trafficVerbosityFactory;
        public Func<HttpMessageParts> TrafficVerbosityFactory
        {
            get => _trafficVerbosityFactory;
            set => _trafficVerbosityFactory = () => TrafficVerbosity = value.Invoke();
        }
        
        private Func<LogLevel> _logLevelFactory;
        public Func<LogLevel> LogLevelFactory
        {
            get => _logLevelFactory;
            set => _logLevelFactory = () => LogLevel = value.Invoke();
        }

        public Func<ILoggerFactory> LoggerFactory { get; set; }
        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }
        public Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; set;  }

        private Func<RefitSettings> _refitSettingsFactory;
        public Func<RefitSettings> RefitSettingsFactory
        {
            get => _refitSettingsFactory;
            set => _refitSettingsFactory = () =>
            {
                var refitSettings = value.Invoke();
                ContentSerializer = refitSettings.ContentSerializer;
                return refitSettings;
            };
        }

        public Func<IConnectivityHandler> ConnectivityHandlerFactory { get; set; }
        public Func<ICacheHandler> CacheHandlerFactory { get; set; }
        public Func<IMappingHandler> MappingHandlerFactory { get; set; }
        public IList<Func<ILogger, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }

    public class ApizrOptions<TWebApi> : IApizrOptions<TWebApi>
    {
        private readonly IApizrOptionsBase _apizrOptions;
        private readonly ILogger _logger;

        public ApizrOptions(IApizrOptionsBase apizrOptions, ILogger logger)
        {
            _apizrOptions = apizrOptions;
            _logger = logger;
        }

        public Type WebApiType => _apizrOptions.WebApiType;
        public Uri BaseAddress => _apizrOptions.BaseAddress;
        public HttpTracerMode HttpTracerMode => _apizrOptions.HttpTracerMode;
        public HttpMessageParts TrafficVerbosity => _apizrOptions.TrafficVerbosity;
        public LogLevel LogLevel => _apizrOptions.LogLevel;
        public ILogger Logger => _logger;
        public string[] PolicyRegistryKeys => _apizrOptions.PolicyRegistryKeys;
        public IHttpContentSerializer ContentSerializer => _apizrOptions.ContentSerializer;
    }
}