using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Resiliencing;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Refit;

namespace Apizr.Configuring.Manager
{
    /// <inheritdoc cref="IApizrManagerOptions"/>
    public class ApizrManagerOptions : ApizrManagerOptionsBase, IApizrManagerOptions
    {
        /// <summary>
        /// The options constructor
        /// </summary>
        /// <param name="commonOptions">The common options</param>
        /// <param name="properOptions">The proper options</param>
        public ApizrManagerOptions(IApizrCommonOptions commonOptions, IApizrProperOptions properOptions) : base(commonOptions, properOptions)
        {
            BaseUriFactory = properOptions.BaseUriFactory;
            BaseAddressFactory = properOptions.BaseAddressFactory;
            BasePathFactory = properOptions.BasePathFactory;
            HttpTracerModeFactory = properOptions.HttpTracerModeFactory;
            TrafficVerbosityFactory = properOptions.TrafficVerbosityFactory;
            LogLevelsFactory = properOptions.LogLevelsFactory;
            LoggerFactoryFactory = commonOptions.LoggerFactoryFactory;
            LoggerFactory = (loggerFactory, webApiFriendlyName) => Logger = properOptions.LoggerFactory.Invoke(loggerFactory, webApiFriendlyName);
            HttpClientHandlerFactory = properOptions.HttpClientHandlerFactory;
            HttpClientConfigurationBuilder = properOptions.HttpClientConfigurationBuilder;
            ResiliencePipelineRegistryFactory = commonOptions.ResiliencePipelineRegistryFactory;
            RefitSettingsFactory = commonOptions.RefitSettingsFactory;
            ConnectivityHandlerFactory = commonOptions.ConnectivityHandlerFactory;
            CacheHandlerFactory = commonOptions.CacheHandlerFactory;
            MappingHandlerFactory = commonOptions.MappingHandlerFactory;
            DelegatingHandlersFactories = properOptions.DelegatingHandlersFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            HttpMessageHandlerFactory = properOptions.HttpMessageHandlerFactory;
            OperationTimeoutFactory = properOptions.OperationTimeoutFactory;
            RequestTimeoutFactory = properOptions.RequestTimeoutFactory;
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
        public Func<ILoggerFactory> LoggerFactoryFactory { get; set; }

        /// <inheritdoc />
        public Func<ILoggerFactory, string, ILogger> LoggerFactory { get; }

        /// <inheritdoc />
        public Func<HttpClientHandler> HttpClientHandlerFactory { get; set; }

        /// <inheritdoc />
        public Action<HttpClient> HttpClientConfigurationBuilder { get; set; }

        /// <inheritdoc />
        public Func<ResiliencePipelineRegistry<string>> ResiliencePipelineRegistryFactory { get; set; }

        private Func<RefitSettings> _refitSettingsFactory;
        /// <inheritdoc />
        public Func<RefitSettings> RefitSettingsFactory
        {
            get => _refitSettingsFactory;
            set => _refitSettingsFactory = () => RefitSettings = value.Invoke();
        }

        /// <inheritdoc />
        public Func<IConnectivityHandler> ConnectivityHandlerFactory { get; set; }

        /// <inheritdoc />
        public Func<ICacheHandler> CacheHandlerFactory { get; set; }

        /// <inheritdoc />
        public Func<IMappingHandler> MappingHandlerFactory { get; set; }

        /// <inheritdoc />
        public IDictionary<Type, Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }

        /// <inheritdoc />
        public Func<ILogger, IApizrManagerOptionsBase, HttpMessageHandler> HttpMessageHandlerFactory { get; set;  }

        private Func<TimeSpan> _operationTimeoutFactory;
        /// <inheritdoc />
        public Func<TimeSpan> OperationTimeoutFactory
        {
            get => _operationTimeoutFactory;
            set => _operationTimeoutFactory = value != null ? () => (TimeSpan)(OperationTimeout = value.Invoke()) : null;
        }

        private Func<TimeSpan> _requestTimeoutFactory;
        /// <inheritdoc />
        public Func<TimeSpan> RequestTimeoutFactory
        {
            get => _requestTimeoutFactory;
            set => _requestTimeoutFactory = value != null ? () => (TimeSpan)(RequestTimeout = value.Invoke()) : null;
        }
    }
    
    /// <inheritdoc cref="IApizrManagerOptions{TWebApi}"/>
    public class ApizrManagerOptions<TWebApi> : IApizrManagerOptions<TWebApi>
    {
        /// <summary>
        /// The options
        /// </summary>
        protected readonly IApizrManagerOptionsBase Options;

        /// <summary>
        /// The options constructor
        /// </summary>
        /// <param name="apizrOptions">The base options</param>
        public ApizrManagerOptions(IApizrManagerOptionsBase apizrOptions)
        {
            Options = apizrOptions;
        }

        /// <inheritdoc />
        public Type WebApiType => Options.WebApiType;

        /// <inheritdoc />
        public Type CrudModelType => Options.CrudModelType;

        /// <inheritdoc />
        public TypeInfo TypeInfo => Options.TypeInfo;

        /// <inheritdoc />
        public bool IsCrudApi => Options.IsCrudApi;

        /// <inheritdoc />
        public Uri BaseUri => Options.BaseUri;

        /// <inheritdoc />
        public string BaseAddress => Options.BaseAddress;

        /// <inheritdoc />
        public string BasePath => Options.BasePath;

        /// <inheritdoc />
        public Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> PrimaryHandlerFactory
            => Options.PrimaryHandlerFactory;

        /// <inheritdoc />
        public HttpTracerMode HttpTracerMode => Options.HttpTracerMode;

        /// <inheritdoc />
        public HttpMessageParts TrafficVerbosity => Options.TrafficVerbosity;

        /// <inheritdoc />
        public LogLevel[] LogLevels => Options.LogLevels;

        /// <inheritdoc />
        public Action<ApizrException> OnException => Options.OnException;

        /// <inheritdoc />
        public bool LetThrowOnExceptionWithEmptyCache => Options.LetThrowOnExceptionWithEmptyCache;

        /// <inheritdoc />
        public IDictionary<string, object> HandlersParameters => Options.HandlersParameters;

        /// <inheritdoc />
        public IDictionary<ApizrRegistrationMode, IList<string>> Headers => Options.Headers;

        /// <inheritdoc />
        public IDictionary<(ApizrRegistrationMode Mode, ApizrLifetimeScope Scope), Func<IList<string>>> HeadersFactories => Options.HeadersFactories;

        /// <inheritdoc />
        public TimeSpan? OperationTimeout => Options.OperationTimeout;

        /// <inheritdoc />
        public TimeSpan? RequestTimeout => Options.RequestTimeout;

        /// <inheritdoc />
        public Func<string, bool> ShouldRedactHeaderValue => Options.ShouldRedactHeaderValue;

        /// <inheritdoc />
        public IDictionary<ApizrConfigurationSource, ResiliencePipelineAttributeBase[]> ResiliencePipelineOptions => Options.ResiliencePipelineOptions;

        /// <inheritdoc />
        public IDictionary<ApizrConfigurationSource, CacheAttributeBase> CacheOptions => Options.CacheOptions;

        /// <inheritdoc />
        Action<IApizrResilienceContextOptionsBuilder> IApizrGlobalSharedOptionsBase.ContextOptionsBuilder
        {
            get => Options.ContextOptionsBuilder;
            set {}
        }

        /// <inheritdoc />
        IDictionary<string, Func<object>> IApizrGlobalSharedOptionsBase.ResiliencePropertiesFactories => Options.ResiliencePropertiesFactories;

        /// <inheritdoc />
        public ILogger Logger => Options.Logger;

        /// <inheritdoc />
        public RefitSettings RefitSettings => Options.RefitSettings;

        /// <inheritdoc />
        public IConfigurationSection ApizrConfigurationSection => Options.ApizrConfigurationSection;
    }
}