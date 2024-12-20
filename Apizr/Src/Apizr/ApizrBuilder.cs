﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
using Apizr.Caching.Attributes;
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

        /// <summary>
        /// Current Apizr builder instance
        /// </summary>
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
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyResiliencePipelineRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        lazyResiliencePipelineRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null) where T : class =>
            CreateCrudManagerFor<T, TKey, IEnumerable<T>, IDictionary<string, object>,
                ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyResiliencePipelineRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        lazyResiliencePipelineRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey,
            TReadAllResult>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where T : class =>
            CreateCrudManagerFor<T, TKey, TReadAllResult, IDictionary<string, object>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyResiliencePipelineRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        lazyResiliencePipelineRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc/>
        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> CreateCrudManagerFor<T, TKey, TReadAllResult,
            TReadAllParams>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where T : class =>
            CreateCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyResiliencePipelineRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        lazyResiliencePipelineRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc/>
        public TApizrManager CreateCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Func<ILazyFactory<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
                IMappingHandler, ILazyFactory<ResiliencePipelineRegistry<string>>,
                IApizrManagerOptions<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>,
                TApizrManager> apizrManagerFactory,
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
            where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            CreateManagerFor(apizrManagerFactory, CreateCommonOptions(), optionsBuilder);

        #endregion

        #region General

        /// <inheritdoc/>
        public IApizrManager<TWebApi> CreateManagerFor<TWebApi>(
            Action<IApizrManagerOptionsBuilder> optionsBuilder = null) =>
            CreateManagerFor<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyResiliencePipelineRegistry, apizrOptions) =>
                    new ApizrManager<TWebApi>(lazyWebApi, connectivityHandler, cacheHandler, mappingHandler,
                        lazyResiliencePipelineRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

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
            var apizrOptions = CreateManagerOptions(commonOptions, properOptions, optionsBuilder);
            var webApiFriendlyName = properOptions.WebApiType.GetFriendlyName();

            var httpHandlerFactory = new Func<HttpMessageHandler>(() =>
            {
                var handlerBuilder = new ExtendedHttpHandlerBuilder(apizrOptions.HttpClientHandlerFactory.Invoke(), apizrOptions);
                handlerBuilder.AddHandler(new ResilienceHttpMessageHandler(apizrOptions.ResiliencePipelineRegistryFactory.Invoke(), apizrOptions));

                foreach (var httpMessageHandlersFactory in apizrOptions.DelegatingHandlersFactories.Values)
                    handlerBuilder.AddHandler(httpMessageHandlersFactory.Invoke(apizrOptions));

                if(apizrOptions.HttpMessageHandlerFactory != null)
                    handlerBuilder.AddHandler(apizrOptions.HttpMessageHandlerFactory.Invoke(apizrOptions));

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

                // Api URI check
                if (httpClient.BaseAddress == null)
                    throw new ArgumentNullException(nameof(httpClient.BaseAddress), $"No base address found for {webApiFriendlyName}");

                // Refit rest service
                return RestService.For<TWebApi>(httpClient, apizrOptions.RefitSettings);
            });
            var lazyWebApi = new LazyFactory<TWebApi>(webApiFactory);
            var lazyResiliencePipelineRegistry = new LazyFactory<ResiliencePipelineRegistry<string>>(apizrOptions.ResiliencePipelineRegistryFactory);
            var apizrManager = apizrManagerFactory(lazyWebApi, apizrOptions.ConnectivityHandlerFactory.Invoke(),
                apizrOptions.GetCacheHandlerInternalFactory()?.Invoke() ?? apizrOptions.CacheHandlerFactory.Invoke(),
                apizrOptions.GetMappingHandlerInternalFactory()?.Invoke() ?? apizrOptions.MappingHandlerFactory.Invoke(),
                lazyResiliencePipelineRegistry, new ApizrManagerOptions<TWebApi>(apizrOptions));

            return apizrManager;
        }

        #endregion

        #region Builder

        internal IApizrCommonOptions CreateCommonOptions(
            Action<IApizrCommonOptionsBuilder> commonOptionsBuilder = null, 
            IApizrCommonOptions baseCommonOptions = null)
        {
            var commonOptions = new ApizrCommonOptions(baseCommonOptions);
            var builder = new ApizrCommonOptionsBuilder(commonOptions) as IApizrCommonOptionsBuilder;

            commonOptionsBuilder?.Invoke(builder);

            builder.ApizrOptions.BaseUriFactory?.Invoke();
            builder.ApizrOptions.BasePathFactory?.Invoke();
            builder.ApizrOptions.LogLevelsFactory?.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory?.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory?.Invoke();
            builder.ApizrOptions.RefitSettingsFactory?.Invoke();
            builder.ApizrOptions.OperationTimeoutFactory?.Invoke();
            builder.ApizrOptions.RequestTimeoutFactory?.Invoke();
            builder.ApizrOptions.ExceptionHandlersFactory?.Invoke();

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
            var isCrudApi = typeof(ICrudApi<,,,>).IsAssignableFromGenericType(webApiType);
            var crudApiEntityType = isCrudApi ? webApiType.GetGenericArguments().First() : null;
            var typeInfo = isCrudApi ? crudApiEntityType.GetTypeInfo() : webApiType.GetTypeInfo();

            var baseAddressAttribute = isCrudApi ? GetBaseAddressAttribute(crudApiEntityType) : GetBaseAddressAttribute(webApiType);
            if (!string.IsNullOrWhiteSpace(baseAddressAttribute?.BaseAddressOrPath))
            {
                if(Uri.IsWellFormedUriString(baseAddressAttribute.BaseAddressOrPath, UriKind.Absolute))
                    baseAddress = baseAddressAttribute.BaseAddressOrPath;
                else
                    basePath = baseAddressAttribute.BaseAddressOrPath;
            }

            var properDeclaringTypeAttributes = typeInfo.DeclaringType != null
                ? typeInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true)
                : typeInfo.GetTypeInfo().GetCustomAttributes(true);

            var properInheritedAttributes = typeInfo.DeclaringType != null
                ? typeInfo.DeclaringType.GetInterfaces().SelectMany(i => i.GetTypeInfo().GetCustomAttributes(true)).ToArray()
                : typeInfo.GetInterfaces().SelectMany(i => i.GetTypeInfo().GetCustomAttributes(true)).ToArray();

            var properAttributesAsc = properDeclaringTypeAttributes.Concat(properInheritedAttributes).ToList();
            var properAttributesDesc = properInheritedAttributes.Reverse().Concat(properDeclaringTypeAttributes).ToList();
            var commonAttributes = typeInfo.Assembly.GetCustomAttributes().ToList();

            var properParameterAttributes = isCrudApi
                ? properAttributesDesc.OfType<HandlerParameterAttribute>().Where(att => att is not CrudHandlerParameterAttribute).ToList()
                : properAttributesDesc.OfType<HandlerParameterAttribute>().ToList();
            var commonParameterAttributes = commonAttributes.OfType<HandlerParameterAttribute>().ToList();
            var properHeadersAttribute = properAttributesAsc.OfType<HeadersAttribute>().FirstOrDefault();
            var commonHeadersAttribute = commonAttributes.OfType<HeadersAttribute>().FirstOrDefault();
            var properLogAttribute = properAttributesAsc.OfType<LogAttribute>().FirstOrDefault();
            var commonLogAttribute = commonAttributes.OfType<LogAttribute>().FirstOrDefault();
            var properOperationTimeoutAttribute = properAttributesAsc.OfType<OperationTimeoutAttribute>().FirstOrDefault();
            var commonOperationTimeoutAttribute = commonAttributes.OfType<OperationTimeoutAttribute>().FirstOrDefault();
            var properRequestTimeoutAttribute = properAttributesAsc.OfType<RequestTimeoutAttribute>().FirstOrDefault();
            var commonRequestTimeoutAttribute = commonAttributes.OfType<RequestTimeoutAttribute>().FirstOrDefault();
            var properCacheAttribute = properAttributesAsc.OfType<CacheAttribute>().FirstOrDefault();
            var commonCacheAttribute = commonAttributes.OfType<CacheAttribute>().FirstOrDefault();
            var properResiliencePipelineAttributes = properAttributesDesc.OfType<ResiliencePipelineAttributeBase>().ToArray();
            var commonResiliencePipelineAttributes = commonAttributes.OfType<ResiliencePipelineAttributeBase>().ToArray();

            // Headers redaction
            var headers = (properHeadersAttribute?.Headers ?? [])
                .Concat(commonHeadersAttribute?.Headers ?? [])
                .ToList();
            var redactHeaders = new List<string>();
            if (headers.Any())
                foreach (var header in headers)
                    if(HttpRequestMessageExtensions.TryGetHeaderKeyValue(header, out var key, out var value) && value.StartsWith("*") && value.EndsWith("*"))
                        redactHeaders.Add(key);

            // Handlers parameters
            var handlersParameters = new Dictionary<string, object>();
            foreach (var commonParameterAttribute in commonParameterAttributes.Where(att => !string.IsNullOrWhiteSpace(att.Key)))
                handlersParameters[commonParameterAttribute.Key!] = commonParameterAttribute.Value;
            foreach (var commonOptionsHandlersParameter in commonOptions.HandlersParameters)
                handlersParameters[commonOptionsHandlersParameter.Key] = commonOptionsHandlersParameter.Value;
            foreach (var properParameterAttribute in properParameterAttributes.Where(att => !string.IsNullOrWhiteSpace(att.Key)))
                handlersParameters[properParameterAttribute.Key!] = properParameterAttribute.Value;

            // Resilience pipelines
            foreach (var commonResiliencePipelineAttribute in commonResiliencePipelineAttributes.Where(attribute => attribute is ResiliencePipelineAttribute))
                commonResiliencePipelineAttribute.RequestMethod = isCrudApi ? ApizrRequestMethod.AllCrud : ApizrRequestMethod.AllHttp;
            foreach (var properResiliencePipelineAttribute in properResiliencePipelineAttributes.Where(attribute => attribute is ResiliencePipelineAttribute))
                properResiliencePipelineAttribute.RequestMethod = isCrudApi ? ApizrRequestMethod.AllCrud : ApizrRequestMethod.AllHttp;

            var builder = new ApizrProperOptionsBuilder(new ApizrProperOptions(commonOptions, 
                webApiType,
                crudApiEntityType,
                typeInfo,
                baseAddress,
                basePath,
                handlersParameters,
                properLogAttribute?.HttpTracerMode ?? (commonOptions.HttpTracerMode != HttpTracerMode.Unspecified ? commonOptions.HttpTracerMode : commonLogAttribute?.HttpTracerMode),
                properLogAttribute?.TrafficVerbosity ?? (commonOptions.TrafficVerbosity != HttpMessageParts.Unspecified ? commonOptions.TrafficVerbosity : commonLogAttribute?.TrafficVerbosity),
                properOperationTimeoutAttribute?.Timeout ?? commonOperationTimeoutAttribute?.Timeout,
                properRequestTimeoutAttribute?.Timeout ?? commonRequestTimeoutAttribute?.Timeout,
                commonResiliencePipelineAttributes,
                properResiliencePipelineAttributes,
                commonCacheAttribute,
                properCacheAttribute,
                redactHeaders.Any() ? header => redactHeaders.Contains(header) : null,
                properLogAttribute?.LogLevels ?? (commonOptions.LogLevels?.Any() == true ? commonOptions.LogLevels : commonLogAttribute?.LogLevels))) as IApizrProperOptionsBuilder;

            if (commonOptions.ApizrConfigurationSection != null)
                builder.WithConfiguration(commonOptions.ApizrConfigurationSection);

            properOptionsBuilder?.Invoke(builder);

            builder.ApizrOptions.BaseUriFactory?.Invoke();
            builder.ApizrOptions.BaseAddressFactory?.Invoke();
            builder.ApizrOptions.BasePathFactory?.Invoke();
            builder.ApizrOptions.LogLevelsFactory?.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory?.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory?.Invoke();
            builder.ApizrOptions.OperationTimeoutFactory?.Invoke();
            builder.ApizrOptions.RequestTimeoutFactory?.Invoke();
            builder.ApizrOptions.ExceptionHandlersFactory?.Invoke();

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

        private static IApizrManagerOptions CreateManagerOptions(IApizrCommonOptions commonOptions, IApizrProperOptions properOptions, Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        {
            var builder = new ApizrManagerOptionsBuilder(new ApizrManagerOptions(commonOptions, properOptions)) as IApizrManagerOptionsBuilder;

            optionsBuilder?.Invoke(builder);

            if (builder.ApizrOptions.BaseUriFactory == null)
            {
                builder.ApizrOptions.BaseAddressFactory?.Invoke();
                builder.ApizrOptions.BasePathFactory?.Invoke();

                if(builder.ApizrOptions.BasePath?.Contains("\r") == true || 
                    builder.ApizrOptions.BasePath?.Contains("\n") == true)
                    throw new ArgumentException($"URL path {builder.ApizrOptions.BasePath} must not contain CR or LF characters");

                if (Uri.TryCreate(UrlHelper.Combine(builder.ApizrOptions.BaseAddress, builder.ApizrOptions.BasePath), UriKind.RelativeOrAbsolute, out var baseUri))
                    builder.WithBaseAddress(baseUri);
            }
            else if (builder.ApizrOptions.BasePathFactory != null)
            {
                builder.ApizrOptions.BaseUriFactory?.Invoke();
                builder.ApizrOptions.BasePathFactory?.Invoke();

                if (builder.ApizrOptions.BasePath?.Contains("\r") == true ||
                    builder.ApizrOptions.BasePath?.Contains("\n") == true)
                    throw new ArgumentException($"URL path {builder.ApizrOptions.BasePath} must not contain CR or LF characters");

                if (Uri.TryCreate(UrlHelper.Combine(builder.ApizrOptions.BaseUri?.ToString(), builder.ApizrOptions.BasePath), UriKind.RelativeOrAbsolute, out var baseUri))
                    builder.WithBaseAddress(baseUri);
            }

            builder.ApizrOptions.BaseUriFactory?.Invoke();
            builder.ApizrOptions.LogLevelsFactory?.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory?.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory?.Invoke();
            builder.ApizrOptions.RefitSettingsFactory?.Invoke();
            builder.ApizrOptions.LoggerFactory?.Invoke(builder.ApizrOptions.LoggerFactoryFactory.Invoke(), builder.ApizrOptions.WebApiType.GetFriendlyName());
            builder.ApizrOptions.OperationTimeoutFactory?.Invoke();
            builder.ApizrOptions.RequestTimeoutFactory?.Invoke();
            builder.ApizrOptions.ExceptionHandlersFactory?.Invoke();

            if (builder.ApizrOptions.HeadersFactories?.TryGetValue((ApizrRegistrationMode.Set, ApizrLifetimeScope.Api), out var setFactory) == true)
            {
                // Set api scoped headers right the way
                var setHeaders = setFactory.Invoke()?.ToArray();
                if(setHeaders?.Length > 0)
                    builder.WithHeaders(setHeaders, mode: ApizrRegistrationMode.Set);
            }
            if (builder.ApizrOptions.HeadersFactories?.TryGetValue((ApizrRegistrationMode.Store, ApizrLifetimeScope.Api), out var storeFactory) == true)
            {
                // Store api scoped headers for further attribute key match use
                var storeHeaders = storeFactory.Invoke()?.ToArray();
                if (storeHeaders?.Length > 0)
                    builder.WithHeaders(storeHeaders, mode: ApizrRegistrationMode.Store);
            }

            return builder.ApizrOptions;
        }

        #endregion

        #region Internal
        
        internal static BaseAddressAttribute GetBaseAddressAttribute(Type type)
        {
            var baseAddressAttribute = type.GetTypeInfo().GetCustomAttributes<BaseAddressAttribute>(true).FirstOrDefault();
            if (baseAddressAttribute != null || type.IsClass)
                return baseAddressAttribute;

            foreach (var parentInterface in type.GetInterfaces())
            {
                baseAddressAttribute = parentInterface.GetTypeInfo().GetCustomAttributes<BaseAddressAttribute>(true).FirstOrDefault();
                if (baseAddressAttribute != null)
                    return baseAddressAttribute;
            }

            return null;
        }

        #endregion
    }
}
