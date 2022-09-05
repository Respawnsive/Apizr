using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Proper
{
    public class ApizrProperOptions : ApizrProperOptionsBase, IApizrProperOptions
    {
        public ApizrProperOptions(IApizrSharedOptions sharedOptions,
            Type webApiType,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys, 
            string baseAddress,
            string basePath,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            params LogLevel[] logLevels) : base(sharedOptions, webApiType, assemblyPolicyRegistryKeys,
            webApiPolicyRegistryKeys)
        {
            BaseUriFactory = !string.IsNullOrWhiteSpace(baseAddress) ? null : sharedOptions.BaseUriFactory;
            BaseAddressFactory = !string.IsNullOrWhiteSpace(baseAddress) ? () => baseAddress : sharedOptions.BaseAddressFactory;
            BasePathFactory = !string.IsNullOrWhiteSpace(basePath) ? () => basePath : sharedOptions.BasePathFactory;
            HttpTracerModeFactory = httpTracerMode.HasValue ? () => httpTracerMode.Value : sharedOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = trafficVerbosity.HasValue ? () => trafficVerbosity.Value : sharedOptions.TrafficVerbosityFactory;
            LogLevelsFactory = logLevels?.Any() == true ? () => logLevels : sharedOptions.LogLevelsFactory;
            LoggerFactory = (loggerFactory, webApiFriendlyName) => Logger = loggerFactory.CreateLogger(webApiFriendlyName);
            HttpClientHandlerFactory = sharedOptions.HttpClientHandlerFactory;
            DelegatingHandlersFactories = sharedOptions.DelegatingHandlersFactories;
        }

        private Func<Uri> _baseUriFactory;
        public Func<Uri> BaseUriFactory
        {
            get => _baseUriFactory;
            set => _baseUriFactory = value != null ? () => BaseUri = value.Invoke() : null;
        }

        private Func<string> _baseAddressFactory;
        public Func<string> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = value != null ? () => BaseAddress = value.Invoke() : null;
        }

        private Func<string> _basePathFactory;
        public Func<string> BasePathFactory
        {
            get => _basePathFactory;
            set => _basePathFactory = value != null ? () => BasePath = value.Invoke() : null;
        }

        private Func<HttpTracerMode> _httpTracerModeFactory;
        public Func<HttpTracerMode> HttpTracerModeFactory
        {
            get => _httpTracerModeFactory;
            set => _httpTracerModeFactory = () => HttpTracerMode = value.Invoke();
        }

        private Func<HttpMessageParts> _trafficVerbosityFactory;
        public Func<HttpMessageParts> TrafficVerbosityFactory
        {
            get => _trafficVerbosityFactory;
            set => _trafficVerbosityFactory = () => TrafficVerbosity = value.Invoke();
        }

        private Func<LogLevel[]> _logLevelsFactory;
        public Func<LogLevel[]> LogLevelsFactory
        {
            get => _logLevelsFactory;
            set => _logLevelsFactory = () => LogLevels = value.Invoke();
        }
        
        public Func<ILoggerFactory, string, ILogger> LoggerFactory { get; }

        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }

        public IList<Func<ILogger, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}
