using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using Apizr.Caching;
using Apizr.Cancelling.Attributes.Operation;
using Apizr.Cancelling.Attributes.Request;
using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Registry;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Helping;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Mapping;
using Apizr.Requesting;
using Apizr.Resiliencing;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Refit;

[assembly: Apizr.Preserve]
namespace Apizr
{
    /// <summary>
    /// The builder
    /// </summary>
    public class ApizrBuilder : IApizrBuilder
    {
        private ApizrBuilder()
        {}
        
        private static ApizrBuilder _instance;
        internal static ApizrBuilder Instance => _instance ??= new ApizrBuilder();
        public static IApizrBuilder Current => Instance;

        #region Registry

        /// <inheritdoc/>
        public IApizrRegistry CreateRegistry(Action<IApizrRegistryBuilder> registryBuilder,
            Action<IApizrCommonOptionsBuilder> commonOptionsBuilder = null)
            => CreateRegistry(registryBuilder, null, commonOptionsBuilder);

        #endregion

        #region Crud
        
        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> CreateCrudManagerFor<T>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null) where T : class =>
            CreateCrudManagerFor<T, int, IEnumerable<T>, IDictionary<string, object>,
                ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null) where T : class =>
            CreateCrudManagerFor<T, TKey, IEnumerable<T>, IDictionary<string, object>,
                ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey,
            TReadAllResult>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where T : class =>
            CreateCrudManagerFor<T, TKey, TReadAllResult, IDictionary<string, object>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> CreateCrudManagerFor<T, TKey, TReadAllResult,
            TReadAllParams>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where T : class =>
            CreateCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc/>
        public TApizrManager CreateCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Func<ILazyFactory<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
                IMappingHandler, ILazyFactory<ResiliencePipelineRegistry<string>>, IApizrManagerOptions<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>,
                TApizrManager> apizrManagerFactory,
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>
        {
            var crudedType = typeof(T);

            var crudAttribute = crudedType.GetCustomAttribute<CrudEntityAttribute>();
            if (crudAttribute != null)
            {
                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.WithBaseAddress(crudAttribute.BaseUri);
                else
                    optionsBuilder += builder => builder.WithBaseAddress(crudAttribute.BaseUri, ApizrDuplicateStrategy.Ignore);
            }

            return CreateManagerFor(apizrManagerFactory, CreateCommonOptions(), optionsBuilder);
        }

        #endregion

        #region General

        /// <inheritdoc/>
        public IApizrManager<TWebApi> CreateManagerFor<TWebApi>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null) =>
            CreateManagerFor<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<TWebApi>(lazyWebApi, connectivityHandler, cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <inheritdoc/>
        public TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                ILazyFactory<ResiliencePipelineRegistry<string>>, IApizrManagerOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi> =>
            CreateManagerFor(apizrManagerFactory, CreateCommonOptions(), optionsBuilder);

        private TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                ILazyFactory<ResiliencePipelineRegistry<string>>, IApizrManagerOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            IApizrCommonOptions commonOptions,
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi>
            => CreateManagerFor(apizrManagerFactory, commonOptions, CreateProperOptions<TWebApi>(commonOptions),
                optionsBuilder);


        internal TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler, ILazyFactory<ResiliencePipelineRegistry<string>>, IApizrManagerOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            IApizrCommonOptions commonOptions,
            IApizrProperOptions properOptions,
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TApizrManager : IApizrManager<TWebApi>
        {
            var apizrOptions = CreateOptions(commonOptions, properOptions, optionsBuilder);

            var httpHandlerFactory = new Func<HttpMessageHandler>(() =>
            {
                var handlerBuilder = new ExtendedHttpHandlerBuilder(apizrOptions.HttpClientHandlerFactory.Invoke(), apizrOptions);
                var resiliencePipeline = ResiliencePipeline<HttpResponseMessage>.Empty;
                var resiliencePipelineBuilder = new ResiliencePipelineBuilder<HttpResponseMessage>();
                var wrappedResiliencePipelineKeys = new List<string>();
                if (apizrOptions.ResiliencePipelineRegistryKeys != null && apizrOptions.ResiliencePipelineRegistryKeys.Any())
                {
                    var resiliencePipelineRegistry = apizrOptions.ResiliencePipelineRegistryFactory.Invoke();
                    foreach (var resiliencePipelineRegistryKey in apizrOptions.ResiliencePipelineRegistryKeys)
                    {
                        if (resiliencePipelineRegistry.TryGetPipeline<HttpResponseMessage>(resiliencePipelineRegistryKey, out var registeredResiliencePipeline))
                        {
                            resiliencePipelineBuilder.AddPipeline(registeredResiliencePipeline);
                            wrappedResiliencePipelineKeys.Add(resiliencePipelineRegistryKey);
                        }
                    }

                    if(wrappedResiliencePipelineKeys.Any())
                        resiliencePipeline = resiliencePipelineBuilder.Build();
                }

                var pipelineProvider = new Func<HttpRequestMessage, CancellationToken, ResiliencePipeline<HttpResponseMessage>>(
                    (request, ct) =>
                    {
                        var context = request.GetOrBuildApizrResilienceContext(ct);
                        if (!context.TryGetLogger(out var contextLogger, out var logLevels, out var verbosity, out var tracerMode))
                        {
                            if (request.TryGetOptions(out var requestOptions))
                            {
                                logLevels = requestOptions.LogLevels;
                                verbosity = requestOptions.TrafficVerbosity;
                                tracerMode = requestOptions.HttpTracerMode;
                            }
                            else
                            {
                                logLevels = apizrOptions.LogLevels;
                                verbosity = apizrOptions.TrafficVerbosity;
                                tracerMode = apizrOptions.HttpTracerMode;
                            }
                            contextLogger = apizrOptions.Logger;

                            context.WithLogger(contextLogger, logLevels, verbosity, tracerMode);
                            request.SetApizrResilienceContext(context);
                        }

                        foreach (var wrappedPolicyKey in wrappedResiliencePipelineKeys)
                            contextLogger.Log(logLevels.Low(), $"{context.OperationKey}: Resilience pipeline with key {wrappedPolicyKey} will be applied");

                        return resiliencePipeline;
                    });
                handlerBuilder.AddHandler(new ResilienceHttpMessageHandler(pipelineProvider, apizrOptions));

                foreach (var delegatingHandlersFactory in apizrOptions.DelegatingHandlersFactories.Values)
                    handlerBuilder.AddHandler(delegatingHandlersFactory.Invoke(apizrOptions.Logger, apizrOptions));

                var innerHandler = handlerBuilder.Build();
                var primaryHandler = apizrOptions.PrimaryHandlerFactory?.Invoke(innerHandler, apizrOptions.Logger, apizrOptions) ?? innerHandler;
                var primaryMessageHandler = new ApizrHttpMessageHandler(primaryHandler, apizrOptions);

                return primaryMessageHandler;
            });
            
            var webApiFactory = new Func<object>(() =>
            {
                // HttpClient
                var httpClient = new ApizrHttpClient(httpHandlerFactory.Invoke(), false, apizrOptions) {BaseAddress = apizrOptions.BaseUri};

                // Custom client config
                apizrOptions.HttpClientConfigurationBuilder.Invoke(httpClient);

                // Refit rest service
                return RestService.For<TWebApi>(httpClient, apizrOptions.RefitSettings);
            });
            var lazyWebApi = new LazyFactory<TWebApi>(webApiFactory);
            var lazyPolicyRegistry = new LazyFactory<ResiliencePipelineRegistry<string>>(apizrOptions.ResiliencePipelineRegistryFactory);
            var apizrManager = apizrManagerFactory(lazyWebApi, apizrOptions.ConnectivityHandlerFactory.Invoke(),
                apizrOptions.GetCacheHanderFactory()?.Invoke() ?? apizrOptions.CacheHandlerFactory.Invoke(),
                apizrOptions.GetMappingHanderFactory()?.Invoke() ?? apizrOptions.MappingHandlerFactory.Invoke(),
                lazyPolicyRegistry, new ApizrManagerOptions<TWebApi>(apizrOptions));

            return apizrManager;
        }

        #endregion

        #region Builder

        internal IApizrCommonOptions CreateCommonOptions(
            Action<IApizrCommonOptionsBuilder> commonOptionsBuilder = null, IApizrCommonOptions baseCommonOptions = null)
        {
            if (baseCommonOptions is not ApizrCommonOptions baseApizrCommonOptions)
                baseApizrCommonOptions = new ApizrCommonOptions();

            var builder = new ApizrCommonOptionsBuilder(baseApizrCommonOptions) as IApizrCommonOptionsBuilder;

            commonOptionsBuilder?.Invoke(builder);

            builder.ApizrOptions.BaseUriFactory?.Invoke();
            builder.ApizrOptions.BasePathFactory?.Invoke();
            builder.ApizrOptions.LogLevelsFactory.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory.Invoke();
            builder.ApizrOptions.RefitSettingsFactory.Invoke();
            builder.ApizrOptions.OperationTimeoutFactory?.Invoke();
            builder.ApizrOptions.RequestTimeoutFactory?.Invoke();
            builder.ApizrOptions.HeadersFactory?.Invoke();

            return builder.ApizrOptions;
        }

        internal IApizrProperOptions CreateProperOptions<TWebApi>(IApizrCommonOptions commonOptions,
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
        {
            if (commonOptions == null)
                throw new ArgumentNullException(nameof(commonOptions));

            var webApiType = typeof(TWebApi);

            string baseAddress = null;
            string basePath = null;
            var webApiAttribute = GetWebApiAttribute(webApiType);
            if (!string.IsNullOrWhiteSpace(webApiAttribute?.BaseAddressOrPath))
            {
                if(Uri.IsWellFormedUriString(webApiAttribute.BaseAddressOrPath, UriKind.Absolute))
                    baseAddress = webApiAttribute.BaseAddressOrPath;
                else
                    basePath = webApiAttribute.BaseAddressOrPath;
            }

            IList<HandlerParameterAttribute> properParameterAttributes, commonParameterAttributes;
            LogAttribute properLogAttribute, commonLogAttribute;
            OperationTimeoutAttribute properOperationTimeoutAttribute, commonOperationTimeoutAttribute;
            RequestTimeoutAttribute properRequestTimeoutAttribute, commonRequestTimeoutAttribute;
            ResiliencePipelineAttribute webApiResiliencePipelineAttribute;
            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(webApiType))
            {
                var modelType = webApiType.GetGenericArguments().First();
                var modelTypeInfo = modelType.GetTypeInfo();
                properParameterAttributes = modelTypeInfo.GetCustomAttributes<HandlerParameterAttribute>(true)
                    .Where(att => att is not CrudHandlerParameterAttribute)
                    .ToList();
                commonParameterAttributes = modelType.Assembly.GetCustomAttributes<HandlerParameterAttribute>().ToList();
                properLogAttribute = modelTypeInfo.GetCustomAttribute<LogAttribute>(true);
                commonLogAttribute = modelType.Assembly.GetCustomAttribute<LogAttribute>();
                properOperationTimeoutAttribute = modelTypeInfo.GetCustomAttribute<OperationTimeoutAttribute>(true);
                commonOperationTimeoutAttribute = modelType.Assembly.GetCustomAttribute<OperationTimeoutAttribute>();
                properRequestTimeoutAttribute = modelTypeInfo.GetCustomAttribute<RequestTimeoutAttribute>(true);
                commonRequestTimeoutAttribute = modelType.Assembly.GetCustomAttribute<RequestTimeoutAttribute>();
                webApiResiliencePipelineAttribute = modelTypeInfo.GetCustomAttribute<ResiliencePipelineAttribute>(true);
            }
            else
            {
                var webApiTypeInfo = webApiType.GetTypeInfo();
                properParameterAttributes = webApiTypeInfo.GetCustomAttributes<HandlerParameterAttribute>(true).ToList();
                commonParameterAttributes = webApiType.Assembly.GetCustomAttributes<HandlerParameterAttribute>().ToList();
                properLogAttribute = webApiTypeInfo.GetCustomAttribute<LogAttribute>(true);
                commonLogAttribute = webApiType.Assembly.GetCustomAttribute<LogAttribute>();
                properOperationTimeoutAttribute = webApiTypeInfo.GetCustomAttribute<OperationTimeoutAttribute>(true);
                commonOperationTimeoutAttribute = webApiType.Assembly.GetCustomAttribute<OperationTimeoutAttribute>();
                properRequestTimeoutAttribute = webApiTypeInfo.GetCustomAttribute<RequestTimeoutAttribute>(true);
                commonRequestTimeoutAttribute = webApiType.Assembly.GetCustomAttribute<RequestTimeoutAttribute>();
                webApiResiliencePipelineAttribute = webApiTypeInfo.GetCustomAttribute<ResiliencePipelineAttribute>(true);
            }

            var assemblyResiliencePipelineAttribute = webApiType.Assembly.GetCustomAttribute<ResiliencePipelineAttribute>();

            var handlersParameters = new Dictionary<string, object>();
            foreach (var commonParameterAttribute in commonParameterAttributes.Where(att => !string.IsNullOrWhiteSpace(att.Key)))
                handlersParameters[commonParameterAttribute.Key!] = commonParameterAttribute.Value;
            foreach (var commonOptionsHandlersParameter in commonOptions.HandlersParameters)
                handlersParameters[commonOptionsHandlersParameter.Key] = commonOptionsHandlersParameter.Value;
            foreach (var properParameterAttribute in properParameterAttributes.Where(att => !string.IsNullOrWhiteSpace(att.Key)))
                handlersParameters[properParameterAttribute.Key!] = properParameterAttribute.Value;

            var builder = new ApizrProperOptionsBuilder(new ApizrProperOptions(commonOptions, webApiType,
                assemblyResiliencePipelineAttribute?.RegistryKeys, webApiResiliencePipelineAttribute?.RegistryKeys,
                baseAddress,
                basePath,
                handlersParameters,
                properLogAttribute?.HttpTracerMode ?? (commonOptions.HttpTracerMode != HttpTracerMode.Unspecified ? commonOptions.HttpTracerMode : commonLogAttribute?.HttpTracerMode),
                properLogAttribute?.TrafficVerbosity ?? (commonOptions.TrafficVerbosity != HttpMessageParts.Unspecified ? commonOptions.TrafficVerbosity : commonLogAttribute?.TrafficVerbosity),
                properOperationTimeoutAttribute?.Timeout ?? commonOperationTimeoutAttribute?.Timeout,
                properRequestTimeoutAttribute?.Timeout ?? commonRequestTimeoutAttribute?.Timeout,
                properLogAttribute?.LogLevels ?? (commonOptions.LogLevels?.Any() == true ? commonOptions.LogLevels : commonLogAttribute?.LogLevels))) as IApizrProperOptionsBuilder;

            properOptionsBuilder?.Invoke(builder);

            builder.ApizrOptions.BaseUriFactory?.Invoke();
            builder.ApizrOptions.BaseAddressFactory?.Invoke();
            builder.ApizrOptions.BasePathFactory?.Invoke();
            builder.ApizrOptions.LogLevelsFactory.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory.Invoke();
            builder.ApizrOptions.OperationTimeoutFactory?.Invoke();
            builder.ApizrOptions.RequestTimeoutFactory?.Invoke();
            builder.ApizrOptions.HeadersFactory?.Invoke();

            return builder.ApizrOptions;
        }

        internal IApizrRegistry CreateRegistry(Action<IApizrRegistryBuilder> registryBuilder,
            IApizrCommonOptions baseCommonOptions,
            Action<IApizrCommonOptionsBuilder> commonOptionsBuilder = null, ApizrRegistry mainRegistry = null)
        {
            if (registryBuilder == null)
                throw new ArgumentNullException(nameof(registryBuilder));

            var commonOptions = CreateCommonOptions(commonOptionsBuilder, baseCommonOptions);

            var apizrRegistry = CreateRegistry(commonOptions, registryBuilder, mainRegistry);

            return apizrRegistry;
        }

        internal IApizrRegistry CreateRegistry(IApizrCommonOptions commonOptions, Action<IApizrRegistryBuilder> registryBuilder, ApizrRegistry mainRegistry = null)
        {
            if (commonOptions == null)
                throw new ArgumentNullException(nameof(commonOptions));

            if (registryBuilder == null)
                throw new ArgumentNullException(nameof(registryBuilder));

            var builder = new ApizrRegistryBuilder(commonOptions, mainRegistry);

            registryBuilder.Invoke(builder);

            return builder.ApizrRegistry;
        }

        private static IApizrManagerOptions CreateOptions(IApizrCommonOptions commonOptions, IApizrProperOptions properOptions, Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        {
            var builder = new ApizrManagerOptionsBuilder(new ApizrManagerOptions(commonOptions, properOptions)) as IApizrManagerOptionsBuilder;

            optionsBuilder?.Invoke(builder);

            if (builder.ApizrOptions.BaseUriFactory == null)
            {
                builder.ApizrOptions.BaseAddressFactory?.Invoke();
                builder.ApizrOptions.BasePathFactory?.Invoke();
                if (Uri.TryCreate(UrlHelper.Combine(builder.ApizrOptions.BaseAddress, builder.ApizrOptions.BasePath), UriKind.RelativeOrAbsolute, out var baseUri))
                    builder.WithBaseAddress(baseUri);
            }
            else if (builder.ApizrOptions.BasePathFactory != null)
            {
                builder.ApizrOptions.BaseUriFactory?.Invoke();
                builder.ApizrOptions.BasePathFactory?.Invoke();
                if (Uri.TryCreate(UrlHelper.Combine(builder.ApizrOptions.BaseUri.ToString(), builder.ApizrOptions.BasePath), UriKind.RelativeOrAbsolute, out var baseUri))
                    builder.WithBaseAddress(baseUri);
            }

            builder.ApizrOptions.BaseUriFactory?.Invoke();
            builder.ApizrOptions.LogLevelsFactory.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory.Invoke();
            builder.ApizrOptions.RefitSettingsFactory.Invoke();
            builder.ApizrOptions.LoggerFactory.Invoke(builder.ApizrOptions.LoggerFactoryFactory.Invoke(), builder.ApizrOptions.WebApiType.GetFriendlyName());
            builder.ApizrOptions.HeadersFactory?.Invoke();
            builder.ApizrOptions.OperationTimeoutFactory?.Invoke();
            builder.ApizrOptions.RequestTimeoutFactory?.Invoke();
            builder.ApizrOptions.HeadersFactory?.Invoke();

            return builder.ApizrOptions;
        }

        #endregion

        #region Internal
        
        internal static WebApiAttribute GetWebApiAttribute(Type webApiType)
        {
            var webApiAttribute = webApiType.GetTypeInfo().GetCustomAttribute<WebApiAttribute>(true);
            if (webApiAttribute != null)
                return webApiAttribute;

            foreach (var parentInterface in webApiType.GetInterfaces())
            {
                webApiAttribute = parentInterface.GetTypeInfo().GetCustomAttribute<WebApiAttribute>(true);
                if (webApiAttribute != null)
                    return webApiAttribute;
            }

            return null;
        }

        #endregion
    }
}
