using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching.Attributes;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Proper;
using Apizr.Extending.Configuring.Shared;
using Apizr.Logging;
using Apizr.Resiliencing;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending.Configuring.Proper
{
    /// <inheritdoc cref="IApizrExtendedProperOptions"/>
    public class ApizrExtendedProperOptions : ApizrProperOptionsBase, IApizrExtendedProperOptions
    {
        /// <summary>
        /// The proper options constructor
        /// </summary>
        /// <param name="sharedOptions">The shared options</param>
        /// <param name="webApiType">The web api type</param>
        /// <param name="crudApiEntityType">The crud api entity type if any</param>
        /// <param name="crudApiEntityKeyType">The crud api entity key type if any</param>
        /// <param name="crudApiReadAllResultType">The crud api read all result type if any</param>
        /// <param name="crudApiReadAllParamsType">The crud api read all params type if any</param>
        /// <param name="typeInfo">The type info</param>
        /// <param name="apizrManagerType">The manager type</param>
        /// <param name="baseAddress">The web api base address</param>
        /// <param name="basePath">The web api base path</param>
        /// <param name="handlersParameters">Some handlers parameters</param>
        /// <param name="httpTracerMode">The http tracer mode</param>
        /// <param name="trafficVerbosity">The traffic verbosity</param>
        /// <param name="operationTimeout">The operation timeout</param>
        /// <param name="requestTimeout">The request timeout</param>
        /// <param name="commonResiliencePipelineAttributes">Global resilience pipelines</param>
        /// <param name="properResiliencePipelineAttributes">Specific resilience pipeline</param>
        /// <param name="commonCacheAttribute">Global caching options</param>
        /// <param name="properCacheAttribute">Specific caching options</param>
        /// <param name="shouldRedactHeaderValue">Headers to redact value</param>
        /// <param name="logLevels">The log levels</param>
        public ApizrExtendedProperOptions(IApizrExtendedSharedOptions sharedOptions,
            Type webApiType,
            Type crudApiEntityType,
            Type crudApiEntityKeyType,
            Type crudApiReadAllResultType,
            Type crudApiReadAllParamsType,
            TypeInfo typeInfo,
            Type apizrManagerType,
            string baseAddress,
            string basePath,
            IDictionary<string, object> handlersParameters,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            TimeSpan? operationTimeout,
            TimeSpan? requestTimeout,
            ResiliencePipelineAttributeBase[] commonResiliencePipelineAttributes,
            ResiliencePipelineAttributeBase[] properResiliencePipelineAttributes,
            CacheAttribute commonCacheAttribute,
            CacheAttribute properCacheAttribute,
            Func<string, bool> shouldRedactHeaderValue = null,
            params LogLevel[] logLevels) : base(sharedOptions, 
            webApiType, 
            crudApiEntityType, 
            typeInfo,
            commonResiliencePipelineAttributes,
            properResiliencePipelineAttributes,
            commonCacheAttribute, 
            properCacheAttribute, 
            shouldRedactHeaderValue)
        {
            CrudApiEntityKeyType = crudApiEntityKeyType;
            CrudApiReadAllResultType = crudApiReadAllResultType;
            CrudApiReadAllParamsType = crudApiReadAllParamsType;
            ApizrManagerImplementationType = apizrManagerType;
            BaseUriFactory = !string.IsNullOrWhiteSpace(baseAddress) ? null : sharedOptions.BaseUriFactory;
            BaseAddressFactory = !string.IsNullOrWhiteSpace(baseAddress) ? _ => baseAddress : sharedOptions.BaseAddressFactory;
            BasePathFactory = !string.IsNullOrWhiteSpace(basePath) ? _ => basePath : sharedOptions.BasePathFactory;
            HandlersParameters = handlersParameters;
            HttpTracerModeFactory = httpTracerMode.HasValue ? _ => httpTracerMode.Value : sharedOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = trafficVerbosity.HasValue ? _ => trafficVerbosity.Value : sharedOptions.TrafficVerbosityFactory;
            LogLevelsFactory = logLevels?.Length > 0 ? _ => logLevels : sharedOptions.LogLevelsFactory;
            HttpClientHandlerFactory = sharedOptions.HttpClientHandlerFactory;
            HttpClientBuilder = sharedOptions.HttpClientBuilder;
            LoggerFactory = (serviceProvider, webApiFriendlyName) => serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(webApiFriendlyName);
            DelegatingHandlersExtendedFactories = sharedOptions.DelegatingHandlersExtendedFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            HttpMessageHandlerFactory = sharedOptions.HttpMessageHandlerFactory;
            OperationTimeoutFactory = operationTimeout.HasValue ? _ => operationTimeout!.Value : sharedOptions.OperationTimeoutFactory;
            RequestTimeoutFactory = requestTimeout.HasValue ? _ => requestTimeout!.Value : sharedOptions.RequestTimeoutFactory;
            HeadersExtendedFactories = sharedOptions.HeadersExtendedFactories?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];
            ExceptionHandlersFactory = sharedOptions.ExceptionHandlersFactory;
            _resiliencePropertiesExtendedFactories = sharedOptions?.ResiliencePropertiesExtendedFactories?.ToDictionary(kpv => kpv.Key, kpv => kpv.Value) ?? [];
        }

        /// <inheritdoc />
        public Type CrudApiEntityKeyType { get; }

        /// <inheritdoc />
        public Type CrudApiReadAllResultType { get; }

        /// <inheritdoc />
        public Type CrudApiReadAllParamsType { get; }

        /// <inheritdoc />
        public Type ApizrManagerImplementationType { get; }


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

        /// <inheritdoc />
        public IDictionary<(ApizrRegistrationMode, ApizrLifetimeScope), Func<IServiceProvider, Func<IList<string>>>> HeadersExtendedFactories { get; }

        private Func<IServiceProvider, IList<IApizrExceptionHandler>> _exceptionHandlersFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, IList<IApizrExceptionHandler>> ExceptionHandlersFactory
        {
            get => _exceptionHandlersFactory;
            set => _exceptionHandlersFactory = value != null ? serviceProvider => ExceptionHandlers = value.Invoke(serviceProvider) : null;
        }

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

        /// <inheritdoc />
        public Func<IServiceProvider, IApizrManagerOptionsBase, HttpMessageHandler> HttpMessageHandlerFactory { get; set; }

        private readonly IDictionary<string, Func<IServiceProvider, object>> _resiliencePropertiesExtendedFactories;
        /// <inheritdoc />
        IDictionary<string, Func<IServiceProvider, object>> IApizrExtendedSharedOptions.ResiliencePropertiesExtendedFactories => _resiliencePropertiesExtendedFactories;
    }
}
