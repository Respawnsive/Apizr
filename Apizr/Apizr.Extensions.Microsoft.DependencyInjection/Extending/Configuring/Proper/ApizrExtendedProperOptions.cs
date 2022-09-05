using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Apizr.Configuring;
using Apizr.Configuring.Proper;
using Apizr.Extending.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending.Configuring.Proper
{
    public class ApizrExtendedProperOptions : ApizrProperOptionsBase, IApizrExtendedProperOptions
    {
        public ApizrExtendedProperOptions(IApizrExtendedSharedOptions sharedOptions,
            Type webApiType, Type apizrManagerType,
            string[] assemblyPolicyRegistryKeys, 
            string[] webApiPolicyRegistryKeys,
            string baseAddress,
            string basePath,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            params LogLevel[] logLevels) : base(sharedOptions, 
            webApiType, 
            assemblyPolicyRegistryKeys, 
            webApiPolicyRegistryKeys)
        {
            ApizrManagerType = apizrManagerType;
            BaseUriFactory = !string.IsNullOrWhiteSpace(baseAddress) ? null : sharedOptions.BaseUriFactory;
            BaseAddressFactory = !string.IsNullOrWhiteSpace(baseAddress) ? _ => baseAddress : sharedOptions.BaseAddressFactory;
            BasePathFactory = !string.IsNullOrWhiteSpace(basePath) ? _ => basePath : sharedOptions.BasePathFactory;
            HttpTracerModeFactory = httpTracerMode.HasValue ? _ => httpTracerMode.Value : sharedOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = trafficVerbosity.HasValue ? _ => trafficVerbosity.Value : sharedOptions.TrafficVerbosityFactory;
            LogLevelsFactory = logLevels?.Any() == true ? _ => logLevels : sharedOptions.LogLevelsFactory;
            HttpClientHandlerFactory = sharedOptions.HttpClientHandlerFactory;
            LoggerFactory = (serviceProvider, webApiFriendlyName) => serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(webApiFriendlyName);
            DelegatingHandlersExtendedFactories = sharedOptions.DelegatingHandlersExtendedFactories;
        }

        public Type ApizrManagerType { get; }

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

        public Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory { get; set; }
        public Action<IHttpClientBuilder> HttpClientBuilder { get; set; }
        public IList<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
    }
}
