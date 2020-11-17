using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Policing;
using Apizr.Prioritizing;
using Apizr.Requesting;
using Fusillade;
using HttpTracer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Registry;
using Refit;
using HttpRequestMessageExtensions = Apizr.Policing.HttpRequestMessageExtensions;

[assembly: Apizr.Preserve]
namespace Apizr
{
    public static class ServiceCollectionExtensions
    {
        #region Crud

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class),
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudFor(services, typeof(T), typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T, TKey>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudFor(services, typeof(T), typeof(TKey), typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T, TKey, TReadAllResult>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) 
            where T : class =>
            AddApizrCrudFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T, TKey, TReadAllResult, TReadAllParams>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
            where T : class =>
            AddApizrCrudFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type, 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
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
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) 
            where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            AddApizrCrudFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(TApizrManager), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type crudedType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudFor(services, crudedType, typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

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
            AddApizrCrudFor(services, crudedType, crudedKeyType, typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudFor(services, crudedType, crudedType, crudedReadAllResultType, typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="crudedReadAllParamsType">ReadAll query parameters type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type crudedReadAllParamsType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudFor(services, crudedType, crudedType, crudedReadAllResultType, crudedReadAllParamsType, typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="crudedReadAllResultType"/>
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
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudFor(this IServiceCollection services, Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type crudedReadAllParamsType, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            if (!crudedType.GetTypeInfo().IsClass)
                throw new ArgumentException($"{crudedType.Name} is not a class", nameof(crudedType));

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

            var crudAttribute = new CrudEntityAttribute(null, crudedKeyType, crudedReadAllResultType, crudedReadAllParamsType, modelEntityType);
            if (optionsBuilder == null)
                optionsBuilder = builder => builder.ApizrOptions.CrudEntities.Add(crudedType, crudAttribute);
            else
                optionsBuilder += builder => builder.ApizrOptions.CrudEntities.Add(crudedType, crudAttribute);

            var readAllResultType = crudedReadAllResultType.MakeGenericTypeIfNeeded(crudedType);

            return AddApizrFor(services,
                typeof(ICrudApi<,,,>).MakeGenericType(crudedType, crudedKeyType,
                    readAllResultType, crudedReadAllParamsType),
                apizrManagerType.MakeGenericTypeIfNeeded(typeof(ICrudApi<,,,>).MakeGenericType(crudedType, crudedKeyType,
                    readAllResultType, crudedReadAllParamsType)),
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

                AddApizrFor(services,
                    typeof(ICrudApi<,,,>).MakeGenericType(crud.Key, crud.Value.KeyType,
                        readAllResultType, crud.Value.ReadAllParamsType),
                    apizrManagerType.MakeGenericType(typeof(ICrudApi<,,,>).MakeGenericType(crud.Key, crud.Value.KeyType,
                        readAllResultType, crud.Value.ReadAllParamsType)),
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
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddApizrFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrFor(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies) =>
            AddApizrFor(services, typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrFor(this IServiceCollection services, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddApizrFor(services, apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining some options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrFor(this IServiceCollection services, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies)
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
                AddApizrFor(services, webApiType, apizrManagerType.MakeGenericType(webApiType), optionsBuilder);

            return services;
        }

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
            var priorities = apizrOptions.IsPriorityManagementEnabled
                ? ((Priority[])Enum.GetValues(typeof(Priority))).Where(x => x != Priority.Explicit).OrderBy(priority => priority)
                : ((Priority[])Enum.GetValues(typeof(Priority))).Where(x => x == Priority.UserInitiated);

            foreach (var priority in priorities)
            {
                var builder = services.AddHttpClient(ForType(apizrOptions.WebApiType, priority))
                    .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                    {
                        var httpClientHandler = apizrOptions.HttpClientHandlerFactory.Invoke(serviceProvider);
                        var logHandler = serviceProvider.GetRequiredService<ILogHandler>();
                        var handlerBuilder = new HttpHandlerBuilder(httpClientHandler, new HttpTracerLogWrapper(logHandler));
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
                        var primaryMessageHandler = apizrOptions.IsPriorityManagementEnabled
                            ? new RateLimitedHttpMessageHandler(innerHandler, priority)
                            : innerHandler;

                        return primaryMessageHandler;
                    })
                    .AddTypedClient(typeof(ILazyPrioritizedWebApi<>).MakeGenericType(apizrOptions.WebApiType),
                        (client, serviceProvider) =>
                        {
                            if (client.BaseAddress == null)
                            {
                                client.BaseAddress = apizrOptions.BaseAddressFactory.Invoke(serviceProvider);
                                if(client.BaseAddress == null)
                                    throw new ArgumentNullException(nameof(client.BaseAddress), $"You must provide a valid web api uri with the {nameof(WebApiAttribute)} or the options builder");
                            }

                            return Prioritize.TypeFor(apizrOptions.WebApiType, priority)
                                .GetConstructor(new[] {typeof(Func<object>)})
                                ?.Invoke(new object[]
                                {
                                    new Func<object>(() => RestService.For(apizrOptions.WebApiType, client,
                                        apizrOptions.RefitSettingsFactory(serviceProvider)))
                                });
                        });

                apizrOptions.HttpClientBuilder?.Invoke(builder);
            }

            services.AddOrReplaceSingleton(typeof(IConnectivityHandler), apizrOptions.ConnectivityHandlerType);

            services.AddOrReplaceSingleton(typeof(ICacheHandler), apizrOptions.CacheHandlerType);

            services.AddOrReplaceSingleton(typeof(ILogHandler), apizrOptions.LogHandlerType);

            services.AddOrReplaceSingleton(typeof(IMappingHandler), apizrOptions.MappingHandlerType);

            services.TryAddSingleton(typeof(IApizrManager<>).MakeGenericType(apizrOptions.WebApiType), typeof(ApizrManager<>).MakeGenericType(apizrOptions.WebApiType));

            foreach (var postRegistrationAction in apizrOptions.PostRegistrationActions)
            {
                postRegistrationAction.Invoke(services);
            }

            return services;
        }

        #endregion

        private static IApizrExtendedOptions CreateApizrExtendedOptions(Type webApiType, Type apizrManagerType, Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            Uri baseAddress = null;
            var webApiAttribute = webApiType.GetTypeInfo().GetCustomAttribute<WebApiAttribute>(true);
            if (webApiAttribute != null)
            {
                Uri.TryCreate(webApiAttribute.BaseUri, UriKind.RelativeOrAbsolute, out baseAddress);
                if (optionsBuilder == null)
                    optionsBuilder = sourceBuilder => sourceBuilder.ApizrOptions.WebApis.Add(webApiType, webApiAttribute);
                else
                    optionsBuilder += sourceBuilder => sourceBuilder.ApizrOptions.WebApis.Add(webApiType, webApiAttribute);
            }

            var traceAttribute = webApiType.GetTypeInfo().GetCustomAttribute<TraceAttribute>(true);

            var assemblyPolicyAttribute = webApiType.Assembly.GetCustomAttribute<PolicyAttribute>();

            var webApiPolicyAttribute = webApiType.GetTypeInfo().GetCustomAttribute<PolicyAttribute>(true);

            var builder = new ApizrExtendedOptionsBuilder(new ApizrExtendedOptions(webApiType, apizrManagerType, baseAddress, traceAttribute?.Verbosity, webApiAttribute?.IsPriorityManagementEnabled, assemblyPolicyAttribute?.RegistryKeys,
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
    }
}
