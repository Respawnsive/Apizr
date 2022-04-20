using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Registry;
using Apizr.Connecting;
using Apizr.Extending;
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
    public static class ApizrBuilder
    {
        #region Registry

        /// <summary>
        /// Create a registry with all managers built with both common and proper options
        /// </summary>
        /// <param name="registryBuilder">The registry builder with access to proper options</param>
        /// <param name="commonOptionsBuilder">The common options shared by all managers</param>
        /// <returns></returns>
        public static IApizrRegistry CreateRegistry(Action<IApizrRegistryBuilder> registryBuilder, Action<IApizrCommonOptionsBuilder> commonOptionsBuilder = null)
        {
            if (registryBuilder == null)
                throw new ArgumentNullException(nameof(registryBuilder));

            var commonOptions = CreateCommonOptions(commonOptionsBuilder);

            var apizrRegistry = CreateRegistry(commonOptions, registryBuilder);

            return apizrRegistry;
        }

        #endregion

        #region Crud


        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> CreateCrudManagerFor<T>(
            Action<IApizrOptionsBuilder> optionsBuilder = null) where T : class =>
            CreateManagerFor<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey>(
            Action<IApizrOptionsBuilder> optionsBuilder = null) where T : class =>
            CreateManagerFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey,
            TReadAllResult>(
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where T : class =>
            CreateManagerFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> CreateCrudManagerFor<T, TKey, TReadAllResult,
            TReadAllParams>(
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where T : class where TReadAllParams : class =>
            CreateManagerFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <summary>
        /// Create a <typeparamref name="TApizrManager"/> instance for a managed crud api for <typeparamref name="T"/> object (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{ICrudApi}"/> implementation</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <param name="apizrManagerFactory">The custom manager implementation instance factory</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static TApizrManager CreateCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Func<ILazyFactory<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
                IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrOptionsBase,
                TApizrManager> apizrManagerFactory,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where T : class
            where TReadAllParams : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            CreateManagerFor(apizrManagerFactory, CreateCommonOptions(), optionsBuilder);

        #endregion

        #region General

        /// <summary>
        /// Create a <see cref="ApizrManager{TWebApi}"/> instance
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<TWebApi> CreateManagerFor<TWebApi>(
            Action<IApizrOptionsBuilder> optionsBuilder = null) =>
            CreateManagerFor<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<TWebApi>(lazyWebApi, connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), CreateCommonOptions(), optionsBuilder);

        /// <summary>
        /// Create a <typeparamref name="TApizrManager"/> instance for a managed <typeparamref name="TWebApi"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="apizrManagerFactory">The custom manager implementation instance factory</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                IReadOnlyPolicyRegistry<string>, IApizrOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi> =>
            CreateManagerFor(apizrManagerFactory, CreateCommonOptions(), optionsBuilder);

        private static TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                IReadOnlyPolicyRegistry<string>, IApizrOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            IApizrCommonOptions commonOptions,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi>
            => CreateManagerFor(apizrManagerFactory, commonOptions, CreateProperOptions<TWebApi>(commonOptions),
                optionsBuilder);


        internal static TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            IApizrCommonOptions commonOptions,
            IApizrProperOptions properOptions,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
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
                                        var context = request.GetOrBuildPolicyExecutionContext();
                                        if (!context.TryGetLogger(out var contextLogger, out var logLevels, out var verbosity, out var tracerMode))
                                        {
                                            contextLogger = apizrOptions.Logger;
                                            logLevels = apizrOptions.LogLevels;
                                            verbosity = apizrOptions.TrafficVerbosity;
                                            tracerMode = apizrOptions.HttpTracerMode;

                                            context.WithLogger(contextLogger, logLevels, verbosity, tracerMode);
                                            request.SetPolicyExecutionContext(context);
                                        }

                                        contextLogger.Log(logLevels.Low(), $"{context.OperationKey}: Policy with key {policyRegistryKey} will be applied");

                                        return registeredPolicyForHttpResponseMessage;
                                    });
                            handlerBuilder.AddHandler(new PolicyHttpMessageHandler(policySelector));
                        }
                    }
                }

                foreach (var delegatingHandlersFactory in apizrOptions.DelegatingHandlersFactories)
                    handlerBuilder.AddHandler(delegatingHandlersFactory.Invoke(apizrOptions.Logger, apizrOptions));

                var primaryMessageHandler = handlerBuilder.GetPrimaryHttpMessageHandler(apizrOptions.Logger, apizrOptions);

                return primaryMessageHandler;
            });

            var webApiFactory = new Func<object>(() => RestService.For<TWebApi>(new HttpClient(httpHandlerFactory.Invoke(), false) { BaseAddress = apizrOptions.BaseAddress }, apizrOptions.RefitSettings));
            var lazyWebApi = new LazyFactory<TWebApi>(webApiFactory);
            var apizrManager = apizrManagerFactory(lazyWebApi, apizrOptions.ConnectivityHandlerFactory.Invoke(),
                apizrOptions.GetCacheHanderFactory()?.Invoke() ?? apizrOptions.CacheHandlerFactory.Invoke(),
                apizrOptions.GetMappingHanderFactory()?.Invoke() ?? apizrOptions.MappingHandlerFactory.Invoke(), 
                apizrOptions.PolicyRegistryFactory.Invoke(), new ApizrOptions<TWebApi>(apizrOptions));

            return apizrManager;
        }

        #endregion

        #region Builder

        private static IApizrCommonOptions CreateCommonOptions(
            Action<IApizrCommonOptionsBuilder> commonOptionsBuilder = null)
        {
            var builder = new ApizrCommonOptionsBuilder(new ApizrCommonOptions());

            commonOptionsBuilder?.Invoke(builder);

            builder.ApizrOptions.LogLevelsFactory.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory.Invoke();
            builder.ApizrOptions.RefitSettingsFactory.Invoke();

            return builder.ApizrOptions;
        }

        internal static IApizrProperOptions CreateProperOptions<TWebApi>(IApizrCommonOptions commonOptions,
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
        {
            if (commonOptions == null)
                throw new ArgumentNullException(nameof(commonOptions));

            var webApiType = typeof(TWebApi);

            var webApiAttribute = webApiType.GetTypeInfo().GetCustomAttribute<WebApiAttribute>(true);
            Uri.TryCreate(webApiAttribute?.BaseUri, UriKind.RelativeOrAbsolute, out var baseAddress);

            LogAttribute logAttribute;
            PolicyAttribute webApiPolicyAttribute;
            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(webApiType))
            {
                var modelType = webApiType.GetGenericArguments().First();
                logAttribute = modelType.GetTypeInfo().GetCustomAttribute<LogAttribute>(true);
                webApiPolicyAttribute = modelType.GetTypeInfo().GetCustomAttribute<PolicyAttribute>(true);
            }
            else
            {
                logAttribute = webApiType.GetTypeInfo().GetCustomAttribute<LogAttribute>(true);
                webApiPolicyAttribute = webApiType.GetTypeInfo().GetCustomAttribute<PolicyAttribute>(true);
            }

            if (logAttribute == null)
                logAttribute = webApiType.Assembly.GetCustomAttribute<LogAttribute>();

            var assemblyPolicyAttribute = webApiType.Assembly.GetCustomAttribute<PolicyAttribute>();

            var builder = new ApizrProperOptionsBuilder(new ApizrProperOptions(commonOptions, webApiType, baseAddress,
                assemblyPolicyAttribute?.RegistryKeys, webApiPolicyAttribute?.RegistryKeys,
                logAttribute?.HttpTracerMode,
                logAttribute?.TrafficVerbosity, logAttribute?.LogLevels));

            properOptionsBuilder?.Invoke(builder);

            builder.ApizrOptions.BaseAddressFactory.Invoke();
            builder.ApizrOptions.LogLevelsFactory.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory.Invoke();

            return builder.ApizrOptions;
        }

        private static IApizrRegistry CreateRegistry(IApizrCommonOptions commonOptions, Action<IApizrRegistryBuilder> registryBuilder)
        {
            if (commonOptions == null)
                throw new ArgumentNullException(nameof(commonOptions));

            if (registryBuilder == null)
                throw new ArgumentNullException(nameof(registryBuilder));

            var builder = new ApizrRegistryBuilder(commonOptions);

            registryBuilder.Invoke(builder);

            return builder.ApizrRegistry;
        }

        private static IApizrOptions CreateOptions(IApizrCommonOptions commonOptions, IApizrProperOptions properOptions, Action<IApizrOptionsBuilder> optionsBuilder = null)
        {
            var builder = new ApizrOptionsBuilder(new ApizrOptions(commonOptions, properOptions));

            optionsBuilder?.Invoke(builder);

            builder.ApizrOptions.BaseAddressFactory.Invoke();
            builder.ApizrOptions.LogLevelsFactory.Invoke();
            builder.ApizrOptions.TrafficVerbosityFactory.Invoke();
            builder.ApizrOptions.HttpTracerModeFactory.Invoke();
            builder.ApizrOptions.RefitSettingsFactory.Invoke();
            builder.ApizrOptions.LoggerFactory.Invoke(builder.ApizrOptions.LoggerFactoryFactory.Invoke(), builder.ApizrOptions.WebApiType.GetFriendlyName());

            return builder.ApizrOptions;
        } 

        #endregion
    }
}
