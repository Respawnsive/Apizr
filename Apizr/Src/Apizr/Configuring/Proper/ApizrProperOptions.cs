using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Apizr.Resiliencing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Apizr.Configuring.Proper
{
    /// <inheritdoc cref="IApizrProperOptions"/>
    public class ApizrProperOptions : ApizrProperOptionsBase, IApizrProperOptions
    {
        /// <summary>
        /// The proper options constructor
        /// </summary>
        /// <param name="sharedOptions">The shared options</param>
        /// <param name="webApiType">The web api type</param>
        /// <param name="commonResiliencePipelineRegistryKeys">Global resilience pipelines</param>
        /// <param name="properResiliencePipelineRegistryKeys">Specific resilience pipeline</param>
        /// <param name="baseAddress">The web api base address</param>
        /// <param name="basePath">The web api base path</param>
        /// <param name="handlersParameters">Some handlers parameters</param>
        /// <param name="httpTracerMode">The http tracer mode</param>
        /// <param name="trafficVerbosity">The traffic verbosity</param>
        /// <param name="operationTimeout">The operation timeout</param>
        /// <param name="requestTimeout">The request timeout</param>
        /// <param name="commonCacheAttribute">Global caching options</param>
        /// <param name="properCacheAttribute">Specific caching options</param>
        /// <param name="shouldRedactHeaderValue">Headers to redact value</param>
        /// <param name="logLevels">The log levels</param>
        public ApizrProperOptions(IApizrSharedRegistrationOptions sharedOptions,
            Type webApiType,
            string[] commonResiliencePipelineRegistryKeys,
            string[] properResiliencePipelineRegistryKeys, 
            string baseAddress,
            string basePath,
            IDictionary<string, object> handlersParameters,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            TimeSpan? operationTimeout,
            TimeSpan? requestTimeout,
            CacheAttribute commonCacheAttribute,
            CacheAttribute properCacheAttribute,
            Func<string, bool> shouldRedactHeaderValue = null,
            params LogLevel[] logLevels) : base(sharedOptions, webApiType, commonResiliencePipelineRegistryKeys,
            properResiliencePipelineRegistryKeys, commonCacheAttribute, properCacheAttribute, shouldRedactHeaderValue)
        {
            BaseUriFactory = !string.IsNullOrWhiteSpace(baseAddress) ? null : sharedOptions.BaseUriFactory;
            BaseAddressFactory = !string.IsNullOrWhiteSpace(baseAddress) ? () => baseAddress : sharedOptions.BaseAddressFactory;
            BasePathFactory = !string.IsNullOrWhiteSpace(basePath) ? () => basePath : sharedOptions.BasePathFactory;
            HandlersParameters = handlersParameters;
            HttpTracerModeFactory = httpTracerMode.HasValue ? () => httpTracerMode.Value : sharedOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = trafficVerbosity.HasValue ? () => trafficVerbosity.Value : sharedOptions.TrafficVerbosityFactory;
            LogLevelsFactory = logLevels?.Any() == true ? () => logLevels : sharedOptions.LogLevelsFactory;
            LoggerFactory = (loggerFactory, webApiFriendlyName) => Logger = loggerFactory.CreateLogger(webApiFriendlyName);
            HttpClientHandlerFactory = sharedOptions.HttpClientHandlerFactory;
            HttpClientConfigurationBuilder = sharedOptions.HttpClientConfigurationBuilder;
            DelegatingHandlersFactories = sharedOptions.DelegatingHandlersFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            HttpMessageHandlerFactory = sharedOptions.HttpMessageHandlerFactory;
            OperationTimeoutFactory = operationTimeout.HasValue ? () => operationTimeout!.Value : sharedOptions.OperationTimeoutFactory;
            RequestTimeoutFactory = requestTimeout.HasValue ? () => requestTimeout!.Value : sharedOptions.RequestTimeoutFactory;
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
        public Func<ILoggerFactory, string, ILogger> LoggerFactory { get; }

        /// <inheritdoc />
        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }

        /// <inheritdoc />
        public Action<HttpClient> HttpClientConfigurationBuilder { get; set;  }

        /// <inheritdoc />
        public IDictionary<Type, Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }

        /// <inheritdoc />
        public Func<ILogger, IApizrManagerOptionsBase, HttpMessageHandler> HttpMessageHandlerFactory { get; set; }

        private Func<TimeSpan> _operationTimeoutFactory;
        /// <inheritdoc />
        public Func<TimeSpan> OperationTimeoutFactory
        {
            get => _operationTimeoutFactory;
            set => _operationTimeoutFactory = value != null ? () => (TimeSpan) (OperationTimeout = value.Invoke()) : null;
        }

        private Func<TimeSpan> _requestTimeoutFactory;
        /// <inheritdoc />
        public Func<TimeSpan> RequestTimeoutFactory
        {
            get => _requestTimeoutFactory;
            set => _requestTimeoutFactory = value != null ? () => (TimeSpan)(RequestTimeout = value.Invoke()) : null;
        }
    }
}
