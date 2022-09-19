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
    /// <inheritdoc cref="IApizrOptions"/>
    public class ApizrOptions : ApizrOptionsBase, IApizrOptions
    {
        /// <summary>
        /// The options constructor
        /// </summary>
        /// <param name="commonOptions">The common options</param>
        /// <param name="properOptions">The proper options</param>
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
            HttpClientFactory = properOptions.HttpClientFactory;
            PolicyRegistryFactory = commonOptions.PolicyRegistryFactory;
            RefitSettingsFactory = commonOptions.RefitSettingsFactory;
            ConnectivityHandlerFactory = commonOptions.ConnectivityHandlerFactory;
            CacheHandlerFactory = commonOptions.CacheHandlerFactory;
            MappingHandlerFactory = commonOptions.MappingHandlerFactory;
            DelegatingHandlersFactories = properOptions.DelegatingHandlersFactories;
        }

        private Func<Uri> _baseUriFactory;
        /// <inheritdoc />
        public Func<Uri> BaseUriFactory
        {
            get => _baseUriFactory;
            set => _baseUriFactory = value != null ? () => BaseUri = value.Invoke() : null;
        }

        private Func<string> _baseAddressFactory;
        /// <inheritdoc />
        public Func<string> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = value != null ? () => BaseAddress = value.Invoke() : null;
        }

        private Func<string> _basePathFactory;
        /// <inheritdoc />
        public Func<string> BasePathFactory
        {
            get => _basePathFactory;
            set => _basePathFactory = value != null ? () => BasePath = value.Invoke() : null;
        }

        private Func<HttpTracerMode> _httpTracerModeFactory;
        /// <inheritdoc />
        public Func<HttpTracerMode> HttpTracerModeFactory
        {
            get => _httpTracerModeFactory;
            set => _httpTracerModeFactory = () => HttpTracerMode = value.Invoke();
        }

        private Func<HttpMessageParts> _trafficVerbosityFactory;
        /// <inheritdoc />
        public Func<HttpMessageParts> TrafficVerbosityFactory
        {
            get => _trafficVerbosityFactory;
            set => _trafficVerbosityFactory = () => TrafficVerbosity = value.Invoke();
        }
        
        private Func<LogLevel[]> _logLevelsFactory;
        /// <inheritdoc />
        public Func<LogLevel[]> LogLevelsFactory
        {
            get => _logLevelsFactory;
            set => _logLevelsFactory = () => LogLevels = value.Invoke();
        }

        /// <inheritdoc />
        public Func<ILoggerFactory> LoggerFactoryFactory { get; set; }

        /// <inheritdoc />
        public Func<ILoggerFactory, string, ILogger> LoggerFactory { get; }

        /// <inheritdoc />
        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }

        /// <inheritdoc />
        public Func<HttpMessageHandler, Uri, HttpClient> HttpClientFactory { get; set; }

        /// <inheritdoc />
        public Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; set;  }

        private Func<RefitSettings> _refitSettingsFactory;
        /// <inheritdoc />
        public Func<RefitSettings> RefitSettingsFactory
        {
            get => _refitSettingsFactory;
            set => _refitSettingsFactory = () => RefitSettings = value.Invoke();
        }

        /// <inheritdoc />
        public Func<IConnectivityHandler> ConnectivityHandlerFactory { get; set; }

        /// <inheritdoc />
        public Func<ICacheHandler> CacheHandlerFactory { get; set; }

        /// <inheritdoc />
        public Func<IMappingHandler> MappingHandlerFactory { get; set; }

        /// <inheritdoc />
        public IList<Func<ILogger, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
    
    /// <inheritdoc cref="IApizrOptions{TWebApi}"/>
    public class ApizrOptions<TWebApi> : IApizrOptions<TWebApi>
    {
        /// <summary>
        /// The options
        /// </summary>
        protected readonly IApizrOptionsBase Options;

        /// <summary>
        /// The options constructor
        /// </summary>
        /// <param name="apizrOptions">The base options</param>
        public ApizrOptions(IApizrOptionsBase apizrOptions)
        {
            Options = apizrOptions;
        }

        /// <inheritdoc />
        public Type WebApiType => Options.WebApiType;

        /// <inheritdoc />
        public Uri BaseUri => Options.BaseUri;

        /// <inheritdoc />
        public string BaseAddress => Options.BaseAddress;

        /// <inheritdoc />
        public string BasePath => Options.BasePath;

        /// <inheritdoc />
        public HttpTracerMode HttpTracerMode => Options.HttpTracerMode;

        /// <inheritdoc />
        public HttpMessageParts TrafficVerbosity => Options.TrafficVerbosity;

        /// <inheritdoc />
        public LogLevel[] LogLevels => Options.LogLevels;

        /// <inheritdoc />
        public ILogger Logger => Options.Logger;

        /// <inheritdoc />
        public string[] PolicyRegistryKeys => Options.PolicyRegistryKeys;

        /// <inheritdoc />
        public RefitSettings RefitSettings => Options.RefitSettings;
    }
}