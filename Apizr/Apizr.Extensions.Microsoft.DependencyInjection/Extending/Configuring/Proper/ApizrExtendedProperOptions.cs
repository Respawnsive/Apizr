﻿using System;
using System.Collections.Generic;
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
            Type webApiType, Type apizrManagerType, Uri baseAddress,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            LogLevel? logLevel,
            string[] assemblyPolicyRegistryKeys, 
            string[] webApiPolicyRegistryKeys) : base(sharedOptions, 
            webApiType, 
            assemblyPolicyRegistryKeys, 
            webApiPolicyRegistryKeys)
        {
            ApizrManagerType = apizrManagerType;
            BaseAddressFactory = _ => baseAddress;
            HttpTracerModeFactory = serviceProvider => httpTracerMode ?? sharedOptions.HttpTracerModeFactory.Invoke(serviceProvider);
            TrafficVerbosityFactory = serviceProvider => trafficVerbosity ?? sharedOptions.TrafficVerbosityFactory.Invoke(serviceProvider);
            LogLevelFactory = serviceProvider => logLevel ?? sharedOptions.LogLevelFactory.Invoke(serviceProvider);
            HttpClientHandlerFactory = sharedOptions.HttpClientHandlerFactory;
            DelegatingHandlersExtendedFactories = sharedOptions.DelegatingHandlersExtendedFactories;
        }

        public Type ApizrManagerType { get; }

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
        public Action<IHttpClientBuilder> HttpClientBuilder { get; set; }
        public IList<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
    }
}
