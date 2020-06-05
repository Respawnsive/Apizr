using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Lazying;
using Apizr.Policing;
using Apizr.Tracing;
using Fusillade;
using HttpTracer;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public static class Apizr
    {
        public static ApizrManager<TWebApi> For<TWebApi>(Func<IPolicyRegistry<string>> policyRegistryFactory = null,
            Action<IApizrOptionsBuilder> optionsBuilder = null) =>
            For<TWebApi, VoidConnectivityProvider, VoidCacheProvider, ApizrManager<TWebApi>>(
                () => new VoidConnectivityProvider(), () => new VoidCacheProvider(),
                (lazyWebApis, connectivityProvider, cacheProvider, policyRegistry) =>
                    new ApizrManager<TWebApi>(lazyWebApis, connectivityProvider, cacheProvider, policyRegistry),
                policyRegistryFactory, optionsBuilder);

        public static TApizrManager For<TWebApi, TConnectivityProvider, TCacheProvider, TApizrManager>(
            Func<TConnectivityProvider> connectivityProviderFactory, 
            Func<TCacheProvider> cacheProviderFactory,
            Func<IEnumerable<ILazyDependency<TWebApi>>, IConnectivityProvider, ICacheProvider, IPolicyRegistry<string>,
                TApizrManager> apizrManagerFactory,
            Func<IPolicyRegistry<string>> policyRegistryFactory = null,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
        where TConnectivityProvider : IConnectivityProvider
        where TCacheProvider : ICacheProvider
        where TApizrManager : IApizrManager<TWebApi>
        {
            var apizrOptions = CreateApizrOptions<TWebApi>(optionsBuilder);
            var lazyWebApis = new List<ILazyDependency<TWebApi>>();
            foreach (var priority in ((Priority[]) Enum.GetValues(typeof(Priority))).Where(x => x != Priority.Explicit))
            {
                var primaryMessageHandler = new RateLimitedHttpMessageHandler(
                    new HttpTracerHandler(
                        new HttpClientHandler
                        {
                            AutomaticDecompression = apizrOptions.DecompressionMethods
                        }, apizrOptions.HttpTracerVerbosity), priority);

                var webApiFactory = new Func<object>(() => RestService.For<TWebApi>(new HttpClient(primaryMessageHandler) { BaseAddress = apizrOptions.BaseAddress}, apizrOptions.RefitSettingsFactory.Invoke()));
                var lazyWebApi = new LazyDependency<TWebApi>(webApiFactory);
                lazyWebApis.Add(lazyWebApi);
            }

            var apizrManager = apizrManagerFactory(lazyWebApis, connectivityProviderFactory.Invoke(), cacheProviderFactory.Invoke(), policyRegistryFactory?.Invoke() ?? new PolicyRegistry());

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
