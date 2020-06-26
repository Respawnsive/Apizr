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
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class),
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizrCrudFor<T>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            UseApizrCrudFor(services, typeof(T), typeof(int), typeof(IEnumerable<>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type, 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizrCrudFor<T>(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            UseApizrCrudFor(services, typeof(T), typeof(int), typeof(IEnumerable<>), apizrManagerType, optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizrCrudFor<T, TKey>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            UseApizrCrudFor(services, typeof(T), typeof(TKey), typeof(IEnumerable<>), typeof(ApizrManager<>), optionsBuilder);

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
        public static bool UseApizrCrudFor<T, TKey>(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            UseApizrCrudFor(services, typeof(T), typeof(TKey), typeof(IEnumerable<>), apizrManagerType, optionsBuilder);

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
        public static bool UseApizrCrudFor<T, TKey, TReadAllResult>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where T : class
            where TReadAllResult : IPagedResult<T> =>
            UseApizrCrudFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(ApizrManager<>), optionsBuilder);

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
        public static bool UseApizrCrudFor<T, TKey, TReadAllResult>(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where T : class
            where TReadAllResult : IPagedResult<T> =>
            UseApizrCrudFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), apizrManagerType, optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizrCrudFor(this IServiceCollection services, Type crudedType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizrCrudFor(services, crudedType, typeof(int), typeof(IEnumerable<>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizrCrudFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizrCrudFor(services, crudedType, crudedKeyType, typeof(IEnumerable<>), typeof(ApizrManager<>), optionsBuilder);

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
        public static bool UseApizrCrudFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizrCrudFor(services, crudedType, crudedType, crudedReadAllResultType, typeof(ApizrManager<>), optionsBuilder);

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
        public static bool UseApizrCrudFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            services.AddApizrCrudFor(crudedType, crudedKeyType, crudedReadAllResultType, apizrManagerType, optionsBuilder);

            CheckHandlers(services);

            return true;
        }

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static bool UseApizrCrudFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            UseApizrCrudFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static bool UseApizrCrudFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies) =>
            UseApizrCrudFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static bool UseApizrCrudFor(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            UseApizrCrudFor(services, apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static bool UseApizrCrudFor(this IServiceCollection services, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies)
        {
            services.AddApizrCrudFor(apizrManagerType, optionsBuilder, assemblies);

            CheckHandlers(services);

            return true;
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
        public static bool UseApizrFor<TWebApi>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizrFor(services, typeof(TWebApi), typeof(ApizrManager<TWebApi>),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizrFor<TWebApi, TApizrManager>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi> =>
            UseApizrFor(services, typeof(TWebApi), typeof(TApizrManager),
                optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizrFor(this IServiceCollection services, Type webApiType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizrFor(services, webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType),
                optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static bool UseApizrFor(
            this IServiceCollection services, Type webApiType, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            services.AddApizrFor(webApiType, apizrManagerType, optionsBuilder);

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