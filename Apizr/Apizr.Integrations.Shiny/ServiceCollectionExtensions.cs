using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Requesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        public static bool UseCrudApizr<T, TKey>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            UseApizr(services, typeof(ICrudApi<T, TKey>), typeof(ApizrManager<ICrudApi<T, TKey>>),
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
        public static bool UseCrudApizr<T, TKey>(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            UseApizr(services, typeof(ICrudApi<T, TKey>), apizrManagerType,
                optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type, with key of type <see cref="crudedKeyType"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls</param>
        /// <param name="crudedKeyType">The object key type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseCrudApizr(this IServiceCollection services, Type crudedType,
            Type crudedKeyType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizr(services, typeof(ICrudApi<,>).MakeGenericType(crudedType, crudedKeyType),
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
        public static bool UseCrudApizr(this IServiceCollection services, Type crudedType, Type crudedKeyType, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizr(services, typeof(ICrudApi<,>).MakeGenericType(crudedType, crudedKeyType), apizrManagerType,
                optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudAttribute"/> decorated classes and containing a <see cref="CrudKeyAttribute"/> decorated property
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudAttribute"/></param>
        /// <returns></returns>
        public static bool UseCrudApizr(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            UseCrudApizr(services, typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudAttribute"/> decorated classes and containing a <see cref="CrudKeyAttribute"/> decorated property
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudAttribute"/></param>
        /// <returns></returns>
        public static bool UseCrudApizr(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies) =>
            UseCrudApizr(services, typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudAttribute"/> decorated classes and containing a <see cref="CrudKeyAttribute"/> decorated property
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudAttribute"/></param>
        /// <returns></returns>
        public static bool UseCrudApizr(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            UseCrudApizr(services, apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudAttribute"/> decorated classes and containing a <see cref="CrudKeyAttribute"/> decorated property
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudAttribute"/></param>
        /// <returns></returns>
        public static bool UseCrudApizr(this IServiceCollection services, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies)
        {
            services.AddCrudApizr(apizrManagerType, optionsBuilder, assemblies);

            CheckHandlers(services);

            return true;
        }


        #endregion

        #region General

        /// <summary>
        /// Register <see cref="IApizrManager{TWebApi}"/>, replacing handlers by Shiny Connectivity, Cache and Log services (if void or default type)
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizr<TWebApi>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizr(services, typeof(TWebApi), typeof(ApizrManager<TWebApi>),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizr<TWebApi, TApizrManager>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi> =>
            UseApizr(services, typeof(TWebApi), typeof(TApizrManager),
                optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{webApiType}"/>, replacing handlers by Shiny Connectivity, Cache and Log services (if void or default type)
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizr(this IServiceCollection services, Type webApiType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizr(services, webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/>, replacing handlers by Shiny Connectivity, Cache and Log services (if void or default type)
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizr(
            this IServiceCollection services, Type webApiType, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            services.AddApizr(webApiType, apizrManagerType, optionsBuilder);

            CheckHandlers(services);

            return true;
        } 

        #endregion

        private static void CheckHandlers(IServiceCollection services)
        {
            var isVoidConnectivityHandlerRegistered = services.Any(x => x.ImplementationType == typeof(VoidConnectivityHandler));
            if (isVoidConnectivityHandlerRegistered)
                services.Replace(new ServiceDescriptor(typeof(IConnectivityHandler), typeof(ShinyConnectivityHandler), ServiceLifetime.Singleton));

            var isVoidCacheHandlerRegistered = services.Any(x => x.ImplementationType == typeof(VoidCacheHandler));
            if (isVoidCacheHandlerRegistered)
                services.Replace(new ServiceDescriptor(typeof(ICacheHandler), typeof(ShinyCacheHandler), ServiceLifetime.Singleton));

            var isDefaultLogHandlerRegistered = services.Any(x => x.ImplementationType == typeof(DefaultLogHandler));
            if (isDefaultLogHandlerRegistered)
                services.Replace(new ServiceDescriptor(typeof(ILogHandler), typeof(ShinyLogHandler), ServiceLifetime.Singleton));
        }
    }
}