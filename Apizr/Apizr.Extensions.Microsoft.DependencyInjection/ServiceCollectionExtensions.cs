using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Extending.Configuring;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Helping;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Mapping;
using Apizr.Policing;
using Apizr.Requesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Refit;
using HttpRequestMessageExtensions = Apizr.Policing.HttpRequestMessageExtensions;

[assembly: Apizr.Preserve]
namespace Apizr
{
    /// <summary>
    /// Apizr service collection extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Registry

        /// <summary>
        /// Create a registry with all managers built with both common and proper options
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="registryBuilder">The registry builder with access to proper options</param>
        /// <param name="optionsBuilder">The common options shared by all managers</param>
        /// <returns></returns>
        public static IServiceCollection AddApizr(this IServiceCollection services,
            Action<IApizrExtendedRegistryBuilder> registryBuilder,
            Action<IApizrExtendedCommonOptionsBuilder> optionsBuilder = null)
        {
            if (registryBuilder == null)
                throw new ArgumentNullException(nameof(registryBuilder));

            var commonOptions = CreateCommonOptions(optionsBuilder);

            var apizrRegistry = (ApizrExtendedRegistry)CreateRegistry(services, commonOptions, registryBuilder);

            services.AddSingleton(serviceProvider => apizrRegistry.GetInstance(serviceProvider));

            return services;
        }

        #endregion

        #region Crud

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type (class),
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor<T>(this IServiceCollection services,Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor<T, TKey>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(TKey), typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor<T, TKey, TReadAllResult>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where T : class =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where T : class =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type, 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{ICrudApi}"/> implementation</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(TApizrManager), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type crudedType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, crudedType, typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <paramref name="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, crudedType, crudedKeyType, typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <paramref name="crudedKeyType"/> (primitive) and "ReadAll" query result of type <paramref name="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, crudedType, crudedType, crudedReadAllResultType, typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <paramref name="crudedKeyType"/> (primitive) and "ReadAll" query result of type <paramref name="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="crudedReadAllParamsType">ReadAll query parameters type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type crudedReadAllParamsType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, crudedType, crudedType, crudedReadAllResultType, crudedReadAllParamsType, typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <paramref name="crudedKeyType"/> (primitive) and "ReadAll" query result of type <paramref name="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls</param>
        /// <param name="crudedKeyType">The object key type</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="crudedReadAllParamsType">ReadAll query parameters type</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type crudedReadAllParamsType, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            if (!crudedType.GetTypeInfo().IsClass)
                throw new ArgumentException($"{crudedType.Name} is not a class", nameof(crudedType));

            var crudAttribute = crudedType.GetCustomAttribute<CrudEntityAttribute>();
            if (crudAttribute != null)
            {
                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.WithBaseAddress(crudAttribute.BaseUri);
                else
                    optionsBuilder += builder => builder.WithBaseAddress(crudAttribute.BaseUri);
            }

            Type modelEntityType = null;
            if (typeof(MappedEntity<,>).IsAssignableFromGenericType(crudedType))
            {
                var entityTypes = crudedType.GetGenericArguments();

                modelEntityType = entityTypes[0];
                crudedType = entityTypes[1];
            }
            else
            {
                modelEntityType = crudedType;
            }

            if (!crudedKeyType.GetTypeInfo().IsPrimitive)
                throw new ArgumentException($"{crudedKeyType.Name} is not primitive", nameof(crudedKeyType));

            if ((!typeof(IEnumerable<>).IsAssignableFromGenericType(crudedReadAllResultType) &&
                !crudedReadAllResultType.IsClass) || !crudedReadAllResultType.IsGenericType)
                throw new ArgumentException(
                    $"{crudedReadAllResultType.Name} must inherit from {typeof(IEnumerable<>)} or be a generic class", nameof(crudedReadAllResultType));

            if (!typeof(IDictionary<string, object>).IsAssignableFrom(crudedReadAllParamsType) &&
                !crudedReadAllParamsType.IsClass)
                throw new ArgumentException(
                    $"{crudedReadAllParamsType.Name} must inherit from {typeof(IDictionary<string, object>)} or be a class", nameof(crudedReadAllParamsType));

            if (!typeof(IApizrManager<>).IsAssignableFromGenericType(apizrManagerType))
                throw new ArgumentException(
                    $"{apizrManagerType} must inherit from {typeof(IApizrManager<>)}", nameof(apizrManagerType));

            crudAttribute = new CrudEntityAttribute(crudAttribute?.BaseUri, crudedKeyType, crudedReadAllResultType, crudedReadAllParamsType, modelEntityType);
            if (optionsBuilder == null)
                optionsBuilder = builder => builder.ApizrOptions.CrudEntities.Add(crudedType, crudAttribute);
            else
                optionsBuilder += builder => builder.ApizrOptions.CrudEntities.Add(crudedType, crudAttribute);

            var readAllResultType = crudedReadAllResultType.MakeGenericTypeIfNeeded(crudedType);

            return AddApizrManagerFor(services,
                typeof(ICrudApi<,,,>).MakeGenericType(crudedType, crudedKeyType,
                    readAllResultType, crudedReadAllParamsType),
                apizrManagerType.MakeGenericTypeIfNeeded(typeof(ICrudApi<,,,>).MakeGenericType(crudedType, crudedKeyType,
                    readAllResultType, crudedReadAllParamsType)),
                CreateCommonOptions(), optionsBuilder);
        }

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
            AddApizrCrudManagerFor(services, typeof(ApizrManager<>), null,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, params Assembly[] assemblies) =>
            AddApizrCrudManagerFor(services, typeof(ApizrManager<>), null,
                assemblies);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddApizrCrudManagerFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies) =>
            AddApizrCrudManagerFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apizrManagerType, params Type[] assemblyMarkerTypes) =>
            AddApizrCrudManagerFor(services, apizrManagerType, null,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apizrManagerType, params Assembly[] assemblies) =>
            AddApizrCrudManagerFor(services, apizrManagerType, null,
                assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddApizrCrudManagerFor(services, apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies)
        {
            if (!assemblies.Any())
                throw new ArgumentException(
                    $"No assemblies found to scan. Supply at least one assembly to scan for {nameof(CrudEntityAttribute)}.", nameof(assemblies));

            var assembliesToScan = assemblies.Distinct().ToList();

            var cruds = new Dictionary<Type, CrudEntityAttribute>();

            var modelEntityTypes = assembliesToScan
                .SelectMany(assembly => assembly.GetTypes().Where(t =>
                    t.IsClass && !t.IsAbstract && t.GetCustomAttribute<MappedCrudEntityAttribute>() != null))
                .ToDictionary(t => t.GetCustomAttribute<MappedCrudEntityAttribute>().MappedEntityType,
                    t => t.GetCustomAttribute<MappedCrudEntityAttribute>().ToCrudEntityAttribute(t));

            var apiEntityTypes = assembliesToScan
                .SelectMany(assembly => assembly.GetTypes().Where(t =>
                    t.IsClass && !t.IsAbstract && t.GetCustomAttribute<CrudEntityAttribute>() != null &&
                    !modelEntityTypes.ContainsKey(t)))
                .ToDictionary(t => t, t => t.GetCustomAttribute<CrudEntityAttribute>());

            var crudEntityDefinitions = modelEntityTypes.Concat(apiEntityTypes).ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault(y => y.MappedEntityType != null) ?? x.First());

            foreach (var crudEntityDefinition in crudEntityDefinitions)
            {
                if (crudEntityDefinition.Value.MappedEntityType == null)
                    crudEntityDefinition.Value.MappedEntityType = crudEntityDefinition.Key;

                cruds.Add(crudEntityDefinition.Key, crudEntityDefinition.Value);

                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.ApizrOptions.CrudEntities.Add(crudEntityDefinition.Key, crudEntityDefinition.Value);
                else
                    optionsBuilder += builder => builder.ApizrOptions.CrudEntities.Add(crudEntityDefinition.Key, crudEntityDefinition.Value);
            }

            foreach (var crud in cruds)
            {
                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.WithBaseAddress(crud.Value.BaseUri);
                else
                    optionsBuilder += builder => builder.WithBaseAddress(crud.Value.BaseUri);

                var readAllResultType = crud.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crud.Key);

                AddApizrManagerFor(services,
                    typeof(ICrudApi<,,,>).MakeGenericType(crud.Key, crud.Value.KeyType,
                        readAllResultType, crud.Value.ReadAllParamsType),
                    apizrManagerType.MakeGenericType(typeof(ICrudApi<,,,>).MakeGenericType(crud.Key, crud.Value.KeyType,
                        readAllResultType, crud.Value.ReadAllParamsType)),
                    CreateCommonOptions(), optionsBuilder);
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
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor<TWebApi>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrManagerFor(services, typeof(TWebApi), typeof(ApizrManager<TWebApi>),
                CreateCommonOptions(), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor<TWebApi, TApizrManager>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi> =>
            AddApizrManagerFor(services, typeof(TWebApi), typeof(TApizrManager),
                CreateCommonOptions(), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(this IServiceCollection services, Type webApiType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrManagerFor(services, webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType),
                CreateCommonOptions(), optionsBuilder);

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddApizrManagerFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies) =>
            AddApizrManagerFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddApizrManagerFor(services, apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies)
        {
            if (!assemblies.Any())
                throw new ArgumentException(
                    $"No assemblies found to scan. Supply at least one assembly to scan for {nameof(WebApiAttribute)}.", nameof(assemblies));

            var assembliesToScan = assemblies.Distinct().ToList();

            var objectMappingDefinitions = assembliesToScan
                .SelectMany(assembly => assembly.GetTypes().Where(t =>
                    t.IsClass && t.GetCustomAttribute<MappedWithAttribute>() != null))
                .ToDictionary(t => t, t => t.GetCustomAttribute<MappedWithAttribute>());

            foreach (var objectMappingDefinition in objectMappingDefinitions)
            {
                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.ApizrOptions.ObjectMappings.Add(objectMappingDefinition.Key, objectMappingDefinition.Value);
                else
                    optionsBuilder += builder => builder.ApizrOptions.ObjectMappings.Add(objectMappingDefinition.Key, objectMappingDefinition.Value);
            }

            var webApiTypes = assembliesToScan
                .SelectMany(assembly => assembly.GetTypes().Where(t =>
                    !t.IsClass && t.GetCustomAttribute<WebApiAttribute>()?.IsAutoRegistrable == true))
                .ToList();

            foreach (var webApiType in webApiTypes)
                AddApizrManagerFor(services, webApiType, apizrManagerType.MakeGenericType(webApiType), CreateCommonOptions(), optionsBuilder);

            return services;
        }

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(
            this IServiceCollection services, Type webApiType, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            => AddApizrManagerFor(services, webApiType, apizrManagerType, CreateCommonOptions(), optionsBuilder);

        private static IServiceCollection AddApizrManagerFor(
            this IServiceCollection services, Type webApiType, Type apizrManagerType,
            IApizrExtendedCommonOptions commonOptions,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            AddApizrManagerFor(services, commonOptions,
                CreateProperOptions(commonOptions, webApiType, apizrManagerType), optionsBuilder);

            return services;
        }

        internal static Type AddApizrManagerFor(
            this IServiceCollection services,
            IApizrExtendedCommonOptions commonOptions,
            IApizrExtendedProperOptions properOptions, 
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            if (!typeof(IApizrManager<>).MakeGenericType(properOptions.WebApiType).IsAssignableFrom(properOptions.ApizrManagerType))
                throw new ArgumentException(
                    $"Your Apizr manager class must inherit from IApizrManager generic interface or derived");
            
            var webApiFriendlyName = properOptions.WebApiType.GetFriendlyName();
            var apizrOptionsBuilder = CreateOptions(commonOptions, properOptions, optionsBuilder);
            var apizrOptions = apizrOptionsBuilder.ApizrOptions;
            var apizrOptionsRegistrationType = typeof(IApizrOptions<>).MakeGenericType(apizrOptions.WebApiType);

            var builder = services.AddHttpClient(ForType(apizrOptions.WebApiType))
                .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                {
                    var options = (IApizrExtendedOptionsBase)serviceProvider.GetRequiredService(apizrOptionsRegistrationType);
                    var handlerBuilder = new ExtendedHttpHandlerBuilder(options.HttpClientHandler, options);

                    if (options.PolicyRegistryKeys != null && options.PolicyRegistryKeys.Any())
                    {
                        IReadOnlyPolicyRegistry<string> policyRegistry = null;
                        try
                        {
                            policyRegistry = serviceProvider.GetRequiredService<IReadOnlyPolicyRegistry<string>>();
                        }
                        catch (Exception)
                        {
                            options.Logger.Log(options.LogLevels.Low(),
                                $"Global policies: You get some global policies but didn't register a {nameof(PolicyRegistry)} instance. Global policies will be ignored for  for {webApiFriendlyName} instance");
                        }

                        if (policyRegistry != null)
                        {
                            foreach (var policyRegistryKey in options.PolicyRegistryKeys)
                            {
                                if (policyRegistry.TryGet<IsPolicy>(policyRegistryKey, out var registeredPolicy))
                                {
                                    if (registeredPolicy is IAsyncPolicy<HttpResponseMessage> registeredPolicyForHttpResponseMessage)
                                    {
                                        var policySelector =
                                            new Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>>(
                                                request =>
                                                {
                                                    var context = request.GetOrBuildPolicyExecutionContext();
                                                    if (!context.TryGetLogger(out var contextLogger, out var logLevels, out var verbosity, out var tracerMode))
                                                    {
                                                        contextLogger = options.Logger;
                                                        logLevels = options.LogLevels;
                                                        verbosity = options.TrafficVerbosity;
                                                        tracerMode = options.HttpTracerMode;

                                                        context.WithLogger(contextLogger, logLevels, verbosity, tracerMode);
                                                        HttpRequestMessageExtensions.SetPolicyExecutionContext(request, context);
                                                    }

                                                    contextLogger.Log(logLevels.Low(), $"{context.OperationKey}: Policy with key {policyRegistryKey} will be applied");

                                                    return registeredPolicyForHttpResponseMessage;
                                                });
                                        handlerBuilder.AddHandler(new PolicyHttpMessageHandler(policySelector));
                                    }
                                }
                            }
                        }
                    }

                    foreach (var delegatingHandlerExtendedFactory in options.DelegatingHandlersExtendedFactories)
                        handlerBuilder.AddHandler(delegatingHandlerExtendedFactory.Invoke(serviceProvider, options));

                    var primaryMessageHandler = handlerBuilder.GetPrimaryHttpMessageHandler(options.Logger, options);

                    return primaryMessageHandler;
                })
                .AddTypedClient(typeof(ILazyFactory<>).MakeGenericType(apizrOptions.WebApiType),
                    (client, serviceProvider) =>
                    {
                        var options = (IApizrExtendedOptionsBase)serviceProvider.GetRequiredService(apizrOptionsRegistrationType);

                        if (client.BaseAddress == null)
                        {
                            client.BaseAddress = options.BaseUri;
                        }

                        return typeof(LazyFactory<>).MakeGenericType(options.WebApiType)
                            .GetConstructor(new[] { typeof(Func<object>) })
                            ?.Invoke(new object[]
                            {
                                new Func<object>(() => RestService.For(options.WebApiType, client,
                                    options.RefitSettings))
                            });
                    });

            apizrOptions.HttpClientBuilder?.Invoke(builder);

            if (apizrOptions.ConnectivityHandlerFactory != null)
                services.AddOrReplaceSingleton(typeof(IConnectivityHandler), apizrOptions.ConnectivityHandlerFactory);
            else if (apizrOptions.ConnectivityHandlerType == typeof(DefaultConnectivityHandler))
                services.TryAddSingleton(typeof(IConnectivityHandler), _ => new DefaultConnectivityHandler(() => true));
            else
                services.TryAddSingleton(typeof(IConnectivityHandler), apizrOptions.ConnectivityHandlerType);

            var cacheHandlerFactory = apizrOptions.GetCacheHanderFactory();
            if (cacheHandlerFactory != null)
                services.AddOrReplaceSingleton(typeof(ICacheHandler), _ => cacheHandlerFactory.Invoke());
            else
                services.TryAddSingleton(typeof(ICacheHandler), apizrOptions.CacheHandlerType);

            var mappingHandlerType = apizrOptions.GetMappingHanderType();
            if (mappingHandlerType != null)
                services.AddOrReplaceSingleton(typeof(IMappingHandler), mappingHandlerType);
            else 
                services.TryAddSingleton(typeof(IMappingHandler), apizrOptions.MappingHandlerType);

            services.TryAddSingleton(apizrOptionsRegistrationType, serviceProvider =>
            {
                if (apizrOptions.BaseUriFactory == null)
                {
                    apizrOptions.BaseAddressFactory?.Invoke(serviceProvider);
                    apizrOptions.BasePathFactory?.Invoke(serviceProvider);
                    if (Uri.TryCreate(UrlHelper.Combine(apizrOptions.BaseAddress, apizrOptions.BasePath), UriKind.RelativeOrAbsolute, out var baseUri))
                        apizrOptionsBuilder.WithBaseAddress(baseUri);
                }
                else if (apizrOptions.BasePathFactory != null)
                {
                    apizrOptions.BaseUriFactory?.Invoke(serviceProvider);
                    apizrOptions.BasePathFactory?.Invoke(serviceProvider);
                    if (Uri.TryCreate(UrlHelper.Combine(apizrOptions.BaseUri.ToString(), apizrOptions.BasePath), UriKind.RelativeOrAbsolute, out var baseUri))
                        apizrOptionsBuilder.WithBaseAddress(baseUri);
                }

                apizrOptions.BaseUriFactory?.Invoke(serviceProvider);
                apizrOptions.LogLevelsFactory.Invoke(serviceProvider);
                apizrOptions.TrafficVerbosityFactory.Invoke(serviceProvider);
                apizrOptions.HttpTracerModeFactory.Invoke(serviceProvider);
                apizrOptions.RefitSettingsFactory.Invoke(serviceProvider);
                apizrOptions.HttpClientHandlerFactory.Invoke(serviceProvider);
                apizrOptions.LoggerFactory.Invoke(serviceProvider, webApiFriendlyName);

                return Activator.CreateInstance(typeof(ApizrExtendedOptions<>).MakeGenericType(apizrOptions.WebApiType), apizrOptions);
            });

            services.TryAddSingleton(serviceProvider => ((IApizrOptionsBase)serviceProvider.GetRequiredService(apizrOptionsRegistrationType)).RefitSettings.ContentSerializer);

            var serviceType = typeof(IApizrManager<>).MakeGenericType(apizrOptions.WebApiType);
            services.TryAddSingleton(serviceType, apizrOptions.ApizrManagerType);

            foreach (var postRegistrationAction in apizrOptions.PostRegistrationActions)
            {
                postRegistrationAction.Invoke(properOptions.WebApiType, services);
            }

            return serviceType;
        }

        #endregion

        #region Builder

        internal static IApizrExtendedRegistry CreateRegistry(this IServiceCollection services, Action<IApizrExtendedRegistryBuilder> registryBuilder,
            IApizrExtendedCommonOptions baseCommonOptions,
            Action<IApizrExtendedCommonOptionsBuilder> commonOptionsBuilder = null, ApizrExtendedRegistry mainRegistry = null)
        {
            if (registryBuilder == null)
                throw new ArgumentNullException(nameof(registryBuilder));

            var commonOptions = CreateCommonOptions(commonOptionsBuilder, baseCommonOptions);

            var apizrRegistry = CreateRegistry(services, commonOptions, registryBuilder, mainRegistry);

            return apizrRegistry;
        }

        internal static IApizrExtendedRegistry CreateRegistry(this IServiceCollection services, IApizrExtendedCommonOptions commonOptions, Action<IApizrExtendedRegistryBuilder> registryBuilder, ApizrExtendedRegistry mainRegistry = null)
        {
            if (commonOptions == null)
                throw new ArgumentNullException(nameof(commonOptions));

            if (registryBuilder == null)
                throw new ArgumentNullException(nameof(registryBuilder));

            var builder = new ApizrExtendedRegistryBuilder(services, commonOptions, mainRegistry);

            registryBuilder.Invoke(builder);

            return builder.ApizrRegistry;
        }

        private static IApizrExtendedCommonOptions CreateCommonOptions(
            Action<IApizrExtendedCommonOptionsBuilder> commonOptionsBuilder = null, IApizrExtendedCommonOptions baseCommonOptions = null)
        {
            if (baseCommonOptions is not ApizrExtendedCommonOptions baseApizrCommonOptions)
                baseApizrCommonOptions = new ApizrExtendedCommonOptions();

            var builder = new ApizrExtendedCommonOptionsBuilder(baseApizrCommonOptions);

            commonOptionsBuilder?.Invoke(builder);

            return builder.ApizrOptions;
        }

        internal static IApizrExtendedProperOptions CreateProperOptions(IApizrExtendedCommonOptions commonOptions, Type webApiType, Type apizrManagerType,
            Action<IApizrExtendedProperOptionsBuilder> properOptionsBuilder = null)
        {
            if (commonOptions == null)
                throw new ArgumentNullException(nameof(commonOptions));

            string baseAddress = null;
            string basePath = null;
            var webApiAttribute = webApiType.GetTypeInfo().GetCustomAttribute<WebApiAttribute>(true);
            if (webApiAttribute != null)
            {
                if (!string.IsNullOrWhiteSpace(webApiAttribute.BaseAddressOrPath))
                {
                    if (Uri.IsWellFormedUriString(webApiAttribute.BaseAddressOrPath, UriKind.Absolute))
                        baseAddress = webApiAttribute.BaseAddressOrPath;
                    else
                        basePath = webApiAttribute.BaseAddressOrPath;
                }
                commonOptions.WebApis.Add(webApiType, webApiAttribute);
            }

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

            var builder = new ApizrExtendedProperOptionsBuilder(new ApizrExtendedProperOptions(commonOptions, webApiType, apizrManagerType,
                assemblyPolicyAttribute?.RegistryKeys, webApiPolicyAttribute?.RegistryKeys, 
                baseAddress,
                basePath,
                logAttribute?.HttpTracerMode,
                logAttribute?.TrafficVerbosity, logAttribute?.LogLevels));

            properOptionsBuilder?.Invoke(builder);

            return builder.ApizrOptions;
        }

        private static IApizrExtendedOptionsBuilder CreateOptions(IApizrExtendedCommonOptions commonOptions,
            IApizrExtendedProperOptions properOptions, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            var builder = new ApizrExtendedOptionsBuilder(new ApizrExtendedOptions(commonOptions, properOptions));

            optionsBuilder?.Invoke(builder);

            return builder;
        } 

        #endregion

        private static string ForType(Type refitInterfaceType)
        {
            string typeName;

            if (refitInterfaceType.IsNested)
            {
                var className = "AutoGenerated" + refitInterfaceType.DeclaringType.Name + refitInterfaceType.Name;
                typeName = refitInterfaceType.AssemblyQualifiedName.Replace(refitInterfaceType.DeclaringType.FullName + "+" + refitInterfaceType.Name, refitInterfaceType.Namespace + "." + className);
            }
            else
            {
                var className = "AutoGenerated" + refitInterfaceType.Name;

                if (refitInterfaceType.Namespace == null)
                {
                    className = $"{className}.{className}";
                }

                typeName = refitInterfaceType.AssemblyQualifiedName.Replace(refitInterfaceType.Name, className);
            }

            return typeName;
        }

        private static IServiceCollection AddOrReplaceSingleton(this IServiceCollection services, Type serviceType, Type implementationType)
        {
            var serviceDescriptors = services.Where(sd => sd.ServiceType == serviceType).ToList();
            if (serviceDescriptors.Any())
            {
                if(serviceDescriptors.Count > 1)
                    throw new Exception("Can't replace more than one registration");

                var serviceDescriptor = serviceDescriptors.First();
                if(serviceDescriptor.ImplementationType != implementationType)
                    services.Replace(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
            }
            else
                services.AddSingleton(serviceType, implementationType);

            return services;
        }

        private static IServiceCollection AddOrReplaceSingleton(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> factory)
        {
            var serviceDescriptors = services.Where(sd => sd.ServiceType == serviceType).ToList();
            if (serviceDescriptors.Any())
            {
                if (serviceDescriptors.Count > 1)
                    throw new Exception("Can't replace more than one registration");

                var serviceDescriptor = serviceDescriptors.First();
                if (serviceDescriptor.ImplementationFactory?.Method.ReturnType != factory.Method.ReturnType)
                    services.Replace(new ServiceDescriptor(serviceType, factory, ServiceLifetime.Singleton));
            }
            else
                services.AddSingleton(serviceType, factory);

            return services;
        }
    }
}
