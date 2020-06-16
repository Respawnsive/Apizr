using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Policing;
using Apizr.Prioritizing;
using Fusillade;
using HttpTracer;
using Polly;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public static class Apizr
    {
        /// <summary>
        /// Create a <see cref="ApizrManager{TWebApi}"/> instance
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static ApizrManager<TWebApi> For<TWebApi>(
            Action<IApizrOptionsBuilder> optionsBuilder = null) =>
            For<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApis, connectivityHandler, cacheHandler, logHandler, policyRegistry) =>
                    new ApizrManager<TWebApi>(lazyWebApis, connectivityHandler, cacheHandler, logHandler, policyRegistry), optionsBuilder);

        /// <summary>
        /// Create a <see cref="TApizrManager"/> instance for a managed <see cref="TWebApi"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="apizrManagerFactory">The custom manager implementation instance factory</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static TApizrManager For<TWebApi, TApizrManager>(
            Func<IEnumerable<ILazyPrioritizedWebApi<TWebApi>>, IConnectivityHandler, ICacheHandler, ILogHandler, IReadOnlyPolicyRegistry<string>,
                TApizrManager> apizrManagerFactory,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
        where TApizrManager : IApizrManager<TWebApi>
        {
            var apizrOptions = CreateApizrOptions<TWebApi>(optionsBuilder);
            var lazyWebApis = new List<ILazyPrioritizedWebApi<TWebApi>>();
            foreach (var priority in ((Priority[])Enum.GetValues(typeof(Priority))).Where(x => x != Priority.Explicit))
            {
                var httpHandlerFactory = new Func<HttpMessageHandler>(() =>
                {
                    var logHandler = apizrOptions.LogHandlerFactory.Invoke();
                    var handlerBuilder = new HttpHandlerBuilder(new HttpClientHandler
                    {
                        AutomaticDecompression = apizrOptions.DecompressionMethods
                    }, new HttpTracerLogWrapper(logHandler));
                    handlerBuilder.HttpTracerHandler.Verbosity = apizrOptions.HttpTracerVerbosity;

                    if (apizrOptions.PolicyRegistryKeys != null && apizrOptions.PolicyRegistryKeys.Any())
                    {
                        var policyRegistry = apizrOptions.PolicyRegistryFactory.Invoke();
                        foreach (var policyRegistryKey in apizrOptions.PolicyRegistryKeys)
                        {
                            if (policyRegistry.TryGet<IsPolicy>(policyRegistryKey, out var registeredPolicy))
                            {
                                logHandler.Write($"Apizr - Global policies: Found a policy with key {policyRegistryKey}");
                                if (registeredPolicy is IAsyncPolicy<HttpResponseMessage> registeredPolicyForHttpResponseMessage)
                                {
                                    var policySelector =
                                        new Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>>(
                                            request =>
                                            {
                                                var pollyContext = new Context().WithLogHandler(logHandler);
                                                request.SetPolicyExecutionContext(pollyContext);
                                                return registeredPolicyForHttpResponseMessage;
                                            });
                                    handlerBuilder.AddHandler(new PolicyHttpMessageHandler(policySelector));

                                    logHandler.Write($"Apizr - Global policies: Policy with key {policyRegistryKey} will be applied");
                                }
                                else
                                {
                                    logHandler.Write($"Apizr - Global policies: Policy with key {policyRegistryKey} is not of {typeof(IAsyncPolicy<HttpResponseMessage>)} type and will be ignored");
                                }
                            }
                            else
                            {
                                logHandler.Write($"Apizr - Global policies: No policy found for key {policyRegistryKey}");
                            }
                        }
                    }

                    foreach (var delegatingHandlersFactory in apizrOptions.DelegatingHandlersFactories)
                        handlerBuilder.AddHandler(delegatingHandlersFactory.Invoke(logHandler));

                    var innerHandler = handlerBuilder.Build();
                    var primaryMessageHandler = new RateLimitedHttpMessageHandler(innerHandler, priority);

                    return primaryMessageHandler;
                });

                var webApiFactory = new Func<object>(() => RestService.For<TWebApi>(new HttpClient(httpHandlerFactory.Invoke()) { BaseAddress = apizrOptions.BaseAddress }, apizrOptions.RefitSettingsFactory.Invoke()));
                var lazyWebApi = Prioritize.For<TWebApi>(priority, webApiFactory);
                lazyWebApis.Add(lazyWebApi);
            }

            var apizrManager = apizrManagerFactory(lazyWebApis, apizrOptions.ConnectivityHandlerFactory.Invoke(), apizrOptions.CacheHandlerFactory.Invoke(), apizrOptions.LogHandlerFactory.Invoke(), apizrOptions.PolicyRegistryFactory.Invoke());

            return apizrManager;
        }

        private static IApizrOptions CreateApizrOptions<TWebApi>(Action<IApizrOptionsBuilder> optionsBuilder = null)
        {
            var webApiType = typeof(TWebApi);

            var webApiAttribute = webApiType.GetTypeInfo().GetCustomAttribute<WebApiAttribute>(true);
            Uri.TryCreate(webApiAttribute?.BaseUri, UriKind.RelativeOrAbsolute, out var baseAddress);

            var traceAttribute = webApiType.GetTypeInfo().GetCustomAttribute<TraceAttribute>(true);

            var assemblyPolicyAttribute = webApiType.Assembly.GetCustomAttribute<PolicyAttribute>();

            var webApiPolicyAttribute = webApiType.GetTypeInfo().GetCustomAttribute<PolicyAttribute>(true);

            var builder = new ApizrOptionsBuilder(new ApizrOptions(webApiType, baseAddress,
                webApiAttribute?.DecompressionMethods, traceAttribute?.Verbosity, assemblyPolicyAttribute?.RegistryKeys,
                webApiPolicyAttribute?.RegistryKeys));

            optionsBuilder?.Invoke(builder);

            return builder.ApizrOptions;
        }
    }
}
