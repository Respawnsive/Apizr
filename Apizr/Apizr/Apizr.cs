using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Mapping;
using Apizr.Policing;
using Apizr.Requesting;
using HttpTracer;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Refit;

[assembly: Apizr.Preserve]
namespace Apizr
{
    public static class Apizr
    {
        #region Crud

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <see cref="T"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> CrudFor<T>(
            Action<IApizrOptionsBuilder> optionsBuilder = null) where T : class =>
            For<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), optionsBuilder);

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> CrudFor<T, TKey>(
            Action<IApizrOptionsBuilder> optionsBuilder = null) where T : class =>
            For<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), optionsBuilder);

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> CrudFor<T, TKey,
            TReadAllResult>(
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where T : class =>
            For<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), optionsBuilder);

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> CrudFor<T, TKey, TReadAllResult,
            TReadAllParams>(
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where T : class where TReadAllParams : class =>
            For<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), optionsBuilder);

        /// <summary>
        /// Create a <see cref="TApizrManager"/> instance for a managed crud api for <see cref="T"/> object (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
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
        public static TApizrManager CrudFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Func<ILazyWebApi<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
                IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrOptionsBase,
                TApizrManager> apizrManagerFactory,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where T : class
            where TReadAllParams : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            For(apizrManagerFactory, optionsBuilder);

        #endregion

        #region General

        /// <summary>
        /// Create a <see cref="ApizrManager{TWebApi}"/> instance
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IApizrManager<TWebApi> For<TWebApi>(
            Action<IApizrOptionsBuilder> optionsBuilder = null) =>
            For<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<TWebApi>(lazyWebApi, connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), optionsBuilder);

        /// <summary>
        /// Create a <see cref="TApizrManager"/> instance for a managed <see cref="TWebApi"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="apizrManagerFactory">The custom manager implementation instance factory</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static TApizrManager For<TWebApi, TApizrManager>(
            Func<ILazyWebApi<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
        where TApizrManager : IApizrManager<TWebApi>
        {
            var apizrOptions = CreateApizrOptions<TWebApi>(optionsBuilder);

            var httpHandlerFactory = new Func<HttpMessageHandler>(() =>
            {
                var httpClientHandler = apizrOptions.HttpClientHandlerFactory.Invoke();
                var logger = apizrOptions.LoggerFactory.Invoke().CreateLogger(apizrOptions.WebApiType.GetFriendlyName());

                apizrOptions.LogLevelFactory.Invoke();
                var handlerBuilder = new HttpHandlerBuilder(httpClientHandler, new HttpTracerLogWrapper(logger, apizrOptions));
                var httpTracerVerbosity = apizrOptions.TrafficVerbosityFactory.Invoke();
                handlerBuilder.HttpTracerHandler.Verbosity = httpTracerVerbosity;

                if (apizrOptions.PolicyRegistryKeys != null && apizrOptions.PolicyRegistryKeys.Any())
                {
                    var policyRegistry = apizrOptions.PolicyRegistryFactory.Invoke();
                    foreach (var policyRegistryKey in apizrOptions.PolicyRegistryKeys)
                    {
                        if (policyRegistry.TryGet<IsPolicy>(policyRegistryKey, out var registeredPolicy))
                        {
                            logger.LogTrace($"Global policies: Found a policy with key {policyRegistryKey}");
                            if (registeredPolicy is IAsyncPolicy<HttpResponseMessage> registeredPolicyForHttpResponseMessage)
                            {
                                var policySelector =
                                    new Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>>(
                                        request =>
                                        {
                                            var pollyContext = new Context().WithLogger(logger);
                                            request.SetPolicyExecutionContext(pollyContext);
                                            return registeredPolicyForHttpResponseMessage;
                                        });
                                handlerBuilder.AddHandler(new PolicyHttpMessageHandler(policySelector));

                                logger.LogTrace($"Global policies: Policy with key {policyRegistryKey} will be applied");
                            }
                            else
                            {
                                logger.LogTrace($"Global policies: Policy with key {policyRegistryKey} is not of {typeof(IAsyncPolicy<HttpResponseMessage>)} type and will be ignored");
                            }
                        }
                        else
                        {
                            logger.LogTrace($"Global policies: No policy found for key {policyRegistryKey}");
                        }
                    }
                }

                foreach (var delegatingHandlersFactory in apizrOptions.DelegatingHandlersFactories)
                    handlerBuilder.AddHandler(delegatingHandlersFactory.Invoke(logger, apizrOptions));

                var primaryMessageHandler = handlerBuilder.GetPrimaryHttpMessageHandler(logger);

                return primaryMessageHandler;
            });

            var webApiFactory = new Func<object>(() => RestService.For<TWebApi>(new HttpClient(httpHandlerFactory.Invoke(), false) { BaseAddress = apizrOptions.BaseAddressFactory.Invoke() }, apizrOptions.RefitSettingsFactory.Invoke()));
            var lazyWebApi = new LazyWebApi<TWebApi>(webApiFactory);
            var apizrManager = apizrManagerFactory(lazyWebApi, apizrOptions.ConnectivityHandlerFactory.Invoke(), apizrOptions.CacheHandlerFactory.Invoke(), apizrOptions.MappingHandlerFactory.Invoke(), apizrOptions.PolicyRegistryFactory.Invoke(), new ApizrOptions<TWebApi>(apizrOptions, apizrOptions.LoggerFactory.Invoke().CreateLogger(apizrOptions.WebApiType.GetFriendlyName())));

            return apizrManager;
        } 

        #endregion

        private static IApizrOptions CreateApizrOptions<TWebApi>(Action<IApizrOptionsBuilder> optionsBuilder = null)
        {
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

            if(logAttribute == null)
                logAttribute = webApiType.Assembly.GetCustomAttribute<LogAttribute>();

            var assemblyPolicyAttribute = webApiType.Assembly.GetCustomAttribute<PolicyAttribute>();

            var builder = new ApizrOptionsBuilder(new ApizrOptions(webApiType, baseAddress,
                logAttribute?.TrafficVerbosity, logAttribute?.LogLevel,
                assemblyPolicyAttribute?.RegistryKeys, webApiPolicyAttribute?.RegistryKeys));

            optionsBuilder?.Invoke(builder);

            return builder.ApizrOptions;
        }
    }
}
