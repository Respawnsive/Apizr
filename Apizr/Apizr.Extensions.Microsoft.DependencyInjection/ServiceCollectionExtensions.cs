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
using Apizr.Requesting;
using Fusillade;
using HttpTracer;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using Refit;
using HttpRequestMessageExtensions = Apizr.Policing.HttpRequestMessageExtensions;

namespace Apizr
{
    public static class ServiceCollectionExtensions
    {
        #region Crud

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type, with key of type <see cref="TKey"/>
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddCrudApizr<T, TKey>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizr(services, typeof(ICrudApi<T, TKey>), typeof(ApizrManager<ICrudApi<T, TKey>>),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type, with key of type <see cref="TKey"/>
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddCrudApizr<T, TKey>(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizr(services, typeof(ICrudApi<T, TKey>), apizrManagerType,
                optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type, with key of type <see cref="crudedKeyType"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls</param>
        /// <param name="crudedKeyType">The object key type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddCrudApizr(this IServiceCollection services, Type crudedType,
            Type crudedKeyType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, typeof(ICrudApi<,>).MakeGenericType(crudedType, crudedKeyType),
                typeof(ApizrManager<>).MakeGenericType(typeof(ICrudApi<,>).MakeGenericType(crudedType, crudedKeyType)),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type, with key of type <see cref="crudedKeyType"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls</param>
        /// <param name="crudedKeyType">The object key type</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddCrudApizr(this IServiceCollection services, Type crudedType, Type crudedKeyType, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, typeof(ICrudApi<,>).MakeGenericType(crudedType, crudedKeyType), apizrManagerType,
                optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudAttribute"/> decorated classes and containing a <see cref="CrudKeyAttribute"/> decorated property
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddCrudApizr(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddCrudApizr(services, typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudAttribute"/> decorated classes and containing a <see cref="CrudKeyAttribute"/> decorated property
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddCrudApizr(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies) =>
            AddCrudApizr(services, typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudAttribute"/> decorated classes and containing a <see cref="CrudKeyAttribute"/> decorated property
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddCrudApizr(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddCrudApizr(services, apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudAttribute"/> decorated classes and containing a <see cref="CrudKeyAttribute"/> decorated property
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddCrudApizr(this IServiceCollection services, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies)
        {
            if (!assemblies.Any())
                throw new ArgumentException(
                    "No assemblies found to scan. Supply at least one assembly to scan for Crud attribute.");

            var assembliesToScan = assemblies.Distinct();

            var cruds = new Dictionary<Type, CrudAttribute>();

            foreach (var assemblyToScan in assembliesToScan)
            {
                foreach (var type in assemblyToScan.GetTypes())
                {
                    var crudAttribute = type.GetCustomAttribute<CrudAttribute>();
                    if (crudAttribute != null)
                    {
                        cruds.Add(type, crudAttribute);
                    }
                }
            }

            foreach (var crud in cruds)
            {
                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.WithBaseAddress(crud.Value.BaseUri);
                else
                    optionsBuilder += builder => builder.WithBaseAddress(crud.Value.BaseUri);

                AddApizr(services, typeof(ICrudApi<,>).MakeGenericType(crud.Key, crud.Value.KeyType),
                    apizrManagerType.MakeGenericType(typeof(ICrudApi<,>).MakeGenericType(crud.Key, crud.Value.KeyType)),
                    optionsBuilder);
            }

            return services;
        }

        #endregion

        #region General

        /// <summary>
        /// Register <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizr<TWebApi>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, typeof(TWebApi), typeof(ApizrManager<TWebApi>),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizr<TWebApi, TApizrManager>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi> =>
            AddApizr(services, typeof(TWebApi), typeof(TApizrManager),
                optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizr(this IServiceCollection services, Type webApiType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizr(
            this IServiceCollection services, Type webApiType, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            if (!typeof(IApizrManager<>).MakeGenericType(webApiType).IsAssignableFrom(apizrManagerType))
                throw new ArgumentException(
                    $"Your Apizr manager class must inherit from IApizrManager generic interface or derived");

            var apizrOptions = CreateApizrExtendedOptions(webApiType, apizrManagerType, optionsBuilder);
            foreach (var priority in ((Priority[])Enum.GetValues(typeof(Priority))).Where(x => x != Priority.Explicit).OrderByDescending(priority => priority))
            {
                var builder = services.AddHttpClient(ForType(apizrOptions.WebApiType, priority))
                    .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                    {
                        var logHandler = serviceProvider.GetRequiredService<ILogHandler>();
                        var handlerBuilder = new HttpHandlerBuilder(new HttpClientHandler
                        {
                            AutomaticDecompression = apizrOptions.DecompressionMethods
                        }, new HttpTracerLogWrapper(logHandler));
                        handlerBuilder.HttpTracerHandler.Verbosity = apizrOptions.HttpTracerVerbosity;

                        if (apizrOptions.PolicyRegistryKeys != null && apizrOptions.PolicyRegistryKeys.Any())
                        {
                            IReadOnlyPolicyRegistry<string> policyRegistry = null;
                            try
                            {
                                policyRegistry = serviceProvider.GetRequiredService<IReadOnlyPolicyRegistry<string>>();
                            }
                            catch (Exception)
                            {
                                logHandler.Write(
                                    $"Apizr - Global policies: You get some global policies but didn't register a {nameof(PolicyRegistry)} instance. Global policies will be ignored for  for {webApiType.Name} {priority} instance");
                            }

                            if (policyRegistry != null)
                            {
                                foreach (var policyRegistryKey in apizrOptions.PolicyRegistryKeys)
                                {
                                    if (policyRegistry.TryGet<IsPolicy>(policyRegistryKey, out var registeredPolicy))
                                    {
                                        logHandler.Write($"Apizr - Global policies: Found a policy with key {policyRegistryKey} for {webApiType.Name} {priority} instance");
                                        if (registeredPolicy is IAsyncPolicy<HttpResponseMessage> registeredPolicyForHttpResponseMessage)
                                        {
                                            var policySelector =
                                                new Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>>(
                                                    request =>
                                                    {
                                                        var pollyContext = new Context().WithLogHandler(logHandler);
                                                        HttpRequestMessageExtensions.SetPolicyExecutionContext(request, pollyContext);
                                                        return registeredPolicyForHttpResponseMessage;
                                                    });
                                            handlerBuilder.AddHandler(new PolicyHttpMessageHandler(policySelector));

                                            logHandler.Write($"Apizr - Global policies: Policy with key {policyRegistryKey} will be applied to {webApiType.Name} {priority} instance");
                                        }
                                        else
                                        {
                                            logHandler.Write($"Apizr - Global policies: Policy with key {policyRegistryKey} is not of {typeof(IAsyncPolicy<HttpResponseMessage>)} type and will be ignored for {webApiType.Name} {priority} instance");
                                        }
                                    }
                                    else
                                    {
                                        logHandler.Write($"Apizr - Global policies: No policy found for key {policyRegistryKey} and will be ignored for  for {webApiType.Name} {priority} instance");
                                    }
                                }
                            }
                        }

                        foreach (var delegatingHandlerExtendedFactory in apizrOptions.DelegatingHandlersExtendedFactories)
                            handlerBuilder.AddHandler(delegatingHandlerExtendedFactory.Invoke(serviceProvider));

                        var innerHandler = handlerBuilder.Build();
                        var primaryMessageHandler = new RateLimitedHttpMessageHandler(innerHandler, priority);

                        return primaryMessageHandler;
                    })
                    .AddTypedClient(typeof(ILazyPrioritizedWebApi<>).MakeGenericType(apizrOptions.WebApiType),
                        (client, serviceProvider) =>
                            Prioritize.TypeFor(apizrOptions.WebApiType, priority)
                                .GetConstructor(new[] { typeof(Func<object>) })
                                ?.Invoke(new object[]
                                {
                                    new Func<object>(() => RestService.For(apizrOptions.WebApiType, client,
                                        apizrOptions.RefitSettingsFactory(serviceProvider)))
                                }));

                if (apizrOptions.BaseAddress != null)
                    builder.ConfigureHttpClient(x => x.BaseAddress = apizrOptions.BaseAddress);

                apizrOptions.HttpClientBuilder?.Invoke(builder);

                builder.ConfigureHttpClient(x =>
                {
                    if (x.BaseAddress == null)
                        throw new ArgumentNullException(nameof(x.BaseAddress), $"You must provide a valid web api uri with the {nameof(WebApiAttribute)} or the options builder");
                });
            }

            services.AddSingleton(typeof(IConnectivityHandler), apizrOptions.ConnectivityHandlerType);

            services.AddSingleton(typeof(ICacheHandler), apizrOptions.CacheHandlerType);

            services.AddSingleton(typeof(ILogHandler), apizrOptions.LogHandlerType);

            services.AddSingleton(typeof(IApizrManager<>).MakeGenericType(apizrOptions.WebApiType), typeof(ApizrManager<>).MakeGenericType(apizrOptions.WebApiType));

            return services;
        }

        #endregion

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
