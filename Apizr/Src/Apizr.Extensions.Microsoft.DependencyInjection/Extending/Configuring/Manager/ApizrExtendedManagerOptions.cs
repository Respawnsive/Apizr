using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Extending.Configuring.Shared;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Requesting;
using Apizr.Resiliencing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Extending.Configuring.Manager
{
    /// <inheritdoc cref="IApizrExtendedManagerOptions"/>
    public class ApizrExtendedManagerOptions : ApizrExtendedManagerOptionsBase, IApizrExtendedManagerOptions
    {
        /// <summary>
        /// The options constructor
        /// </summary>
        /// <param name="commonOptions">The common options</param>
        /// <param name="properOptions">The proper options</param>
        public ApizrExtendedManagerOptions(IApizrExtendedCommonOptions commonOptions, IApizrExtendedProperOptions properOptions) : base(commonOptions, properOptions)
        {
            ApizrManagerType = properOptions.ApizrManagerType;
            BaseUriFactory = properOptions.BaseUriFactory;
            BaseAddressFactory = properOptions.BaseAddressFactory;
            BasePathFactory = properOptions.BasePathFactory;
            HttpTracerModeFactory = properOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = properOptions.TrafficVerbosityFactory;
            LogLevelsFactory = properOptions.LogLevelsFactory;
            LoggerFactory = properOptions.LoggerFactory;
            HttpClientHandlerFactory = properOptions.HttpClientHandlerFactory;
            HttpClientBuilder = properOptions.HttpClientBuilder;
            RefitSettingsFactory = commonOptions.RefitSettingsFactory;
            ConnectivityHandlerType = commonOptions.ConnectivityHandlerType;
            ConnectivityHandlerFactory = commonOptions.ConnectivityHandlerFactory;
            CacheHandlerType = commonOptions.CacheHandlerType;
            CacheHandlerFactory = commonOptions.CacheHandlerFactory;
            MappingHandlerType = commonOptions.MappingHandlerType;
            MappingHandlerFactory = commonOptions.MappingHandlerFactory;
            DelegatingHandlersExtendedFactories = properOptions.DelegatingHandlersExtendedFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            HttpMessageHandlerFactory = properOptions.HttpMessageHandlerFactory;
            CrudEntities = commonOptions.CrudEntities.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            WebApis = commonOptions.WebApis.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            ObjectMappings = commonOptions.ObjectMappings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            PostRegistries = commonOptions.PostRegistries.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            PostRegistrationActions = commonOptions.PostRegistrationActions.ToList();
            OperationTimeoutFactory = properOptions.OperationTimeoutFactory;
            RequestTimeoutFactory = properOptions.RequestTimeoutFactory;
            HeadersExtendedFactories = properOptions.HeadersExtendedFactories?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];
            _resiliencePropertiesExtendedFactories = properOptions.ResiliencePropertiesExtendedFactories?.ToDictionary(kpv => kpv.Key, kpv => kpv.Value) ??
                                                     new Dictionary<string, Func<IServiceProvider, object>>();
        }

        /// <inheritdoc />
        public Type ApizrManagerType { get; }

        /// <inheritdoc />
        public Type ConnectivityHandlerType { get; set; }

        /// <inheritdoc />
        public Type CacheHandlerType { get; set; }

        /// <inheritdoc />
        public Type MappingHandlerType { get; set; }

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

        private Func<IServiceProvider, HttpClientHandler> _httpClientHandlerFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory
        {
            get => _httpClientHandlerFactory;
            set => _httpClientHandlerFactory = serviceProvider => HttpClientHandler = value.Invoke(serviceProvider);
        }

        /// <inheritdoc />
        public Func<IServiceProvider, IApizrManagerOptionsBase, HttpMessageHandler> HttpMessageHandlerFactory { get; set; }

        private Func<IServiceProvider, RefitSettings> _refitSettingsFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, RefitSettings> RefitSettingsFactory
        {
            get => _refitSettingsFactory;
            set => _refitSettingsFactory = serviceProvider => RefitSettings = value.Invoke(serviceProvider);
        }

        /// <inheritdoc />
        public Func<IServiceProvider, IConnectivityHandler> ConnectivityHandlerFactory { get; set; }

        /// <inheritdoc />
        public Func<IServiceProvider, ICacheHandler> CacheHandlerFactory { get; set; }

        /// <inheritdoc />
        public Func<IServiceProvider, IMappingHandler> MappingHandlerFactory { get; set; }

        /// <inheritdoc />
        public Action<IHttpClientBuilder> HttpClientBuilder { get; set; }
        
        /// <inheritdoc />
        public IDictionary<(ApizrRegistrationMode, ApizrLifetimeScope), Func<IServiceProvider, Func<IList<string>>>> HeadersExtendedFactories { get; }

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
        public IDictionary<Type, CrudEntityAttribute> CrudEntities { get; }

        /// <inheritdoc />
        public IDictionary<Type, WebApiAttribute> WebApis { get; }

        /// <inheritdoc />
        public IDictionary<Type, MappedWithAttribute> ObjectMappings { get; }

        /// <inheritdoc />
        public IDictionary<Type, IApizrExtendedConcurrentRegistryBase> PostRegistries { get; }

        /// <inheritdoc />
        public IList<Action<Type, IServiceCollection>> PostRegistrationActions { get; }

        private readonly IDictionary<string, Func<IServiceProvider, object>> _resiliencePropertiesExtendedFactories;
        /// <inheritdoc />
        IDictionary<string, Func<IServiceProvider, object>> IApizrExtendedSharedOptions.ResiliencePropertiesExtendedFactories => _resiliencePropertiesExtendedFactories;
    }

    /// <inheritdoc cref="IApizrExtendedManagerOptionsBase"/>
    public class ApizrExtendedManagerOptions<TWebApi> : ApizrManagerOptions<TWebApi>, IApizrExtendedManagerOptionsBase
    {
        private readonly IApizrExtendedManagerOptionsBase _apizrExtendedOptions;

        public ApizrExtendedManagerOptions(IApizrExtendedManagerOptionsBase apizrOptions) : base(apizrOptions)
        {
            _apizrExtendedOptions = apizrOptions;
        }

        /// <inheritdoc />
        public HttpClientHandler HttpClientHandler => _apizrExtendedOptions.HttpClientHandler;

        /// <inheritdoc />

        public IDictionary<Type, Func<IServiceProvider, IApizrManagerOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories => _apizrExtendedOptions.DelegatingHandlersExtendedFactories;
    }
}
