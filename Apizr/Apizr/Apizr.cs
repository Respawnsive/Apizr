using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Policing;
using Apizr.Prioritizing;
using Apizr.Tracing;
using Fusillade;
using HttpTracer;
using Polly;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public static class Apizr
    {
        public static ApizrManager<TWebApi> For<TWebApi>(
            Action<IApizrOptionsBuilder> optionsBuilder = null) =>
            For<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApis, connectivityProvider, cacheProvider, policyRegistry) =>
                    new ApizrManager<TWebApi>(lazyWebApis, connectivityProvider, cacheProvider, policyRegistry), optionsBuilder);

        public static TApizrManager For<TWebApi, TApizrManager>(
            Func<IEnumerable<ILazyPrioritizedWebApi<TWebApi>>, IConnectivityProvider, ICacheProvider, IReadOnlyPolicyRegistry<string>,
                TApizrManager> apizrManagerFactory,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
        where TApizrManager : IApizrManager<TWebApi>
        {
            var apizrOptions = CreateApizrOptions<TWebApi>(optionsBuilder);
            var lazyWebApis = new List<ILazyPrioritizedWebApi<TWebApi>>();
            foreach (var priority in ((Priority[]) Enum.GetValues(typeof(Priority))).Where(x => x != Priority.Explicit))
            {
                var httpHandlerFactory = new Func<HttpMessageHandler>(() =>
                {
                    var handlerBuilder = new HttpHandlerBuilder(new HttpClientHandler
                    {
                        AutomaticDecompression = apizrOptions.DecompressionMethods
                    });
                    handlerBuilder.HttpTracerHandler.Verbosity = apizrOptions.HttpTracerVerbosity;

                    if (apizrOptions.PolicyRegistryKeys != null && apizrOptions.PolicyRegistryKeys.Any())
                    {
                        var registry = apizrOptions.PolicyRegistryFactory.Invoke();
                        foreach (var policyRegistryKey in apizrOptions.PolicyRegistryKeys)
                        {
                            var policy = registry.Get<IAsyncPolicy<HttpResponseMessage>>(policyRegistryKey);
                            handlerBuilder.AddHandler(new PolicyHttpMessageHandler(policy));
                        } 
                    }

                    foreach (var delegatingHandlersFactory in apizrOptions.DelegatingHandlersFactories)
                        handlerBuilder.AddHandler(delegatingHandlersFactory.Invoke());

                    var innerHandler = handlerBuilder.Build();
                    var primaryMessageHandler = new RateLimitedHttpMessageHandler(innerHandler, priority);

                    return primaryMessageHandler;
                });
                
                var webApiFactory = new Func<object>(() => RestService.For<TWebApi>(new HttpClient(httpHandlerFactory.Invoke()) { BaseAddress = apizrOptions.BaseAddress}, apizrOptions.RefitSettingsFactory.Invoke()));
                var lazyWebApi = Prioritize.For<TWebApi>(priority, webApiFactory);
                lazyWebApis.Add(lazyWebApi);
            }

            var apizrManager = apizrManagerFactory(lazyWebApis, apizrOptions.ConnectivityProviderFactory.Invoke(), apizrOptions.CacheProviderFactory.Invoke(), apizrOptions.PolicyRegistryFactory.Invoke());

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
