using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Proper
{
    /// <inheritdoc cref="IApizrProperOptions"/>
    public class ApizrProperOptions : ApizrProperOptionsBase, IApizrProperOptions
    {
        /// <summary>
        /// The common options constructor
        /// </summary>
        /// <param name="sharedOptions">The shared options</param>
        /// <param name="webApiType">The web api type</param>
        /// <param name="assemblyPolicyRegistryKeys">Global policies</param>
        /// <param name="webApiPolicyRegistryKeys">Specific policies</param>
        /// <param name="baseAddress">The web api base address</param>
        /// <param name="basePath">The web api base path</param>
        /// <param name="handlersParameters">Some handlers parameters</param>
        /// <param name="httpTracerMode">The http tracer mode</param>
        /// <param name="trafficVerbosity">The traffic verbosity</param>
        /// <param name="logLevels">The log levels</param>
        public ApizrProperOptions(IApizrSharedRegistrationOptions sharedOptions,
            Type webApiType,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys, 
            string baseAddress,
            string basePath,
            IDictionary<string, object> handlersParameters,
            HttpTracerMode? httpTracerMode,
            HttpMessageParts? trafficVerbosity,
            params LogLevel[] logLevels) : base(sharedOptions, webApiType, assemblyPolicyRegistryKeys,
            webApiPolicyRegistryKeys)
        {
            BaseUriFactory = !string.IsNullOrWhiteSpace(baseAddress) ? null : sharedOptions.BaseUriFactory;
            BaseAddressFactory = !string.IsNullOrWhiteSpace(baseAddress) ? () => baseAddress : sharedOptions.BaseAddressFactory;
            BasePathFactory = !string.IsNullOrWhiteSpace(basePath) ? () => basePath : sharedOptions.BasePathFactory;
            HandlersParameters = handlersParameters;
            HttpTracerModeFactory = httpTracerMode.HasValue ? () => httpTracerMode.Value : sharedOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = trafficVerbosity.HasValue ? () => trafficVerbosity.Value : sharedOptions.TrafficVerbosityFactory;
            LogLevelsFactory = logLevels?.Any() == true ? () => logLevels : sharedOptions.LogLevelsFactory;
            LoggerFactory = (loggerFactory, webApiFriendlyName) => Logger = loggerFactory.CreateLogger(webApiFriendlyName);
            HttpClientHandlerFactory = sharedOptions.HttpClientHandlerFactory;
            HttpClientFactory = sharedOptions.HttpClientFactory;
            DelegatingHandlersFactories = sharedOptions.DelegatingHandlersFactories;
            ContextFactory = sharedOptions.ContextFactory;
        }

        private Func<Uri> _baseUriFactory;
        /// <inheritdoc />
        public Func<Uri> BaseUriFactory
        {
            get => _baseUriFactory;
            set => _baseUriFactory = value != null ? () => BaseUri = value.Invoke() : null;
        }

        private Func<string> _baseAddressFactory;
        /// <inheritdoc />
        public Func<string> BaseAddressFactory
        {
            get => _baseAddressFactory;
            set => _baseAddressFactory = value != null ? () => BaseAddress = value.Invoke() : null;
        }

        private Func<string> _basePathFactory;
        /// <inheritdoc />
        public Func<string> BasePathFactory
        {
            get => _basePathFactory;
            set => _basePathFactory = value != null ? () => BasePath = value.Invoke() : null;
        }

        private Func<HttpTracerMode> _httpTracerModeFactory;
        /// <inheritdoc />
        public Func<HttpTracerMode> HttpTracerModeFactory
        {
            get => _httpTracerModeFactory;
            set => _httpTracerModeFactory = () => HttpTracerMode = value.Invoke();
        }

        private Func<HttpMessageParts> _trafficVerbosityFactory;
        /// <inheritdoc />
        public Func<HttpMessageParts> TrafficVerbosityFactory
        {
            get => _trafficVerbosityFactory;
            set => _trafficVerbosityFactory = () => TrafficVerbosity = value.Invoke();
        }

        private Func<LogLevel[]> _logLevelsFactory;
        /// <inheritdoc />
        public Func<LogLevel[]> LogLevelsFactory
        {
            get => _logLevelsFactory;
            set => _logLevelsFactory = () => LogLevels = value.Invoke();
        }

        /// <inheritdoc />
        public Func<ILoggerFactory, string, ILogger> LoggerFactory { get; }

        /// <inheritdoc />
        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }

        /// <inheritdoc />
        public Func<HttpMessageHandler, Uri, HttpClient> HttpClientFactory { get; set; }

        /// <inheritdoc />
        public IList<Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}
