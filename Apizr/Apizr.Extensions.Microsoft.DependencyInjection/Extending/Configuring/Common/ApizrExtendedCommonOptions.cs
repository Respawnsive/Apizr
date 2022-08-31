using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Common;
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
    public class ApizrExtendedCommonOptions : ApizrCommonOptionsBase, IApizrExtendedCommonOptions
    {
        public ApizrExtendedCommonOptions()
        {
            HttpTracerModeFactory = _ => HttpTracerMode.Everything;
            TrafficVerbosityFactory = _ => HttpMessageParts.All;
            LogLevelsFactory = _ => new []{ Constants.LowLogLevel, Constants.MediumLogLevel, Constants.HighLogLevel };
            HttpClientHandlerFactory = _ => new HttpClientHandler();
            RefitSettingsFactory = _ => new RefitSettings();
            ConnectivityHandlerType = typeof(DefaultConnectivityHandler);
            CacheHandlerType = typeof(VoidCacheHandler);
            MappingHandlerType = typeof(VoidMappingHandler);
            DelegatingHandlersExtendedFactories = new List<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>>();
            CrudEntities = new Dictionary<Type, CrudEntityAttribute>();
            WebApis = new Dictionary<Type, WebApiAttribute>();
            ObjectMappings = new Dictionary<Type, MappedWithAttribute>();
            PostRegistries = new Dictionary<Type, IApizrExtendedConcurrentRegistryBase>();
            PostRegistrationActions = new List<Action<Type, IServiceCollection>>();
        }
        public Type ConnectivityHandlerType { get; set; }
        public Type CacheHandlerType { get; set; }
        public Type MappingHandlerType { get; set; }

        private Func<IServiceProvider, Uri> _baseUriFactory;
        public Func<IServiceProvider, Uri> BaseUriFactory
        {
            get => _baseUriFactory;
            set => _baseUriFactory = serviceProvider => BaseUri = value.Invoke(serviceProvider);
        }

        private Func<IServiceProvider, string> _baseAddressFactory;
        public Func<IServiceProvider, string> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = serviceProvider => BaseAddress = value.Invoke(serviceProvider);
        }

        private Func<IServiceProvider, string> _basePathFactory;
        public Func<IServiceProvider, string> BasePathFactory
        {
            get => _basePathFactory;
            set => _basePathFactory = serviceProvider => BasePath = value.Invoke(serviceProvider);
        }

        private Func<IServiceProvider, HttpTracerMode> _httpTracerModeFactory;
        public Func<IServiceProvider, HttpTracerMode> HttpTracerModeFactory
        {
            get => _httpTracerModeFactory;
            set => _httpTracerModeFactory = serviceProvider => HttpTracerMode = value.Invoke(serviceProvider);
        }

        private Func<IServiceProvider, HttpMessageParts> _trafficVerbosityFactory;
        public Func<IServiceProvider, HttpMessageParts> TrafficVerbosityFactory
        {
            get => _trafficVerbosityFactory;
            set => _trafficVerbosityFactory = serviceProvider => TrafficVerbosity = value.Invoke(serviceProvider);
        }

        private Func<IServiceProvider, LogLevel[]> _logLevelsFactory;
        public Func<IServiceProvider, LogLevel[]> LogLevelsFactory
        {
            get => _logLevelsFactory;
            set => _logLevelsFactory = serviceProvider => LogLevels = value.Invoke(serviceProvider);
        }

        public Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory { get; set; }

        private Func<IServiceProvider, RefitSettings> _refitSettingsFactory;
        public Func<IServiceProvider, RefitSettings> RefitSettingsFactory
        {
            get => _refitSettingsFactory;
            set => _refitSettingsFactory = serviceProvider => RefitSettings = value.Invoke(serviceProvider);
        }

        public Func<IServiceProvider, IConnectivityHandler> ConnectivityHandlerFactory { get; set; }
        public Func<IServiceProvider, ICacheHandler> CacheHandlerFactory { get; set; }
        public Func<IServiceProvider, IMappingHandler> MappingHandlerFactory { get; set; }
        public Action<IHttpClientBuilder> HttpClientBuilder { get; set; }
        public IList<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
        public IDictionary<Type, CrudEntityAttribute> CrudEntities { get; }
        public IDictionary<Type, WebApiAttribute> WebApis { get; }
        public IDictionary<Type, MappedWithAttribute> ObjectMappings { get; }
        public IDictionary<Type, IApizrExtendedConcurrentRegistryBase> PostRegistries { get; }
        public IList<Action<Type, IServiceCollection>> PostRegistrationActions { get; }
    }
}
