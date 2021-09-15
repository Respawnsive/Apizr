using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Requesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr
{
    public class ApizrExtendedOptions : ApizrOptionsBase, IApizrExtendedOptions
    {
        public ApizrExtendedOptions(Type webApiType, Type apizrManagerType, Uri baseAddress,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? httpTracerVerbosity,
            LogLevel? logLevel,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys) : base(webApiType, 
            assemblyPolicyRegistryKeys,
            webApiPolicyRegistryKeys)
        {
            ApizrManagerType = apizrManagerType;
            BaseAddressFactory = _ => baseAddress;
            HttpTracerModeFactory = _ => httpTracerMode ?? HttpTracerMode.Everything;
            TrafficVerbosityFactory = _ => httpTracerVerbosity ?? HttpMessageParts.None;
            LogLevelFactory = _ => logLevel ?? LogLevel.None;
            HttpClientHandlerFactory = _ => new HttpClientHandler();
            RefitSettingsFactory = _ => new RefitSettings();
            ConnectivityHandlerType = typeof(DefaultConnectivityHandler);
            CacheHandlerType = typeof(VoidCacheHandler);
            MappingHandlerType = typeof(VoidMappingHandler);
            DelegatingHandlersExtendedFactories = new List<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>>();
            CrudEntities = new Dictionary<Type, CrudEntityAttribute>();
            WebApis = new Dictionary<Type, WebApiAttribute>();
            ObjectMappings = new Dictionary<Type, MappedWithAttribute>();
            PostRegistrationActions = new List<Action<IServiceCollection>>();
        }

        public Type ApizrManagerType { get; }
        public Type ConnectivityHandlerType { get; set; }
        public Type CacheHandlerType { get; set; }
        public Type MappingHandlerType { get; set; }

        private Func<IServiceProvider, Uri> _baseAddressFactory;
        public Func<IServiceProvider, Uri> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = serviceProvider => BaseAddress = value.Invoke(serviceProvider);
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

        private Func<IServiceProvider, LogLevel> _logLevelFactory;
        public Func<IServiceProvider, LogLevel> LogLevelFactory
        {
            get => _logLevelFactory;
            set => _logLevelFactory = serviceProvider => LogLevel = value.Invoke(serviceProvider);
        }

        public Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory { get; set; }

        private Func<IServiceProvider, RefitSettings> _refitSettingsFactory;
        public Func<IServiceProvider, RefitSettings> RefitSettingsFactory
        {
            get => _refitSettingsFactory;
            set => _refitSettingsFactory = serviceProvider =>
            {
                var refitSettings = value.Invoke(serviceProvider);
                ContentSerializer = refitSettings.ContentSerializer;
                return refitSettings;
            };
        }

        public Func<IServiceProvider, IConnectivityHandler> ConnectivityHandlerFactory { get; set; }
        public Action<IHttpClientBuilder> HttpClientBuilder { get; set; }
        public IList<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
        public IDictionary<Type, CrudEntityAttribute> CrudEntities { get; }
        public IDictionary<Type, WebApiAttribute> WebApis { get; }
        public IDictionary<Type, MappedWithAttribute> ObjectMappings { get; }
        public IList<Action<IServiceCollection>> PostRegistrationActions { get; }
    }
}
