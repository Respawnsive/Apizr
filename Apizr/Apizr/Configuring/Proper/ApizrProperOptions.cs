using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly.Registry;

namespace Apizr.Configuring.Proper
{
    public class ApizrProperOptions : ApizrProperOptionsBase, IApizrProperOptions
    {
        public ApizrProperOptions(IApizrSharedOptions sharedOptions,
            Type webApiType, Uri baseAddress,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            LogLevel? logLevel,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys) : base(sharedOptions, webApiType, assemblyPolicyRegistryKeys,
            webApiPolicyRegistryKeys)
        {
            BaseAddressFactory = () => baseAddress;
            HttpTracerModeFactory = () => httpTracerMode ?? sharedOptions.HttpTracerModeFactory.Invoke();
            TrafficVerbosityFactory = () => trafficVerbosity ?? sharedOptions.TrafficVerbosityFactory.Invoke();
            LogLevelFactory = () => logLevel ?? sharedOptions.LogLevelFactory.Invoke();
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

        private Func<LogLevel> _logLevelFactory;
        public Func<LogLevel> LogLevelFactory
        {
            get => _logLevelFactory;
            set => _logLevelFactory = () => LogLevel = value.Invoke();
        }

        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }

        public IList<Func<ILogger, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}
