using System;
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
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Apizr
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApizr<TWebApi>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, typeof(TWebApi), typeof(ApizrManager<TWebApi>),
                optionsBuilder);
        public static IServiceCollection AddApizr(this IServiceCollection services, Type webApiType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType),
                optionsBuilder);

        public static IServiceCollection AddApizr(
            this IServiceCollection services, Type webApiType, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            if (!typeof(IApizrManager<>).MakeGenericType(webApiType).IsAssignableFrom(apizrManagerType))
                throw new ArgumentException(
                    $"Your Apizr manager class must inherit from IApizrManager generic interface or derived");

            var apizrOptions = CreateApizrExtendedOptions(webApiType, apizrManagerType, optionsBuilder);
            foreach (var priority in ((Priority[])Enum.GetValues(typeof(Priority))).Where(x => x != Priority.Explicit))
            {
                var builder = services.AddHttpClient(ForType(apizrOptions.WebApiType, priority))
                    .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                    {
                        var handlerBuilder = new HttpHandlerBuilder(new HttpClientHandler
                        {
                            AutomaticDecompression = apizrOptions.DecompressionMethods
                        });
                        handlerBuilder.HttpTracerHandler.Verbosity = apizrOptions.HttpTracerVerbosity;

                        foreach (var handlerExtendedFactory in apizrOptions.DelegatingHandlersExtendedFactories)
                            handlerBuilder.AddHandler(handlerExtendedFactory.Invoke(serviceProvider));

                        var innerHandler = handlerBuilder.Build();
                        var primaryMessageHandler = new RateLimitedHttpMessageHandler(innerHandler, priority);

                        return primaryMessageHandler;
                    })
                    .AddTypedClient(typeof(ILazyPrioritizedWebApi<>).MakeGenericType(apizrOptions.WebApiType),
                        (client, serviceProvider) =>
                            Prioritize.TypeFor(apizrOptions.WebApiType, priority)
                                .GetConstructor(new[] {typeof(Func<object>)})
                                ?.Invoke(new object[]
                                {
                                    new Func<object>(() => RestService.For(apizrOptions.WebApiType, client,
                                        apizrOptions.RefitSettingsFactory()))
                                }));

                if (apizrOptions.BaseAddress != null)
                    builder.ConfigureHttpClient(x => x.BaseAddress = apizrOptions.BaseAddress);

                apizrOptions.HttpClientBuilder?.Invoke(builder);

                builder.ConfigureHttpClient(x =>
                {
                    if (x.BaseAddress == null)
                        throw new ArgumentNullException(nameof(x.BaseAddress), $"You must provide a valid web api uri with the {nameof(WebApiAttribute)} or the options builder");
                });

                foreach (var policyRegistryKey in apizrOptions.PolicyRegistryKeys)
                    builder.AddPolicyHandlerFromRegistry(policyRegistryKey);
            }

            services.AddSingleton(typeof(IConnectivityProvider), apizrOptions.ConnectivityProviderType);

            services.AddSingleton(typeof(ICacheProvider), apizrOptions.CacheProviderType);

            services.AddSingleton(typeof(IApizrManager<>).MakeGenericType(apizrOptions.WebApiType), typeof(ApizrManager<>).MakeGenericType(apizrOptions.WebApiType));

            return services;
        }

        private static IApizrExtendedOptions CreateApizrExtendedOptions(Type webApiType, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            var webApiAttribute = webApiType.GetTypeInfo().GetCustomAttribute<WebApiAttribute>(true);
            Uri.TryCreate(webApiAttribute?.BaseUri, UriKind.RelativeOrAbsolute, out var baseAddress);

            var traceAttribute = webApiType.GetTypeInfo().GetCustomAttribute<TraceAttribute>(true);

            var assemblyPolicyAttribute = webApiType.Assembly.GetCustomAttribute<PolicyAttribute>();

            var webApiPolicyAttribute = webApiType.GetTypeInfo().GetCustomAttribute<PolicyAttribute>(true);

            var builder = new ApizrExtendedOptionsBuilder(new ApizrExtendedOptions(webApiType, apizrManagerType, baseAddress,
                webApiAttribute?.DecompressionMethods, traceAttribute?.Verbosity, assemblyPolicyAttribute?.RegistryKeys,
                webApiPolicyAttribute?.RegistryKeys));

            optionsBuilder?.Invoke(builder);

            return builder.ApizrOptions;
        }

        private static string ForType(Type refitInterfaceType, Priority priority)
        {
            string typeName;

            if (refitInterfaceType.IsNested)
            {
                var className = "AutoGenerated" + priority + refitInterfaceType.DeclaringType.Name + refitInterfaceType.Name;
                typeName = refitInterfaceType.AssemblyQualifiedName.Replace(refitInterfaceType.DeclaringType.FullName + "+" + refitInterfaceType.Name, refitInterfaceType.Namespace + "." + className);
            }
            else
            {
                var className = "AutoGenerated" + priority + refitInterfaceType.Name;

                if (refitInterfaceType.Namespace == null)
                {
                    className = $"{className}.{className}";
                }

                typeName = refitInterfaceType.AssemblyQualifiedName.Replace(refitInterfaceType.Name, className);
            }

            return typeName;
        }
    }
}
