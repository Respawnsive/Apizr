using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Refit;

namespace Apizr.Configuring
{
    public class ApizrOptions : ApizrOptionsBase, IApizrOptions
    {
        public ApizrOptions(IApizrCommonOptions commonOptions, IApizrProperOptions properOptions) : base(commonOptions, properOptions)
        {
            BaseUriFactory = properOptions.BaseUriFactory;
            BaseAddressFactory = properOptions.BaseAddressFactory;
            BasePathFactory = properOptions.BasePathFactory;
            HttpTracerModeFactory = properOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = properOptions.TrafficVerbosityFactory;
            LogLevelsFactory = properOptions.LogLevelsFactory;
            LoggerFactoryFactory = commonOptions.LoggerFactoryFactory;
            LoggerFactory = (loggerFactory, webApiFriendlyName) => Logger = properOptions.LoggerFactory.Invoke(loggerFactory, webApiFriendlyName);
            HttpClientHandlerFactory = properOptions.HttpClientHandlerFactory;
            PolicyRegistryFactory = commonOptions.PolicyRegistryFactory;
            RefitSettingsFactory = commonOptions.RefitSettingsFactory;
            ConnectivityHandlerFactory = commonOptions.ConnectivityHandlerFactory;
            CacheHandlerFactory = commonOptions.CacheHandlerFactory;
            MappingHandlerFactory = commonOptions.MappingHandlerFactory;
            DelegatingHandlersFactories = properOptions.DelegatingHandlersFactories;
        }

        private Func<Uri> _baseUriFactory;
        public Func<Uri> BaseUriFactory
        {
            get => _baseUriFactory;
            set => _baseUriFactory = value != null ? () => BaseUri = value.Invoke() : null;
        }

        private Func<string> _baseAddressFactory;
        public Func<string> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = value != null ? () => BaseAddress = value.Invoke() : null;
        }

        private Func<string> _basePathFactory;
        public Func<string> BasePathFactory
        {
            get => _basePathFactory;
            set => _basePathFactory = value != null ? () => BasePath = value.Invoke() : null;
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
        
        private Func<LogLevel[]> _logLevelsFactory;
        public Func<LogLevel[]> LogLevelsFactory
        {
            get => _logLevelsFactory;
            set => _logLevelsFactory = () => LogLevels = value.Invoke();
        }

        public Func<ILoggerFactory> LoggerFactoryFactory { get; set; }

        public Func<ILoggerFactory, string, ILogger> LoggerFactory { get; }

        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }

        public Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; set;  }

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
    }

    public class ApizrOptions<TWebApi> : IApizrOptions<TWebApi>
    {
        protected readonly IApizrOptionsBase Options;

        public ApizrOptions(IApizrOptionsBase apizrOptions)
        {
            Options = apizrOptions;
        }

        public Type WebApiType => Options.WebApiType;
        public Uri BaseUri => Options.BaseUri;
        public string BaseAddress => Options.BaseAddress;
        public string BasePath => Options.BasePath;
        public HttpTracerMode HttpTracerMode => Options.HttpTracerMode;
        public HttpMessageParts TrafficVerbosity => Options.TrafficVerbosity;
        public LogLevel[] LogLevels => Options.LogLevels;
        public ILogger Logger => Options.Logger;
        public string[] PolicyRegistryKeys => Options.PolicyRegistryKeys;
        public RefitSettings RefitSettings => Options.RefitSettings;
    }
}