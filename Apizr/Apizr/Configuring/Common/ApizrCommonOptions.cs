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

namespace Apizr.Configuring.Common
{
    public class ApizrCommonOptions : ApizrCommonOptionsBase, IApizrCommonOptions
    {
        public ApizrCommonOptions()
        {
            HttpTracerModeFactory = () => HttpTracerMode.Everything;
            TrafficVerbosityFactory = () => HttpMessageParts.All;
            LogLevelsFactory = () => new []{Constants.LowLogLevel, Constants.MediumLogLevel, Constants.HighLogLevel};
            LoggerFactoryFactory = () => new DebugLoggerFactory(LogLevel.Trace);
            PolicyRegistryFactory = () => new PolicyRegistry();
            HttpClientHandlerFactory = () => new HttpClientHandler();
            RefitSettingsFactory = () => new RefitSettings();
            ConnectivityHandlerFactory = () => new DefaultConnectivityHandler(() => true);
            CacheHandlerFactory = () => new VoidCacheHandler();
            MappingHandlerFactory = () => new VoidMappingHandler();
            DelegatingHandlersFactories = new List<Func<ILogger, IApizrOptionsBase, DelegatingHandler>>();
        }

        public Func<ILoggerFactory> LoggerFactoryFactory { get; set; }
        public Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; set; }
        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }

        private Func<RefitSettings> _refitSettingsFactory;
        public Func<RefitSettings> RefitSettingsFactory
        {
            get => _refitSettingsFactory;
            set => _refitSettingsFactory = () => RefitSettings = value.Invoke();
        }

        public Func<IConnectivityHandler> ConnectivityHandlerFactory { get; set; }
        public Func<ICacheHandler> CacheHandlerFactory { get; set; }
        public Func<IMappingHandler> MappingHandlerFactory { get; set; }
        public IList<Func<ILogger, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }

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

        private Func<LogLevel[]> _logLevelsFactory;
        public Func<LogLevel[]> LogLevelsFactory
        {
            get => _logLevelsFactory;
            set => _logLevelsFactory = () => LogLevels = value.Invoke();
        }
    }
}
