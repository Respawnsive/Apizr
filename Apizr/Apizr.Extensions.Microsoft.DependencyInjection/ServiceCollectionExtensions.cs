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
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class),
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudFor(services, typeof(T), typeof(int), typeof(IEnumerable<>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type, 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T>(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudFor(services, typeof(T), typeof(int), typeof(IEnumerable<>), apizrManagerType, optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T, TKey>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudFor(services, typeof(T), typeof(TKey), typeof(IEnumerable<>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T, TKey>(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudFor(services, typeof(T), typeof(TKey), typeof(IEnumerable<>), apizrManagerType, optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or <see cref="IPagedResult{T}"/>)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or <see cref="IPagedResult{T}"/>)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T, TKey, TReadAllResult>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) 
            where T : class
            where TReadAllResult : IPagedResult<T> =>
            AddApizrCrudFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type, 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or <see cref="IPagedResult{T}"/>)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or <see cref="IPagedResult{T}"/>)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T, TKey, TReadAllResult>(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) 
            where T : class
            where TReadAllResult : IPagedResult<T> =>
            AddApizrCrudFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), apizrManagerType, optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type crudedType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudFor(services, crudedType, typeof(int), typeof(IEnumerable<>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudFor(services, crudedType, crudedKeyType, typeof(IEnumerable<>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or <see cref="IPagedResult{T}"/>)
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or <see cref="IPagedResult{T}"/>)</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudFor(services, crudedType, crudedType, crudedReadAllResultType, typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type, with key of type <see cref="crudedKeyType"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls</param>
        /// <param name="crudedKeyType">The object key type</param>
        /// <param name="crudedReadAllResultType"></param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            if (!crudedType.GetTypeInfo().IsClass)
                throw new ArgumentException($"{crudedType.Name} is not a class", nameof(crudedType));

            if (!crudedKeyType.GetTypeInfo().IsPrimitive)
                throw new ArgumentException($"{crudedKeyType.Name} is not primitive", nameof(crudedKeyType));

            if (!typeof(IEnumerable<>).IsAssignableFromGenericType(crudedReadAllResultType) &&
                !typeof(IPagedResult<>).IsAssignableFromGenericType(crudedReadAllResultType))
                throw new ArgumentException(
                    $"{crudedReadAllResultType.Name} must inherit from {typeof(IEnumerable<>)} or {typeof(IPagedResult<>)}", nameof(crudedReadAllResultType));

            if (!typeof(IApizrManager<>).IsAssignableFromGenericType(apizrManagerType))
                throw new ArgumentException(
                    $"{apizrManagerType} must inherit from {typeof(IApizrManager<>)}", nameof(apizrManagerType));

            return AddApizrFor(services,
                typeof(ICrudApi<,,>).MakeGenericType(crudedType, crudedKeyType,
                    crudedReadAllResultType.MakeGenericTypeIfNeeded(crudedType)),
                apizrManagerType.MakeGenericTypeIfNeeded(typeof(ICrudApi<,,>).MakeGenericType(crudedType, crudedKeyType,
                    crudedReadAllResultType.MakeGenericTypeIfNeeded(crudedType))),
                optionsBuilder);
        }

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddApizrCrudFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies) =>
            AddApizrCrudFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddApizrCrudFor(services, apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies)
        {
            if (!assemblies.Any())
                throw new ArgumentException(
                    $"No assemblies found to scan. Supply at least one assembly to scan for {nameof(CrudEntityAttribute)}.", nameof(assemblies));

            var assembliesToScan = assemblies.Distinct();

            var cruds = new Dictionary<Type, CrudEntityAttribute>();

            foreach (var assemblyToScan in assembliesToScan)
            {
                foreach (var type in assemblyToScan.GetTypes())
                {
                    var crudAttribute = type.GetCustomAttribute<CrudEntityAttribute>();
                    if (crudAttribute != null)
                    {
                        cruds.Add(type, crudAttribute);

                        if (optionsBuilder == null)
                            optionsBuilder = builder => builder.ApizrOptions.CrudEntities.Add(type, crudAttribute);
                        else
                            optionsBuilder += builder => builder.ApizrOptions.CrudEntities.Add(type, crudAttribute);
                    }
                }
            }

            foreach (var crud in cruds)
            {
                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.WithBaseAddress(crud.Value.BaseUri);
                else
                    optionsBuilder += builder => builder.WithBaseAddress(crud.Value.BaseUri);

                AddApizrFor(services,
                    typeof(ICrudApi<,,>).MakeGenericType(crud.Key, crud.Value.KeyType,
                        crud.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crud.Key)),
                    apizrManagerType.MakeGenericType(typeof(ICrudApi<,,>).MakeGenericType(crud.Key, crud.Value.KeyType,
                        crud.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crud.Key))),
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
        public static IServiceCollection AddApizrFor<TWebApi>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrFor(services, typeof(TWebApi), typeof(ApizrManager<TWebApi>),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrFor<TWebApi, TApizrManager>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi> =>
            AddApizrFor(services, typeof(TWebApi), typeof(TApizrManager),
                optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrFor(this IServiceCollection services, Type webApiType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrFor(services, webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrFor(
            this IServiceCollection services, Type webApiType, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            if (!typeof(IApizrManager<>).MakeGenericType(webApiType).IsAssignableFrom(apizrManagerType))
                throw new ArgumentException(
                    $"Your Apizr manager class must inherit from IApizrManager generic interface or derived");

            var webApiFriendlyName = webApiType.GetFriendlyName();
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
                                    $"Apizr - Global policies: You get some global policies but didn't register a {nameof(PolicyRegistry)} instance. Global policies will be ignored for  for {webApiFriendlyName} {priority} instance");
                            }

                            if (policyRegistry != null)
                            {
                                foreach (var policyRegistryKey in apizrOptions.PolicyRegistryKeys)
                                {
                                    if (policyRegistry.TryGet<IsPolicy>(policyRegistryKey, out var registeredPolicy))
                                    {
                                        logHandler.Write($"Apizr - Global policies: Found a policy with key {policyRegistryKey} for {webApiFriendlyName} {priority} instance");
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

                                            logHandler.Write($"Apizr - Global policies: Policy with key {policyRegistryKey} will be applied to {webApiFriendlyName} {priority} instance");
                                        }
                                        else
                                        {
                                            logHandler.Write($"Apizr - Global policies: Policy with key {policyRegistryKey} is not of {typeof(IAsyncPolicy<HttpResponseMessage>)} type and will be ignored for {webApiType.GetFriendlyName()} {priority} instance");
                                        }
                                    }
                                    else
                                    {
                                        logHandler.Write($"Apizr - Global policies: No policy found for key {policyRegistryKey} and will be ignored for  for {webApiFriendlyName} {priority} instance");
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

            foreach (var postRegistrationAction in apizrOptions.PostRegistrationActions)
            {
                postRegistrationAction.Invoke(services);
            }

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
