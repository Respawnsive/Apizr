using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Proper;
using Apizr.Extending.Configuring.Shared;
using Apizr.Logging;
using Apizr.Resiliencing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending.Configuring.Proper
{
    /// <inheritdoc cref="IApizrExtendedProperOptions"/>
    public class ApizrExtendedProperOptions : ApizrProperOptionsBase, IApizrExtendedProperOptions
    {
        public ApizrExtendedProperOptions(IApizrExtendedSharedOptions sharedOptions,
            Type webApiType, Type apizrManagerType,
            string[] assemblyResiliencePipelineRegistryKeys, 
            string[] webApiResiliencePipelineRegistryKeys,
            string baseAddress,
            string basePath,
            IDictionary<string, object> handlersParameters,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            TimeSpan? operationTimeout,
            TimeSpan? requestTimeout,
            params LogLevel[] logLevels) : base(sharedOptions, 
            webApiType, 
            assemblyResiliencePipelineRegistryKeys, 
            webApiResiliencePipelineRegistryKeys)
        {
            ApizrManagerType = apizrManagerType;
            BaseUriFactory = !string.IsNullOrWhiteSpace(baseAddress) ? null : sharedOptions.BaseUriFactory;
            BaseAddressFactory = !string.IsNullOrWhiteSpace(baseAddress) ? _ => baseAddress : sharedOptions.BaseAddressFactory;
            BasePathFactory = !string.IsNullOrWhiteSpace(basePath) ? _ => basePath : sharedOptions.BasePathFactory;
            HandlersParameters = handlersParameters;
            HttpTracerModeFactory = httpTracerMode.HasValue ? _ => httpTracerMode.Value : sharedOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = trafficVerbosity.HasValue ? _ => trafficVerbosity.Value : sharedOptions.TrafficVerbosityFactory;
            LogLevelsFactory = logLevels?.Any() == true ? _ => logLevels : sharedOptions.LogLevelsFactory;
            HttpClientHandlerFactory = sharedOptions.HttpClientHandlerFactory;
            HttpClientBuilder = sharedOptions.HttpClientBuilder;
            LoggerFactory = (serviceProvider, webApiFriendlyName) => serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(webApiFriendlyName);
            DelegatingHandlersExtendedFactories = sharedOptions.DelegatingHandlersExtendedFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            HeadersFactories = new List<Func<IServiceProvider, IList<string>>> { sharedOptions.HeadersFactory };
            OperationTimeoutFactory = operationTimeout.HasValue ? _ => operationTimeout!.Value : sharedOptions.OperationTimeoutFactory;
            RequestTimeoutFactory = requestTimeout.HasValue ? _ => requestTimeout!.Value : sharedOptions.RequestTimeoutFactory;
            _resiliencePropertiesExtendedFactories = sharedOptions?.ResiliencePropertiesExtendedFactories?.ToDictionary(kpv => kpv.Key, kpv => kpv.Value) ??
                                             new Dictionary<string, Func<IServiceProvider, object>>();
        }

        /// <inheritdoc />
        public Type ApizrManagerType { get; }

        private Func<IServiceProvider, Uri> _baseUriFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, Uri> BaseUriFactory
        {
            get => _baseUriFactory;
            set => _baseUriFactory = value != null ? serviceProvider => BaseUri = value.Invoke(serviceProvider) : null;
        }

        private Func<IServiceProvider, string> _baseAddressFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, string> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = value != null ? serviceProvider => BaseAddress = value.Invoke(serviceProvider) : null;
        }

        private Func<IServiceProvider, string> _basePathFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, string> BasePathFactory
        {
            get => _basePathFactory;
            set => _basePathFactory = value != null ? serviceProvider => BasePath = value.Invoke(serviceProvider) : null;
        }

        private Func<IServiceProvider, HttpTracerMode> _httpTracerModeFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, HttpTracerMode> HttpTracerModeFactory
        {
            get => _httpTracerModeFactory;
            set => _httpTracerModeFactory = serviceProvider => HttpTracerMode = value.Invoke(serviceProvider);
        }

        private Func<IServiceProvider, HttpMessageParts> _trafficVerbosityFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, HttpMessageParts> TrafficVerbosityFactory
        {
            get => _trafficVerbosityFactory;
            set => _trafficVerbosityFactory = serviceProvider => TrafficVerbosity = value.Invoke(serviceProvider);
        }

        private Func<IServiceProvider, LogLevel[]> _logLevelsFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, LogLevel[]> LogLevelsFactory
        {
            get => _logLevelsFactory;
            set => _logLevelsFactory = serviceProvider => LogLevels = value.Invoke(serviceProvider);
        }

        private Func<IServiceProvider, string, ILogger> _loggerFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, string, ILogger> LoggerFactory
        {
            get => _loggerFactory;
            protected set => _loggerFactory = (serviceProvider, webApiFriendlyName) => Logger = value.Invoke(serviceProvider, webApiFriendlyName);
        }

        /// <inheritdoc />
        public Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory { get; set; }

        /// <inheritdoc />
        public Action<IHttpClientBuilder> HttpClientBuilder { get; set; }

        internal IList<Func<IServiceProvider, IList<string>>> HeadersFactories { get; }
        private Func<IServiceProvider, IList<string>> _headersFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, IList<string>> HeadersFactory => _headersFactory ??= serviceProvider => Headers = HeadersFactories.SelectMany(factory => factory.Invoke(serviceProvider)).ToList();


        private Func<IServiceProvider, TimeSpan> _operationTimeoutFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, TimeSpan> OperationTimeoutFactory
        {
            get => _operationTimeoutFactory;
            set => _operationTimeoutFactory = value != null ? serviceProvider => (TimeSpan)(OperationTimeout = value.Invoke(serviceProvider)) : null;
        }

        private Func<IServiceProvider, TimeSpan> _requestTimeoutFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, TimeSpan> RequestTimeoutFactory
        {
            get => _requestTimeoutFactory;
            set => _requestTimeoutFactory = value != null ? serviceProvider => (TimeSpan)(RequestTimeout = value.Invoke(serviceProvider)) : null;
        }

        /// <inheritdoc />
        public IDictionary<Type, Func<IServiceProvider, IApizrManagerOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }

        private readonly IDictionary<string, Func<IServiceProvider, object>> _resiliencePropertiesExtendedFactories;
        /// <inheritdoc />
        IDictionary<string, Func<IServiceProvider, object>> IApizrExtendedSharedOptions.ResiliencePropertiesExtendedFactories => _resiliencePropertiesExtendedFactories;
    }
}
