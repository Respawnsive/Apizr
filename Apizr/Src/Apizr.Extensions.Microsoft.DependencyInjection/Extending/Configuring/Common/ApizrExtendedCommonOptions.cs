using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Manager;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Manager;
using Apizr.Extending.Configuring.Registry;
using Apizr.Extending.Configuring.Shared;
using Apizr.Logging;
using Apizr.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Extending.Configuring.Common
{
    /// <inheritdoc cref="IApizrExtendedCommonOptions"/>
    public class ApizrExtendedCommonOptions : ApizrCommonOptionsBase, IApizrExtendedCommonOptions
    {
        public ApizrExtendedCommonOptions(IApizrExtendedCommonOptions baseCommonOptions = null) : base(
            baseCommonOptions)
        {
            HttpClientHandlerFactory = baseCommonOptions?.HttpClientHandlerFactory ?? (_ => new HttpClientHandler());
            HttpClientBuilder = baseCommonOptions?.HttpClientBuilder ?? (_ => { });
            RefitSettingsFactory = baseCommonOptions?.RefitSettingsFactory ?? (_ => new RefitSettings());
            ConnectivityHandlerType = baseCommonOptions?.ConnectivityHandlerType ?? typeof(DefaultConnectivityHandler);
            CacheHandlerType = baseCommonOptions?.CacheHandlerType ?? typeof(VoidCacheHandler);
            MappingHandlerType = baseCommonOptions?.MappingHandlerType ?? typeof(VoidMappingHandler);
            DelegatingHandlersExtendedFactories =
                baseCommonOptions?.DelegatingHandlersExtendedFactories?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                new Dictionary<Type, Func<IServiceProvider, IApizrManagerOptionsBase, DelegatingHandler>>();
            WebApis = baseCommonOptions?.WebApis?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                      new Dictionary<Type, BaseAddressAttribute>();
            ObjectMappings = baseCommonOptions?.ObjectMappings?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                             new Dictionary<Assembly, MappedWithAttribute[]>();
            PostRegistries = baseCommonOptions?.PostRegistries?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                             new Dictionary<Type, IApizrExtendedConcurrentRegistryBase>();
            PostRegistrationActions = baseCommonOptions?.PostRegistrationActions?.ToList() ?? [];
            HeadersExtendedFactories =
                baseCommonOptions?.HeadersExtendedFactories?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                new Dictionary<(ApizrRegistrationMode, ApizrLifetimeScope), Func<IServiceProvider, Func<IList<string>>>>();
            _resiliencePropertiesExtendedFactories = new Dictionary<string, Func<IServiceProvider, object>>();
        }

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
            set => _httpTracerModeFactory = value != null ? serviceProvider => HttpTracerMode = value.Invoke(serviceProvider) : null;
        }

        private Func<IServiceProvider, HttpMessageParts> _trafficVerbosityFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, HttpMessageParts> TrafficVerbosityFactory
        {
            get => _trafficVerbosityFactory;
            set => _trafficVerbosityFactory = value != null ? serviceProvider => TrafficVerbosity = value.Invoke(serviceProvider) : null;
        }

        private Func<IServiceProvider, LogLevel[]> _logLevelsFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, LogLevel[]> LogLevelsFactory
        {
            get => _logLevelsFactory;
            set => _logLevelsFactory = value != null ? serviceProvider => LogLevels = value.Invoke(serviceProvider) : null;
        }

        /// <inheritdoc />
        public Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory { get; set; }

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
            set => _operationTimeoutFactory = value != null ? serviceProvider => (TimeSpan) (OperationTimeout = value.Invoke(serviceProvider)) : null;
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

        /// <inheritdoc />
        public IDictionary<Type, BaseAddressAttribute> WebApis { get; }

        /// <inheritdoc />
        public IDictionary<Assembly, MappedWithAttribute[]> ObjectMappings { get; }

        /// <inheritdoc />
        public IDictionary<Type, IApizrExtendedConcurrentRegistryBase> PostRegistries { get; }

        /// <inheritdoc />
        public IList<Action<IApizrExtendedManagerOptions, IServiceCollection>> PostRegistrationActions { get; }

        private readonly IDictionary<string, Func<IServiceProvider, object>> _resiliencePropertiesExtendedFactories;
        /// <inheritdoc />
        IDictionary<string, Func<IServiceProvider, object>> IApizrExtendedSharedOptions.ResiliencePropertiesExtendedFactories => _resiliencePropertiesExtendedFactories;
    }
}
