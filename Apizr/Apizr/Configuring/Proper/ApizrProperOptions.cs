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
            Uri baseAddress,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            params LogLevel[] logLevels) : base(sharedOptions, webApiType, assemblyPolicyRegistryKeys,
            webApiPolicyRegistryKeys)
        {
            BaseAddressFactory = () => baseAddress ?? sharedOptions.BaseAddressFactory?.Invoke();
            HttpTracerModeFactory = () => httpTracerMode ?? sharedOptions.HttpTracerModeFactory.Invoke();
            TrafficVerbosityFactory = () => trafficVerbosity ?? sharedOptions.TrafficVerbosityFactory.Invoke();
            LogLevelsFactory = () => logLevels?.Any() == true ? logLevels : sharedOptions.LogLevelsFactory.Invoke();
            LoggerFactory = (loggerFactory, webApiFriendlyName) => Logger = loggerFactory.CreateLogger(webApiFriendlyName);
            HttpClientHandlerFactory = sharedOptions.HttpClientHandlerFactory;
            DelegatingHandlersFactories = sharedOptions.DelegatingHandlersFactories;
        }

        private Func<Uri> _baseAddressFactory;
        public Func<Uri> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = () => BaseAddress = value.Invoke();
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
