using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Mapping;
using Apizr.Requesting;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Extending.Configuring.Registry
{
    public class ApizrExtendedRegistryBuilder : IApizrExtendedRegistryBuilder
    {
        protected readonly ApizrExtendedRegistry Registry;
        protected readonly IApizrExtendedCommonOptions CommonOptions;
        protected readonly IServiceCollection Services;

        internal ApizrExtendedRegistryBuilder(IServiceCollection services, IApizrExtendedCommonOptions commonOptions, ApizrExtendedRegistry mainRegistry = null)
        {
            Services = services;
            CommonOptions = commonOptions;
            Registry = mainRegistry ?? new ApizrExtendedRegistry();
        }

        #region Registry

        public IApizrExtendedRegistry ApizrRegistry => Registry;

        public IApizrExtendedRegistryBuilder AddGroup(Action<IApizrExtendedRegistryBuilder> registryGroupBuilder, Action<IApizrExtendedCommonOptionsBuilder> commonOptionsBuilder = null)
        {
            Services.CreateRegistry(registryGroupBuilder, CommonOptions, commonOptionsBuilder, Registry);

            return this;
        }

        #endregion

        #region Crud

        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where T : class =>
            AddCrudManagerFor(typeof(T), typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T, TKey>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where T : class =>
            AddCrudManagerFor(typeof(T), typeof(TKey), typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where T : class =>
            AddCrudManagerFor(typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
            where T : class  =>
            AddCrudManagerFor(typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(ApizrManager<>),
                optionsBuilder);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            AddCrudManagerFor(typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(TApizrManager),
                optionsBuilder);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(crudedType, typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType, Type crudedKeyType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(crudedType, crudedKeyType, typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType, Type crudedKeyType,
            Type crudedReadAllResultType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(crudedType, crudedType, crudedReadAllResultType, typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType, Type crudedKeyType,
            Type crudedReadAllResultType,
            Type crudedReadAllParamsType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(crudedType, crudedType, crudedReadAllResultType, crudedReadAllParamsType, typeof(ApizrManager<>),
                optionsBuilder);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType, Type crudedKeyType, Type crudedReadAllResultType,
            Type crudedReadAllParamsType, Type apizrManagerType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
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

            Type modelEntityType;
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
                    $"{crudedReadAllResultType.Name} must inherit from {typeof(IEnumerable<>)} or be a generic class",
                    nameof(crudedReadAllResultType));

            if (!typeof(IDictionary<string, object>).IsAssignableFrom(crudedReadAllParamsType) &&
                !crudedReadAllParamsType.IsClass)
                throw new ArgumentException(
                    $"{crudedReadAllParamsType.Name} must inherit from {typeof(IDictionary<string, object>)} or be a class",
                    nameof(crudedReadAllParamsType));

            if (!typeof(IApizrManager<>).IsAssignableFromGenericType(apizrManagerType))
                throw new ArgumentException(
                    $"{apizrManagerType} must inherit from {typeof(IApizrManager<>)}", nameof(apizrManagerType));

            crudAttribute = new CrudEntityAttribute(crudAttribute?.BaseUri, crudedKeyType, crudedReadAllResultType,
                crudedReadAllParamsType, modelEntityType);

            CommonOptions.CrudEntities.Add(crudedType, crudAttribute);

            var readAllResultType = crudedReadAllResultType.MakeGenericTypeIfNeeded(crudedType);

            return AddManagerFor(typeof(ICrudApi<,,,>).MakeGenericType(crudedType, crudedKeyType,
                    readAllResultType, crudedReadAllParamsType),
                apizrManagerType.MakeGenericTypeIfNeeded(typeof(ICrudApi<,,,>).MakeGenericType(crudedType, crudedKeyType,
                    readAllResultType, crudedReadAllParamsType)), optionsBuilder);
        }

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(params Type[] assemblyMarkerTypes) =>
            AddCrudManagerFor(typeof(ApizrManager<>), null,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(params Assembly[] assemblies) =>
            AddCrudManagerFor(typeof(ApizrManager<>), null,
                assemblies);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes) =>
            AddCrudManagerFor(typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies) =>
            AddCrudManagerFor(typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType,
            params Type[] assemblyMarkerTypes) =>
            AddCrudManagerFor(apizrManagerType, null,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType,
            params Assembly[] assemblies) =>
            AddCrudManagerFor(apizrManagerType, null,
                assemblies);

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder,
            params Type[] assemblyMarkerTypes) =>
            AddCrudManagerFor(apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder,
            params Assembly[] assemblies)
        {
            if (!assemblies.Any())
                throw new ArgumentException(
                    $"No assemblies found to scan. Supply at least one assembly to scan for {nameof(CrudEntityAttribute)}.",
                    nameof(assemblies));

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
                CommonOptions.CrudEntities.Add(crudEntityDefinition.Key, crudEntityDefinition.Value);
            }

            foreach (var crud in cruds)
            {
                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.WithBaseAddress(crud.Value.BaseUri);
                else
                    optionsBuilder += builder => builder.WithBaseAddress(crud.Value.BaseUri);

                var readAllResultType = crud.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crud.Key);

                AddManagerFor(typeof(ICrudApi<,,,>).MakeGenericType(crud.Key, crud.Value.KeyType,
                        readAllResultType, crud.Value.ReadAllParamsType),
                    apizrManagerType.MakeGenericType(typeof(ICrudApi<,,,>).MakeGenericType(crud.Key, crud.Value.KeyType,
                        readAllResultType, crud.Value.ReadAllParamsType)), optionsBuilder);
            }

            return this;
        }

        #endregion
        
        #region General

        public IApizrExtendedRegistryBuilder AddManagerFor<TWebApi>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddManagerFor(typeof(TWebApi), typeof(ApizrManager<TWebApi>), optionsBuilder);

        public IApizrExtendedRegistryBuilder AddManagerFor<TWebApi, TApizrManager>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddManagerFor(typeof(TWebApi), typeof(TApizrManager), optionsBuilder);

        public IApizrExtendedRegistryBuilder AddManagerFor(Type webApiType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddManagerFor(webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType), optionsBuilder);

        public IApizrExtendedRegistryBuilder AddManagerFor(Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Type[] assemblyMarkerTypes) =>
            AddManagerFor(typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        public IApizrExtendedRegistryBuilder AddManagerFor(Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Assembly[] assemblies) =>
            AddManagerFor(typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        public IApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Type[] assemblyMarkerTypes) =>
            AddManagerFor(apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        public IApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies)
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
                CommonOptions.ObjectMappings.Add(objectMappingDefinition.Key, objectMappingDefinition.Value);
            }

            var webApiTypes = assembliesToScan
                .SelectMany(assembly => assembly.GetTypes().Where(t =>
                    !t.IsClass && t.GetCustomAttribute<WebApiAttribute>()?.IsAutoRegistrable == true))
                .ToList();

            foreach (var webApiType in webApiTypes)
                AddManagerFor(webApiType, apizrManagerType.MakeGenericType(webApiType), optionsBuilder);

            return this;
        }

        public IApizrExtendedRegistryBuilder AddManagerFor(
            Type webApiType, Type apizrManagerType,
            Action<IApizrExtendedProperOptionsBuilder> properOptionsBuilder = null)
        {
            var properOptions = ServiceCollectionExtensions.CreateProperOptions(CommonOptions, webApiType, apizrManagerType, properOptionsBuilder);
            var serviceType = Services.AddApizrManagerFor(CommonOptions, properOptions);

            Registry.AddOrUpdate(webApiType, serviceType);

            return this;
        }

        #endregion
    }
}
