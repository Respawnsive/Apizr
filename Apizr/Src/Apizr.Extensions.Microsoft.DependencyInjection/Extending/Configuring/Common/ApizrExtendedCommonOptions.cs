using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Manager;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Requesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Extending.Configuring.Common
{
    /// <inheritdoc cref="IApizrExtendedCommonOptions"/>
    public class ApizrExtendedCommonOptions : ApizrCommonOptionsBase, IApizrExtendedCommonOptions
    {
        public ApizrExtendedCommonOptions()
        {
            HttpTracerModeFactory = _ => HttpTracerMode.Everything;
            TrafficVerbosityFactory = _ => HttpMessageParts.All;
            LogLevelsFactory = _ => new []{ Constants.LowLogLevel, Constants.MediumLogLevel, Constants.HighLogLevel };
            HttpClientHandlerFactory = _ => new HttpClientHandler();
            HttpClientBuilder = _ => { };
            RefitSettingsFactory = _ => new RefitSettings();
            ConnectivityHandlerType = typeof(DefaultConnectivityHandler);
            CacheHandlerType = typeof(VoidCacheHandler);
            MappingHandlerType = typeof(VoidMappingHandler);
            DelegatingHandlersExtendedFactories = new Dictionary<Type, Func<IServiceProvider, IApizrManagerOptionsBase, DelegatingHandler>>();
            CrudEntities = new Dictionary<Type, CrudEntityAttribute>();
            WebApis = new Dictionary<Type, WebApiAttribute>();
            ObjectMappings = new Dictionary<Type, MappedWithAttribute>();
            PostRegistries = new Dictionary<Type, IApizrExtendedConcurrentRegistryBase>();
            PostRegistrationActions = new List<Action<Type, IServiceCollection>>();
            HeadersFactories = new List<Func<IServiceProvider, IList<string>>>();
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

        internal IList<Func<IServiceProvider, IList<string>>> HeadersFactories { get; }
        private Func<IServiceProvider, IList<string>> _headersFactory;
        /// <inheritdoc />
        public Func<IServiceProvider, IList<string>> HeadersFactory => _headersFactory ??= serviceProvider => Headers = HeadersFactories.SelectMany(factory => factory.Invoke(serviceProvider)).ToList();
        
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
        public IDictionary<Type, CrudEntityAttribute> CrudEntities { get; }

        /// <inheritdoc />
        public IDictionary<Type, WebApiAttribute> WebApis { get; }

        /// <inheritdoc />
        public IDictionary<Type, MappedWithAttribute> ObjectMappings { get; }

        /// <inheritdoc />
        public IDictionary<Type, IApizrExtendedConcurrentRegistryBase> PostRegistries { get; }

        /// <inheritdoc />
        public IList<Action<Type, IServiceCollection>> PostRegistrationActions { get; }
    }
}
