using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
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
using Apizr.Policing;
using Apizr.Requesting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Refit;

[assembly: Apizr.Preserve]
namespace Apizr
{
    /// <inheritdoc/>
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
            CreateManagerFor<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null) where T : class =>
            CreateManagerFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey,
            TReadAllResult>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where T : class =>
            CreateManagerFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> CreateCrudManagerFor<T, TKey, TReadAllResult,
            TReadAllParams>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where T : class where TReadAllParams : class =>
            CreateManagerFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <inheritdoc/>
        public TApizrManager CreateCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Func<ILazyFactory<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
                IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrManagerOptionsBase,
                TApizrManager> apizrManagerFactory,
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where T : class
            where TReadAllParams : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            CreateManagerFor(apizrManagerFactory, CreateCommonOptions(), optionsBuilder);

        #endregion

        #region General

        /// <inheritdoc/>
        public IApizrManager<TWebApi> CreateManagerFor<TWebApi>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null) =>
            CreateManagerFor<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<TWebApi>(lazyWebApi, connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <inheritdoc/>
        public TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                IReadOnlyPolicyRegistry<string>, IApizrManagerOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi> =>
            CreateManagerFor(apizrManagerFactory, CreateCommonOptions(), optionsBuilder);

        private TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                IReadOnlyPolicyRegistry<string>, IApizrManagerOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            IApizrCommonOptions commonOptions,
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi>
            => CreateManagerFor(apizrManagerFactory, commonOptions, CreateProperOptions<TWebApi>(commonOptions),
                optionsBuilder);


        internal TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrManagerOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            IApizrCommonOptions commonOptions,
            IApizrProperOptions properOptions,
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TApizrManager : IApizrManager<TWebApi>
        {
            var apizrOptions = CreateOptions(commonOptions, properOptions, optionsBuilder);

            var httpHandlerFactory = new Func<HttpMessageHandler>(() =>
            {
                var handlerBuilder = new ExtendedHttpHandlerBuilder(apizrOptions.HttpClientHandlerFactory.Invoke(), apizrOptions);

                if (apizrOptions.PolicyRegistryKeys != null && apizrOptions.PolicyRegistryKeys.Any())
                {
                    var policyRegistry = apizrOptions.PolicyRegistryFactory.Invoke();
                    foreach (var policyRegistryKey in apizrOptions.PolicyRegistryKeys)
                    {
                        if (policyRegistry.TryGet<IsPolicy>(policyRegistryKey, out var registeredPolicy) &&
                            registeredPolicy is IAsyncPolicy<HttpResponseMessage> registeredPolicyForHttpResponseMessage)
                        {
                            var policySelector = new Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>>(
                                    request =>
                                    {
                                        var context = request.GetOrBuildApizrPolicyExecutionContext();
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
                                            request.SetApizrPolicyExecutionContext(context);
                                        }

                                        contextLogger.Log(logLevels.Low(), $"{context.OperationKey}: Policy with key {policyRegistryKey} will be applied");

                                        return registeredPolicyForHttpResponseMessage;
                                    });
                            handlerBuilder.AddHandler(new PolicyHttpMessageHandler(policySelector));
                        }
                    }
                }

                foreach (var delegatingHandlersFactory in apizrOptions.DelegatingHandlersFactories.Values)
                    handlerBuilder.AddHandler(delegatingHandlersFactory.Invoke(apizrOptions.Logger, apizrOptions));

                var innerHandler = handlerBuilder.Build();
                var primaryHandler = apizrOptions.PrimaryHandlerFactory?.Invoke(innerHandler, apizrOptions.Logger, apizrOptions) ?? innerHandler;
                var primaryMessageHandler = new ApizrHttpMessageHandler(primaryHandler);

                return primaryMessageHandler;
            });
            
            var webApiFactory = new Func<object>(() => RestService.For<TWebApi>(apizrOptions.HttpClientFactory.Invoke(httpHandlerFactory.Invoke(), apizrOptions.BaseUri), apizrOptions.RefitSettings));
            var lazyWebApi = new LazyFactory<TWebApi>(webApiFactory);
            var apizrManager = apizrManagerFactory(lazyWebApi, apizrOptions.ConnectivityHandlerFactory.Invoke(),
                apizrOptions.GetCacheHanderFactory()?.Invoke() ?? apizrOptions.CacheHandlerFactory.Invoke(),
                apizrOptions.GetMappingHanderFactory()?.Invoke() ?? apizrOptions.MappingHandlerFactory.Invoke(), 
                apizrOptions.PolicyRegistryFactory.Invoke(), new ApizrManagerOptions<TWebApi>(apizrOptions));

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
            PolicyAttribute webApiPolicyAttribute;
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
                webApiPolicyAttribute = modelTypeInfo.GetCustomAttribute<PolicyAttribute>(true);
            }
            else
            {
                var webApiTypeInfo = webApiType.GetTypeInfo();
                properParameterAttributes = webApiTypeInfo.GetCustomAttributes<HandlerParameterAttribute>(true).ToList();
                commonParameterAttributes = webApiType.Assembly.GetCustomAttributes<HandlerParameterAttribute>().ToList();
                properLogAttribute = webApiTypeInfo.GetCustomAttribute<LogAttribute>(true);
                commonLogAttribute = webApiType.Assembly.GetCustomAttribute<LogAttribute>();
                webApiPolicyAttribute = webApiTypeInfo.GetCustomAttribute<PolicyAttribute>(true);
            }

            var assemblyPolicyAttribute = webApiType.Assembly.GetCustomAttribute<PolicyAttribute>();

            var handlersParameters = new Dictionary<string, object>();
            foreach (var commonParameterAttribute in commonParameterAttributes.Where(att => !string.IsNullOrWhiteSpace(att.Key)))
                handlersParameters[commonParameterAttribute.Key!] = commonParameterAttribute.Value;
            foreach (var commonOptionsHandlersParameter in commonOptions.HandlersParameters)
                handlersParameters[commonOptionsHandlersParameter.Key] = commonOptionsHandlersParameter.Value;
            foreach (var properParameterAttribute in properParameterAttributes.Where(att => !string.IsNullOrWhiteSpace(att.Key)))
                handlersParameters[properParameterAttribute.Key!] = properParameterAttribute.Value;

            var builder = new ApizrProperOptionsBuilder(new ApizrProperOptions(commonOptions, webApiType,
                assemblyPolicyAttribute?.RegistryKeys, webApiPolicyAttribute?.RegistryKeys,
                baseAddress,
                basePath,
                handlersParameters,
                properLogAttribute?.HttpTracerMode ?? (commonOptions.HttpTracerMode != HttpTracerMode.Unspecified ? commonOptions.HttpTracerMode : commonLogAttribute?.HttpTracerMode),
                properLogAttribute?.TrafficVerbosity ?? (commonOptions.TrafficVerbosity != HttpMessageParts.Unspecified ? commonOptions.TrafficVerbosity : commonLogAttribute?.TrafficVerbosity), 
                properLogAttribute?.LogLevels ?? (commonOptions.LogLevels?.Any() == true ? commonOptions.LogLevels : commonLogAttribute?.LogLevels))) as IApizrProperOptionsBuilder;

            properOptionsBuilder?.Invoke(builder);

            builder.ApizrOptions.BaseUriFactory?.Invoke();
            builder.ApizrOptions.BaseAddressFactory?.Invoke();
            builder.ApizrOptions.BasePathFactory?.Invoke();
            builder.ApizrOptions.LogLevelsFactory.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory.Invoke();

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
