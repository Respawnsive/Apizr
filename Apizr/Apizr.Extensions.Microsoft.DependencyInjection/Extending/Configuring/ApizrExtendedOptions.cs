using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Requesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Extending.Configuring
{
    public class ApizrExtendedOptions : ApizrOptionsBase, IApizrExtendedOptions
    {
        public ApizrExtendedOptions(IApizrExtendedCommonOptions commonOptions, IApizrExtendedProperOptions properOptions) : base(commonOptions, properOptions)
        {
            ApizrManagerType = properOptions.ApizrManagerType;
            BaseAddressFactory = properOptions.BaseAddressFactory;
            HttpTracerModeFactory = properOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = properOptions.TrafficVerbosityFactory;
            LogLevelFactory = properOptions.LogLevelFactory;
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
            PostRegistrationActions = commonOptions.PostRegistrationActions;
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
        public Func<IServiceProvider, ICacheHandler> CacheHandlerFactory { get; set; }
        public Func<IServiceProvider, IMappingHandler> MappingHandlerFactory { get; set; }
        public Action<IHttpClientBuilder> HttpClientBuilder { get; set; }
        public IList<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
        public IDictionary<Type, CrudEntityAttribute> CrudEntities { get; }
        public IDictionary<Type, WebApiAttribute> WebApis { get; }
        public IDictionary<Type, MappedWithAttribute> ObjectMappings { get; }
        public IList<Action<IServiceCollection>> PostRegistrationActions { get; }
    }
}
