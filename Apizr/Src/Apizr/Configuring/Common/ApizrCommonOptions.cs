using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Shared;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Refit;

namespace Apizr.Configuring.Common
{
    /// <inheritdoc cref="IApizrCommonOptions"/>
    public class ApizrCommonOptions : ApizrCommonOptionsBase, IApizrCommonOptions
    {
        /// <summary>
        /// The Apizr common options constructor
        /// </summary>
        public ApizrCommonOptions(IApizrCommonOptions baseCommonOptions = null) : base(baseCommonOptions)
        {
            LoggerFactoryFactory = baseCommonOptions?.LoggerFactoryFactory ?? (() => new DebugLoggerFactory(Constants.LowLogLevel));
            ResiliencePipelineRegistryFactory = baseCommonOptions?.ResiliencePipelineRegistryFactory ?? (() => new ResiliencePipelineRegistry<string>());
            HttpClientHandlerFactory = baseCommonOptions?.HttpClientHandlerFactory ?? (() => new HttpClientHandler());
            HttpClientConfigurationBuilder = baseCommonOptions?.HttpClientConfigurationBuilder ?? (_ => { });
            RefitSettingsFactory = baseCommonOptions?.RefitSettingsFactory ?? (() => new RefitSettings());
            ConnectivityHandlerFactory = baseCommonOptions?.ConnectivityHandlerFactory ?? (() => new DefaultConnectivityHandler(() => true));
            CacheHandlerFactory = baseCommonOptions?.CacheHandlerFactory ?? (() => new VoidCacheHandler());
            MappingHandlerFactory = baseCommonOptions?.MappingHandlerFactory ?? (() => new VoidMappingHandler());
            DelegatingHandlersFactories =
                baseCommonOptions?.DelegatingHandlersFactories?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                new Dictionary<Type, Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>>();
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

        /// <inheritdoc />
        public Func<ILoggerFactory> LoggerFactoryFactory { get; set; }

        /// <inheritdoc />
        public Func<ResiliencePipelineRegistry<string>> ResiliencePipelineRegistryFactory { get; set; }

        /// <inheritdoc />
        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }

        /// <inheritdoc />
        public Action<HttpClient> HttpClientConfigurationBuilder { get; set; }

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
        public IDictionary<Type, Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }

        /// <inheritdoc />
        public Func<ILogger, IApizrManagerOptionsBase, HttpMessageHandler> HttpMessageHandlerFactory { get; set; }

        private Func<TimeSpan> _operationTimeoutFactory;
        /// <inheritdoc />
        public Func<TimeSpan> OperationTimeoutFactory
        {
            get => _operationTimeoutFactory;
            set => _operationTimeoutFactory = value != null ? () => (TimeSpan)(OperationTimeout = value.Invoke()) : null;
        }

        private Func<TimeSpan> _requestTimeoutFactory;
        /// <inheritdoc />
        public Func<TimeSpan> RequestTimeoutFactory
        {
            get => _requestTimeoutFactory;
            set => _requestTimeoutFactory = value != null ? () => (TimeSpan)(RequestTimeout = value.Invoke()) : null;
        }
        
        private Func<HttpTracerMode> _httpTracerModeFactory;
        /// <inheritdoc />
        public Func<HttpTracerMode> HttpTracerModeFactory
        {
            get => _httpTracerModeFactory;
            set => _httpTracerModeFactory = value != null ? () => HttpTracerMode = value.Invoke() : null;
        }

        private Func<HttpMessageParts> _trafficVerbosityFactory;
        /// <inheritdoc />
        public Func<HttpMessageParts> TrafficVerbosityFactory
        {
            get => _trafficVerbosityFactory;
            set => _trafficVerbosityFactory = value != null ? () => TrafficVerbosity = value.Invoke() : null;
        }

        private Func<LogLevel[]> _logLevelsFactory;
        /// <inheritdoc />
        public Func<LogLevel[]> LogLevelsFactory
        {
            get => _logLevelsFactory;
            set => _logLevelsFactory = value != null ? () => LogLevels = value.Invoke() : null;
        }

        private Func<IList<IApizrExceptionHandler>> _exceptionHandlersFactory;
        /// <inheritdoc />
        public Func<IList<IApizrExceptionHandler>> ExceptionHandlersFactory
        {
            get => _exceptionHandlersFactory;
            set => _exceptionHandlersFactory = value != null ? () => ExceptionHandlers = value.Invoke() : null;
        }
    }
}
