using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Requesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Extending.Configuring
{
    public class ApizrExtendedOptions : ApizrExtendedOptionsBase, IApizrExtendedOptions
    {
        public ApizrExtendedOptions(IApizrExtendedCommonOptions commonOptions, IApizrExtendedProperOptions properOptions) : base(commonOptions, properOptions)
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
            RefitSettingsFactory = commonOptions.RefitSettingsFactory;
            ConnectivityHandlerType = commonOptions.ConnectivityHandlerType;
            ConnectivityHandlerFactory = commonOptions.ConnectivityHandlerFactory;
            CacheHandlerType = commonOptions.CacheHandlerType;
            CacheHandlerFactory = commonOptions.CacheHandlerFactory;
            MappingHandlerType = commonOptions.MappingHandlerType;
            MappingHandlerFactory = commonOptions.MappingHandlerFactory;
            DelegatingHandlersExtendedFactories = properOptions.DelegatingHandlersExtendedFactories;
            CrudEntities = commonOptions.CrudEntities;
            WebApis = commonOptions.WebApis;
            ObjectMappings = commonOptions.ObjectMappings;
            PostRegistries = commonOptions.PostRegistries;
            PostRegistrationActions = commonOptions.PostRegistrationActions;
        }

        public Type ApizrManagerType { get; }
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

        private Func<IServiceProvider, string, ILogger> _loggerFactory;
        public Func<IServiceProvider, string, ILogger> LoggerFactory
        {
            get => _loggerFactory;
            protected set => _loggerFactory = (serviceProvider, webApiFriendlyName) => Logger = value.Invoke(serviceProvider, webApiFriendlyName);
        }

        private Func<IServiceProvider, HttpClientHandler> _httpClientHandlerFactory;
        public Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory
        {
            get => _httpClientHandlerFactory;
            set => _httpClientHandlerFactory = serviceProvider => HttpClientHandler = value.Invoke(serviceProvider);
        }


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
        public IDictionary<Type, CrudEntityAttribute> CrudEntities { get; }
        public IDictionary<Type, WebApiAttribute> WebApis { get; }
        public IDictionary<Type, MappedWithAttribute> ObjectMappings { get; }
        public IDictionary<Type, IApizrExtendedConcurrentRegistryBase> PostRegistries { get; }
        public IList<Action<Type, IServiceCollection>> PostRegistrationActions { get; }
    }

    public class ApizrExtendedOptions<TWebApi> : ApizrOptions<TWebApi>, IApizrExtendedOptionsBase
    {
        private readonly IApizrExtendedOptionsBase _apizrExtendedOptions;
        public ApizrExtendedOptions(IApizrExtendedOptionsBase apizrOptions) : base(apizrOptions)
        {
            _apizrExtendedOptions = apizrOptions;
        }

        public HttpClientHandler HttpClientHandler => _apizrExtendedOptions.HttpClientHandler;

        public IList<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>>
            DelegatingHandlersExtendedFactories => _apizrExtendedOptions.DelegatingHandlersExtendedFactories;
    }
}
