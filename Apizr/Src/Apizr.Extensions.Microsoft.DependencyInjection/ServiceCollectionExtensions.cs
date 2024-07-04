﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Cancelling.Attributes.Operation;
using Apizr.Cancelling.Attributes.Request;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Manager;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Helping;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Mapping;
using Apizr.Requesting;
using Apizr.Requesting.Attributes;
using Apizr.Resiliencing;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using Refit;

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
        public static IServiceCollection AddApizrCrudManagerFor<T>(this IServiceCollection services,Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor<T, TKey>(this IServiceCollection services,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) where T : class =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(TKey), typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor<T, TKey, TReadAllResult>(this IServiceCollection services,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
            where T : class =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(this IServiceCollection services,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
            where T : class =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type, 
        /// with key of type <typeparamref name="TKey"/> and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{ICrudApi}"/> implementation</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(this IServiceCollection services,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
            where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            AddApizrCrudManagerFor(services, typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(TApizrManager), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apiEntityType">The object type to manage with crud api calls (class)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apiEntityType,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, apiEntityType, typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <paramref name="apiEntityKeyType"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apiEntityType">The object type to manage with crud api calls (class)</param>
        /// <param name="apiEntityKeyType">The object key type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apiEntityType,
            Type apiEntityKeyType, Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, apiEntityType, apiEntityKeyType, typeof(IEnumerable<>), typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <paramref name="apiEntityKeyType"/> and "ReadAll" query result of type <paramref name="readAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apiEntityType">The object type to manage with crud api calls (class)</param>
        /// <param name="apiEntityKeyType">The object key type</param>
        /// <param name="readAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apiEntityType,
            Type apiEntityKeyType, Type readAllResultType, Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, apiEntityType, apiEntityType, readAllResultType, typeof(IDictionary<string, object>), typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <paramref name="apiEntityKeyType"/> and "ReadAll" query result of type <paramref name="readAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apiEntityType">The object type to manage with crud api calls (class)</param>
        /// <param name="apiEntityKeyType">The object key type</param>
        /// <param name="readAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="readAllParamsType">ReadAll query parameters type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apiEntityType,
            Type apiEntityKeyType, Type readAllResultType, Type readAllParamsType,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, apiEntityType, apiEntityType, readAllResultType, readAllParamsType, typeof(ApizrManager<>), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <paramref name="apiEntityKeyType"/> and "ReadAll" query result of type <paramref name="readAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apiEntityType">The object type to manage with crud api calls</param>
        /// <param name="apiEntityKeyType">The object key type</param>
        /// <param name="readAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="readAllParamsType">ReadAll query parameters type</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apiEntityType,
            Type apiEntityKeyType, Type readAllResultType, Type readAllParamsType, Type apizrManagerType,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        {
            var readAllResultFullType = readAllResultType.MakeGenericTypeIfNeeded(apiEntityType);
            var crudApiType = typeof(ICrudApi<,,,>).MakeGenericType(apiEntityType, apiEntityKeyType, readAllResultFullType, readAllParamsType);

            return AddApizrManagerFor(services, crudApiType, apizrManagerType.MakeGenericTypeIfNeeded(crudApiType), CreateCommonOptions(), optionsBuilder);
        }

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services,
            Type[] assemblyMarkerTypes,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, typeof(ApizrManager<>),
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray(), optionsBuilder);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Assembly[] assemblies,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, typeof(ApizrManager<>),
                assemblies, optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apizrManagerType,
            Type[] assemblyMarkerTypes,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrCrudManagerFor(services, apizrManagerType,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray(), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrCrudManagerFor(this IServiceCollection services, Type apizrManagerType, Assembly[] assemblies,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        {
            if (assemblies?.Length is null or 0)
                throw new ArgumentException(
                    $"No assemblies found to scan. Supply at least one assembly to scan for {nameof(CrudEntityAttribute)}.", nameof(assemblies));

            var crudTypes = assemblies
                .Distinct()
                .SelectMany(assembly => assembly
                    .GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract))
                .Select(type => new
                {
                    Type = type,
                    Attributes = type.GetCustomAttributes<CrudEntityAttribute>(true)
                })
                .Where(item => item.Attributes != null)
                .ToDictionary(item => item.Type, item => item.Attributes);

            var modelEntityTypes = crudTypes
                .Select(item => new
                {
                    Type = item.Key,
                    Attribute = item.Value.FirstOrDefault(attribute => attribute is MappedCrudEntityAttribute) as MappedCrudEntityAttribute
                })
                .Where(item => item.Attribute != null)
                .ToDictionary(item => item.Attribute.MappedEntityType, item => item.Attribute.ToCrudEntityAttribute(item.Type));

            var apiEntityTypes = crudTypes
                .Select(item => new
                {
                    Type = item.Key,
                    Attribute = item.Value.FirstOrDefault(attribute => attribute is not MappedCrudEntityAttribute)
                })
                .Where(item => item.Attribute != null && !modelEntityTypes.ContainsKey(item.Type))
                .ToDictionary(item => item.Type, item => item.Attribute);

            var crudEntityDefinitions = modelEntityTypes
                .Concat(apiEntityTypes)
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault(y => y.MappedEntityType != null) ?? x.First());

            foreach (var crudEntityDefinition in crudEntityDefinitions)
            {
                var readAllResultType = crudEntityDefinition.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crudEntityDefinition.Key);

                AddApizrCrudManagerFor(services, crudEntityDefinition.Key, crudEntityDefinition.Value.KeyType, readAllResultType, crudEntityDefinition.Value.ReadAllParamsType, apizrManagerType, optionsBuilder);
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
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
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
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
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
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrManagerFor(services, webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType),
                CreateCommonOptions(), optionsBuilder);

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(this IServiceCollection services,
            Type[] assemblyMarkerTypes,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrManagerFor(services, typeof(ApizrManager<>),
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray(), optionsBuilder);

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(this IServiceCollection services, Assembly[] assemblies,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrManagerFor(services, typeof(ApizrManager<>),
                assemblies, optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(this IServiceCollection services, Type apizrManagerType,
            Type[] assemblyMarkerTypes,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
            AddApizrManagerFor(services, apizrManagerType,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray(), optionsBuilder);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <returns></returns>
        public static IServiceCollection AddApizrManagerFor(this IServiceCollection services, Type apizrManagerType, Assembly[] assemblies,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        {
            if (assemblies?.Length is not > 0)
                throw new ArgumentException(
                    $"No assemblies found to scan.", nameof(assemblies));

            var webApiTypes = assemblies
                .Distinct()
                .SelectMany(assembly => assembly
                    .GetTypes()
                    .Where(type => type.IsInterface && type.GetMethods()
                        .Any(method => method
                            .GetCustomAttribute<HttpMethodAttribute>(true) != null)))
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
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
            => AddApizrManagerFor(services, webApiType, apizrManagerType, CreateCommonOptions(), optionsBuilder);

        private static IServiceCollection AddApizrManagerFor(
            this IServiceCollection services, Type webApiType, Type apizrManagerType,
            IApizrExtendedCommonOptions commonOptions,
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        {
            AddApizrManagerFor(services, commonOptions,
                CreateProperOptions(commonOptions, webApiType, apizrManagerType), optionsBuilder);

            return services;
        }

        internal static Type AddApizrManagerFor(
            this IServiceCollection services,
            IApizrExtendedCommonOptions commonOptions,
            IApizrExtendedProperOptions properOptions, 
            Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        {
            var serviceType = typeof(IApizrManager<>).MakeGenericType(properOptions.WebApiType);

            if (!serviceType.IsAssignableFrom(properOptions.ApizrManagerType))
                throw new ArgumentException(
                    $"Your Apizr manager class must inherit from IApizrManager generic interface or derived");

            if (services.Any(sd => sd.ServiceType == serviceType && sd.ImplementationType == properOptions.ApizrManagerType))
                return serviceType;

            var webApiFriendlyName = properOptions.WebApiType.GetFriendlyName();
            var apizrOptionsBuilder = CreateOptions(commonOptions, properOptions, optionsBuilder);
            var apizrOptions = apizrOptionsBuilder.ApizrOptions;
            var apizrOptionsRegistrationType = typeof(IApizrManagerOptions<>).MakeGenericType(apizrOptions.WebApiType);
            
            var builder = services.AddHttpClient(ForType(apizrOptions.WebApiType))
                .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                {
                    var options = (IApizrExtendedManagerOptionsBase)serviceProvider.GetRequiredService(apizrOptionsRegistrationType);
                    var handlerBuilder = new ExtendedHttpHandlerBuilder(options.HttpClientHandler, options);
                    handlerBuilder.AddHandler(new ResilienceHttpMessageHandler(serviceProvider.GetService<ResiliencePipelineRegistry<string>>(), apizrOptions));

                    foreach (var delegatingHandlerExtendedFactory in options.DelegatingHandlersExtendedFactories.Values)
                        handlerBuilder.AddHandler(delegatingHandlerExtendedFactory.Invoke(serviceProvider, options));

                    if (apizrOptions.HttpMessageHandlerFactory != null)
                        handlerBuilder.AddHandler(apizrOptions.HttpMessageHandlerFactory.Invoke(serviceProvider, options));

                    var innerHandler = handlerBuilder.Build();
                    var primaryHandler = apizrOptions.PrimaryHandlerFactory?.Invoke(innerHandler, apizrOptions.Logger, apizrOptions) ?? innerHandler;
                    var primaryMessageHandler = new ApizrHttpMessageHandler(primaryHandler, apizrOptions);

                    return primaryMessageHandler;
                })
                .AddTypedClient(typeof(ILazyFactory<>).MakeGenericType(apizrOptions.WebApiType), apizrOptions,
                    (httpClient, serviceProvider) =>
                    {
                        var options = (IApizrExtendedManagerOptionsBase)serviceProvider.GetRequiredService(apizrOptionsRegistrationType);

                        // HttpClient
                        if (httpClient.BaseAddress == null) 
                            httpClient.BaseAddress = options.BaseUri;

                        // Refit rest service
                        return typeof(LazyFactory<>).MakeGenericType(options.WebApiType)
                            .GetConstructor(new[] { typeof(Func<object>) })
                            ?.Invoke(new object[]
                            {
                                new Func<object>(() => RestService.For(options.WebApiType, httpClient,
                                    options.RefitSettings))
                            });
                    });

            // Custom client config
            apizrOptions.HttpClientBuilder?.Invoke(builder);

            if(apizrOptions.ShouldRedactHeaderValue != null)
                builder.RedactLoggedHeaders(apizrOptions.ShouldRedactHeaderValue);

            services.TryAddSingleton(typeof(ILazyFactory<ResiliencePipelineRegistry<string>>), serviceProvider =>
                new LazyFactory<ResiliencePipelineRegistry<string>>(
                    () => serviceProvider.GetService<ResiliencePipelineRegistry<string>>() ?? new ResiliencePipelineRegistry<string>()));

            if (apizrOptions.ConnectivityHandlerFactory != null)
                services.AddOrReplaceSingleton(typeof(IConnectivityHandler), apizrOptions.ConnectivityHandlerFactory);
            else if (apizrOptions.ConnectivityHandlerType == typeof(DefaultConnectivityHandler))
                services.TryAddSingleton(typeof(IConnectivityHandler), _ => new DefaultConnectivityHandler(() => true));
            else
                services.TryAddSingleton(typeof(IConnectivityHandler), apizrOptions.ConnectivityHandlerType);

            var cacheHandlerFactory = apizrOptions.GetCacheHanderFactory();
            if (cacheHandlerFactory != null)
            {
                Func<IServiceProvider, ICacheHandler> factory = _ => cacheHandlerFactory.Invoke();
                services.AddOrReplaceSingleton(typeof(ICacheHandler), factory);
                apizrOptionsBuilder.WithCacheHandler(factory);
            }
            else
            {
                var cacheHandlerType = apizrOptions.GetCacheHanderType();
                if (cacheHandlerType != null)
                {
                    services.AddOrReplaceSingleton(typeof(ICacheHandler), cacheHandlerType);
                    apizrOptionsBuilder.WithCacheHandler(cacheHandlerType);
                }
                else
                    services.TryAddSingleton(typeof(ICacheHandler), apizrOptions.CacheHandlerType);
            }

            var mappingHandlerFactory = apizrOptions.GetMappingHanderFactory();
            if (mappingHandlerFactory != null)
            {
                Func<IServiceProvider, IMappingHandler> factory = _ => mappingHandlerFactory.Invoke();
                services.AddOrReplaceSingleton(typeof(IMappingHandler), factory);
                apizrOptionsBuilder.WithMappingHandler(factory);
            }
            else
            {
                var mappingHandlerType = apizrOptions.GetMappingHanderType();
                if (mappingHandlerType != null)
                {
                    services.AddOrReplaceSingleton(typeof(IMappingHandler), mappingHandlerType);
                    apizrOptionsBuilder.WithMappingHandler(mappingHandlerType);
                }
                else
                {
                    services.TryAddSingleton(typeof(IMappingHandler), apizrOptions.MappingHandlerType);
                }
            }

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
                apizrOptions.OperationTimeoutFactory?.Invoke(serviceProvider);
                apizrOptions.RequestTimeoutFactory?.Invoke(serviceProvider);

                var headersFactories = apizrOptions.HeadersExtendedFactories?.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Invoke(serviceProvider));
                if (headersFactories?.Count > 0)
                    apizrOptionsBuilder.WithHeaders(headersFactories);

                if (headersFactories?.TryGetValue((ApizrRegistrationMode.Set, ApizrLifetimeScope.Api), out var setFactory) == true)
                {
                    // Set api scoped headers right the way
                    var setHeaders = setFactory.Invoke()?.ToArray();
                    if (setHeaders?.Length > 0)
                        apizrOptionsBuilder.WithHeaders(setHeaders, mode: ApizrRegistrationMode.Set);
                }
                if (headersFactories?.TryGetValue((ApizrRegistrationMode.Store, ApizrLifetimeScope.Api), out var storeFactory) == true)
                {
                    // Store api scoped headers for further attribute key match use
                    var storeHeaders = storeFactory.Invoke()?.ToArray();
                    if (storeHeaders?.Length > 0)
                        apizrOptionsBuilder.WithHeaders(storeHeaders, mode: ApizrRegistrationMode.Store);
                }

                foreach (var resiliencePropertiesExtendedFactory in apizrOptions.ResiliencePropertiesExtendedFactories)
                    apizrOptions.ResiliencePropertiesFactories[resiliencePropertiesExtendedFactory.Key] = () =>
                        resiliencePropertiesExtendedFactory.Value.Invoke(serviceProvider);

                return Activator.CreateInstance(typeof(ApizrExtendedManagerOptions<>).MakeGenericType(apizrOptions.WebApiType), apizrOptions);
            });

            services.TryAddSingleton(serviceProvider => ((IApizrManagerOptionsBase)serviceProvider.GetRequiredService(apizrOptionsRegistrationType)).RefitSettings.ContentSerializer);

            services.TryAddSingleton(serviceType, apizrOptions.ApizrManagerType);

            foreach (var postRegistrationAction in apizrOptions.PostRegistrationActions)
            {
                postRegistrationAction.Invoke(apizrOptions, services);
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

            var builder = new ApizrExtendedCommonOptionsBuilder(baseApizrCommonOptions) as IApizrExtendedCommonOptionsBuilder;

            commonOptionsBuilder?.Invoke(builder);

            return builder.ApizrOptions;
        }

        internal static IApizrExtendedProperOptions CreateProperOptions(IApizrExtendedCommonOptions commonOptions, Type webApiType, Type apizrManagerType,
            Action<IApizrExtendedProperOptionsBuilder> properOptionsBuilder = null)
        {
            if (commonOptions == null)
                throw new ArgumentNullException(nameof(commonOptions));

            if (!typeof(IApizrManager<>).IsAssignableFromGenericType(apizrManagerType))
                throw new ArgumentException(
                    $"{apizrManagerType} must inherit from {typeof(IApizrManager<>)}", nameof(apizrManagerType));

            string baseAddress = null;
            string basePath = null;

            TypeInfo typeInfo;
            Type crudApiEntityType = null, crudApiEntityKeyType = null, crudApiReadAllResultType = null, crudApiReadAllParamsType = null;
            var isCrudApi = typeof(ICrudApi<,,,>).IsAssignableFromGenericType(webApiType);
            if (isCrudApi)
            {
                var argumentTypes = webApiType.GetGenericArguments();

                crudApiEntityType = argumentTypes[0];
                typeInfo = crudApiEntityType.GetTypeInfo();
                if (!typeInfo.IsClass)
                    throw new ArgumentException($"{crudApiEntityType.Name} is not a class", nameof(crudApiEntityType));
                if (typeInfo.IsAbstract)
                    throw new ArgumentException($"{crudApiEntityType.Name} is an abstract class", nameof(crudApiEntityType));

                crudApiEntityKeyType = argumentTypes[1];
                if (crudApiEntityKeyType.GetTypeInfo().IsClass)
                    throw new ArgumentException($"{crudApiEntityKeyType.Name} can't be a class", nameof(crudApiEntityKeyType));

                crudApiReadAllResultType = argumentTypes[2];
                if ((!typeof(IEnumerable<>).IsAssignableFromGenericType(crudApiReadAllResultType) &&
                     !crudApiReadAllResultType.IsClass) || !crudApiReadAllResultType.IsGenericType)
                    throw new ArgumentException(
                        $"{crudApiReadAllResultType.Name} must inherit from {typeof(IEnumerable<>)} or be a generic class", nameof(crudApiReadAllResultType));

                crudApiReadAllParamsType = argumentTypes[3];
                if (!typeof(IDictionary<string, object>).IsAssignableFrom(crudApiReadAllParamsType) &&
                    !crudApiReadAllParamsType!.IsClass)
                    throw new ArgumentException(
                        $"{crudApiReadAllParamsType.Name} must inherit from {typeof(IDictionary<string, object>)} or be a class", nameof(crudApiReadAllParamsType));
            }
            else
            {
                typeInfo = webApiType.GetTypeInfo();
            }

            var baseAddressAttribute = isCrudApi ? ApizrBuilder.GetBaseAddressAttribute(crudApiEntityType) : ApizrBuilder.GetBaseAddressAttribute(webApiType);
            if (baseAddressAttribute != null)
            {
                if (!string.IsNullOrWhiteSpace(baseAddressAttribute.BaseAddressOrPath))
                {
                    if (Uri.IsWellFormedUriString(baseAddressAttribute.BaseAddressOrPath, UriKind.Absolute))
                        baseAddress = baseAddressAttribute.BaseAddressOrPath;
                    else
                        basePath = baseAddressAttribute.BaseAddressOrPath;
                }
            }

            if (!commonOptions.ObjectMappings.ContainsKey(typeInfo.Assembly))
            {
                var objectMappingDefinitions = typeInfo.Assembly
                    .GetTypes()
                    .Where(type => type.IsClass && !type.IsAbstract)
                    .Select(type => new
                    {
                        Type = type,
                        Attribute = type.GetCustomAttribute<MappedWithAttribute>(true)
                    })
                    .Where(item => item.Attribute != null)
                    .Select(item => new MappedWithAttribute(item.Type, item.Attribute.TargetEntityType))
                    .ToArray();

                commonOptions.ObjectMappings.Add(typeInfo.Assembly, objectMappingDefinitions);
            }

            var properDeclaringTypeAttributes = typeInfo.DeclaringType != null
                ? typeInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true)
                : typeInfo.GetTypeInfo().GetCustomAttributes(true);

            var properInheritedAttributes = typeInfo.DeclaringType != null
                ? typeInfo.DeclaringType.GetInterfaces().SelectMany(i => i.GetTypeInfo().GetCustomAttributes(true)).ToArray()
                : typeInfo.GetInterfaces().SelectMany(i => i.GetTypeInfo().GetCustomAttributes(true)).ToArray();

            var properAttributesAsc = properDeclaringTypeAttributes.Concat(properInheritedAttributes).ToList();
            var properAttributesDesc = properInheritedAttributes.Reverse().Concat(properDeclaringTypeAttributes).ToList();
            var commonAttributes = typeInfo.Assembly.GetCustomAttributes().ToList();

            var properParameterAttributes = isCrudApi
                ? properAttributesDesc.OfType<HandlerParameterAttribute>().Where(att => att is not CrudHandlerParameterAttribute).ToList()
                : properAttributesDesc.OfType<HandlerParameterAttribute>().ToList();
            var commonParameterAttributes = commonAttributes.OfType<HandlerParameterAttribute>().ToList();
            var properHeadersAttribute = properAttributesAsc.OfType<HeadersAttribute>().FirstOrDefault();
            var commonHeadersAttribute = commonAttributes.OfType<HeadersAttribute>().FirstOrDefault();
            var properLogAttribute = properAttributesAsc.OfType<LogAttribute>().FirstOrDefault();
            var commonLogAttribute = commonAttributes.OfType<LogAttribute>().FirstOrDefault();
            var properOperationTimeoutAttribute = properAttributesAsc.OfType<OperationTimeoutAttribute>().FirstOrDefault();
            var commonOperationTimeoutAttribute = commonAttributes.OfType<OperationTimeoutAttribute>().FirstOrDefault();
            var properRequestTimeoutAttribute = properAttributesAsc.OfType<RequestTimeoutAttribute>().FirstOrDefault();
            var commonRequestTimeoutAttribute = commonAttributes.OfType<RequestTimeoutAttribute>().FirstOrDefault();
            var properCacheAttribute = properAttributesAsc.OfType<CacheAttribute>().FirstOrDefault();
            var commonCacheAttribute = commonAttributes.OfType<CacheAttribute>().FirstOrDefault();
            var properResiliencePipelineAttributes = properAttributesDesc.OfType<ResiliencePipelineAttributeBase>().ToArray();
            var commonResiliencePipelineAttributes = commonAttributes.OfType<ResiliencePipelineAttributeBase>().ToArray();

            // Headers redaction
            var headers = (properHeadersAttribute?.Headers ?? [])
                .Concat(commonHeadersAttribute?.Headers ?? [])
                .ToList();
            var redactHeaders = new List<string>();
            if (headers.Count > 0)
                foreach (var header in headers)
                    if (HttpRequestMessageExtensions.TryGetHeaderKeyValue(header, out var key, out var value) && value.StartsWith("*") && value.EndsWith("*"))
                        redactHeaders.Add(key);

            // Handlers parameters
            var handlersParameters = new Dictionary<string, object>();
            foreach (var commonParameterAttribute in commonParameterAttributes.Where(att => !string.IsNullOrWhiteSpace(att.Key)))
                handlersParameters[commonParameterAttribute.Key!] = commonParameterAttribute.Value;
            foreach (var commonOptionsHandlersParameter in commonOptions.HandlersParameters)
                handlersParameters[commonOptionsHandlersParameter.Key] = commonOptionsHandlersParameter.Value;
            foreach (var properParameterAttribute in properParameterAttributes.Where(att => !string.IsNullOrWhiteSpace(att.Key)))
                handlersParameters[properParameterAttribute.Key!] = properParameterAttribute.Value;

            // Resilience pipelines
            foreach (var commonResiliencePipelineAttribute in commonResiliencePipelineAttributes.Where(attribute => attribute is ResiliencePipelineAttribute))
                commonResiliencePipelineAttribute.RequestMethod = isCrudApi ? ApizrRequestMethod.AllCrud : ApizrRequestMethod.AllHttp;
            foreach (var properResiliencePipelineAttribute in properResiliencePipelineAttributes.Where(attribute => attribute is ResiliencePipelineAttribute))
                properResiliencePipelineAttribute.RequestMethod = isCrudApi ? ApizrRequestMethod.AllCrud : ApizrRequestMethod.AllHttp;

            var builder = new ApizrExtendedProperOptionsBuilder(new ApizrExtendedProperOptions(commonOptions, 
                webApiType,
                crudApiEntityType,
                crudApiEntityKeyType,
                crudApiReadAllResultType,
                crudApiReadAllParamsType,
                typeInfo,
                apizrManagerType,
                baseAddress,
                basePath,
                handlersParameters,
                properLogAttribute?.HttpTracerMode ?? (commonOptions.HttpTracerMode != HttpTracerMode.Unspecified ? commonOptions.HttpTracerMode : commonLogAttribute?.HttpTracerMode),
                properLogAttribute?.TrafficVerbosity ?? (commonOptions.TrafficVerbosity != HttpMessageParts.Unspecified ? commonOptions.TrafficVerbosity : commonLogAttribute?.TrafficVerbosity),
                properOperationTimeoutAttribute?.Timeout ?? commonOperationTimeoutAttribute?.Timeout,
                properRequestTimeoutAttribute?.Timeout ?? commonRequestTimeoutAttribute?.Timeout,
                commonResiliencePipelineAttributes,
                properResiliencePipelineAttributes,
                commonCacheAttribute,
                properCacheAttribute,
                redactHeaders.Count > 0 ? header => redactHeaders.Contains(header) : null,
                properLogAttribute?.LogLevels ?? (commonOptions.LogLevels?.Any() == true ? commonOptions.LogLevels : commonLogAttribute?.LogLevels))) as IApizrExtendedProperOptionsBuilder;

            if(commonOptions.ApizrConfigurationSection != null)
                builder.WithConfiguration(commonOptions.ApizrConfigurationSection);

            properOptionsBuilder?.Invoke(builder);

            return builder.ApizrOptions;
        }

        private static IApizrExtendedManagerOptionsBuilder CreateOptions(IApizrExtendedCommonOptions commonOptions,
            IApizrExtendedProperOptions properOptions, Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        {
            var builder = new ApizrExtendedManagerOptionsBuilder(new ApizrExtendedManagerOptions(commonOptions, properOptions));

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

        internal static IServiceCollection AddOrReplaceSingleton(this IServiceCollection services, Type serviceType, Type implementationType)
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

        internal static IServiceCollection AddOrReplaceSingleton(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> factory)
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

        internal static IServiceCollection TryAddSingleton<TService, TImplementation>(this IServiceCollection service)
            where TService : class
            where TImplementation : class, TService
        {
            service.TryAddSingleton(typeof(TService), typeof(TImplementation));

            return service;
        }

        internal static IServiceCollection TryAddSingleton<TService>(this IServiceCollection service, Func<IServiceProvider, object> factory)
            where TService : class
        {
            service.TryAddSingleton(typeof(TService), factory);

            return service;
        }
    }
}
