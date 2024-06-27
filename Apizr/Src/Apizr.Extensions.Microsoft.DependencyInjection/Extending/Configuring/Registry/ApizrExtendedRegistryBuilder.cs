using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Apizr.Configuring;
using Apizr.Configuring.Registry;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Mapping;
using Apizr.Requesting;
using Apizr.Requesting.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Apizr.Extending.Configuring.Registry
{
    /// <summary>
    /// Registry builder options available for extended registrations
    /// </summary>
    public class ApizrExtendedRegistryBuilder : IApizrExtendedRegistryBuilder, IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder>
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

        /// <inheritdoc />
        public IApizrExtendedRegistry ApizrRegistry => Registry;

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddGroup(Action<IApizrExtendedRegistryBuilder> registryGroupBuilder, Action<IApizrExtendedCommonOptionsBuilder> commonOptionsBuilder = null)
        {
            Services.CreateRegistry(registryGroupBuilder, CommonOptions, commonOptionsBuilder, Registry);

            return this;
        }

        #endregion

        #region Crud

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where T : class =>
            AddCrudManagerFor(typeof(T), typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T, TKey>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where T : class =>
            AddCrudManagerFor(typeof(T), typeof(TKey), typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where T : class =>
            AddCrudManagerFor(typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
            where T : class  =>
            AddCrudManagerFor(typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(ApizrManager<>),
                optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            AddCrudManagerFor(typeof(T), typeof(TKey), typeof(TReadAllResult), typeof(TReadAllParams), typeof(TApizrManager),
                optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(crudedType, typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType, Type crudedKeyType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(crudedType, crudedKeyType, typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType, Type crudedKeyType,
            Type crudedReadAllResultType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(crudedType, crudedType, crudedReadAllResultType, typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType, Type crudedKeyType,
            Type crudedReadAllResultType,
            Type crudedReadAllParamsType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(crudedType, crudedType, crudedReadAllResultType, crudedReadAllParamsType, typeof(ApizrManager<>),
                optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType, Type crudedKeyType, Type crudedReadAllResultType,
            Type crudedReadAllParamsType, Type apizrManagerType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        {
            var crudedTypeInfos = crudedType.GetTypeInfo();
            if (!crudedTypeInfos.IsClass)
                throw new ArgumentException($"{crudedType.Name} is not a class", nameof(crudedType));
            if (crudedTypeInfos.IsAbstract)
                throw new ArgumentException($"{crudedType.Name} is an abstract class", nameof(crudedType));

            var baseAddressAttribute = ApizrBuilder.GetBaseAddressAttribute(crudedType);
            if (baseAddressAttribute != null)
            {
                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.WithBaseAddress(baseAddressAttribute.BaseAddressOrPath);
                else
                    optionsBuilder += builder => builder.WithBaseAddress(baseAddressAttribute.BaseAddressOrPath, ApizrDuplicateStrategy.Ignore);
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
                !crudedReadAllParamsType!.IsClass)
                throw new ArgumentException(
                    $"{crudedReadAllParamsType.Name} must inherit from {typeof(IDictionary<string, object>)} or be a class",
                    nameof(crudedReadAllParamsType));

            if (!typeof(IApizrManager<>).IsAssignableFromGenericType(apizrManagerType))
                throw new ArgumentException(
                    $"{apizrManagerType} must inherit from {typeof(IApizrManager<>)}", nameof(apizrManagerType));

            var crudAttribute = new CrudEntityAttribute(baseAddressAttribute?.BaseAddressOrPath, crudedKeyType, crudedReadAllResultType, crudedReadAllParamsType, modelEntityType);
            
            CommonOptions.CrudEntities.Add(crudedType, crudAttribute);

            var readAllResultType = crudedReadAllResultType.MakeGenericTypeIfNeeded(crudedType);
            var crudApiType = typeof(ICrudApi<,,,>).MakeGenericType(crudedType, crudedKeyType, readAllResultType, crudedReadAllParamsType);

            return AddManagerFor(crudApiType, apizrManagerType.MakeGenericTypeIfNeeded(crudApiType), optionsBuilder);
        }

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(params Type[] assemblyMarkerTypes) =>
            AddCrudManagerFor(typeof(ApizrManager<>), null,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(params Assembly[] assemblies) =>
            AddCrudManagerFor(typeof(ApizrManager<>), null,
                assemblies);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Action<IApizrExtendedProperOptionsBuilder> optionsBuilder, params Type[] assemblyMarkerTypes) =>
            AddCrudManagerFor(typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Action<IApizrExtendedProperOptionsBuilder> optionsBuilder, params Assembly[] assemblies) =>
            AddCrudManagerFor(typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType,
            params Type[] assemblyMarkerTypes) =>
            AddCrudManagerFor(apizrManagerType, null,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType,
            params Assembly[] assemblies) =>
            AddCrudManagerFor(apizrManagerType, null,
                assemblies);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder,
            params Type[] assemblyMarkerTypes) =>
            AddCrudManagerFor(apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder,
            params Assembly[] assemblies)
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

            var cruds = new Dictionary<Type, CrudEntityAttribute>();

            foreach (var crudEntityDefinition in crudEntityDefinitions)
            {
                if (crudEntityDefinition.Value.MappedEntityType == null)
                    crudEntityDefinition.Value.MappedEntityType = crudEntityDefinition.Key;

                var baseAddressAttribute = ApizrBuilder.GetBaseAddressAttribute(crudEntityDefinition.Key);
                if (baseAddressAttribute != null)
                    crudEntityDefinition.Value.BaseAddressOrPath = baseAddressAttribute.BaseAddressOrPath;

                cruds.Add(crudEntityDefinition.Key, crudEntityDefinition.Value);

                CommonOptions.CrudEntities.Add(crudEntityDefinition.Key, crudEntityDefinition.Value);
            }

            foreach (var crud in cruds)
            {
                if (optionsBuilder == null)
                    optionsBuilder = builder => builder.WithBaseAddress(crud.Value.BaseAddressOrPath);
                else
                    optionsBuilder += builder => builder.WithBaseAddress(crud.Value.BaseAddressOrPath, ApizrDuplicateStrategy.Ignore);

                var readAllResultType = crud.Value.ReadAllResultType.MakeGenericTypeIfNeeded(crud.Key);
                var crudApiType = typeof(ICrudApi<,,,>).MakeGenericType(crud.Key, crud.Value.KeyType, readAllResultType, crud.Value.ReadAllParamsType);

                AddManagerFor(crudApiType, apizrManagerType.MakeGenericType(crudApiType), optionsBuilder);
            }

            return this;
        }

        #endregion

        #region General

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor<TWebApi>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddManagerFor(typeof(TWebApi), typeof(ApizrManager<TWebApi>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor<TWebApi, TApizrManager>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddManagerFor(typeof(TWebApi), typeof(TApizrManager), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(Type webApiType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddManagerFor(webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Type[] assemblyMarkerTypes) =>
            AddManagerFor(typeof(ApizrManager<>), optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Assembly[] assemblies) =>
            AddManagerFor(typeof(ApizrManager<>), optionsBuilder,
                assemblies);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Type[] assemblyMarkerTypes) =>
            AddManagerFor(apizrManagerType, optionsBuilder,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray());

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies)
        {
            if (assemblies?.Length is null or 0)
                throw new ArgumentException(
                    $"No assemblies found to scan. Supply at least one assembly to scan for {nameof(BaseAddressAttribute)}.", nameof(assemblies));

            var allTypes = assemblies
                .Distinct()
                .SelectMany(assembly => assembly
                    .GetTypes())
                .ToList();

            var objectMappingDefinitions = allTypes
                .Where(type => type.IsClass)
                .Select(type => new
                {
                    Type = type,
                    Attribute = type.GetCustomAttribute<MappedWithAttribute>()
                })
                .Where(item => item.Attribute != null)
                .ToDictionary(item => item.Type, item => item.Attribute);

            foreach (var objectMappingDefinition in objectMappingDefinitions)
            {
                CommonOptions.ObjectMappings.Add(objectMappingDefinition.Key, objectMappingDefinition.Value);
            }

            var webApiTypes = allTypes.Where(type =>
                    !type.IsClass && type.GetMethods().Any(method => method.GetCustomAttribute<HttpMethodAttribute>(true) != null))
                .ToList();

            foreach (var webApiType in webApiTypes)
                AddManagerFor(webApiType, apizrManagerType.MakeGenericType(webApiType), optionsBuilder);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(
            Type webApiType, Type apizrManagerType,
            Action<IApizrExtendedProperOptionsBuilder> properOptionsBuilder = null)
        {
            var properOptions = ServiceCollectionExtensions.CreateProperOptions(CommonOptions, webApiType, apizrManagerType, properOptionsBuilder);
            var apizrManagerServiceType = Services.AddApizrManagerFor(CommonOptions, properOptions);

            Registry.AddOrUpdateManager(apizrManagerServiceType);

            return this;
        }

        #endregion

        void IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder>.AddWrappingManagerFor<TWebApi, TWrappingManagerService, TWrappingManagerImplementation>(
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        {
            if (!Registry.ContainsManagerFor<TWebApi>())
                AddManagerFor<TWebApi>(optionsBuilder);

            Services.AddOrReplaceSingleton(typeof(TWrappingManagerService), typeof(TWrappingManagerImplementation));
            Registry.AddOrUpdateManager(typeof(TWrappingManagerService));
        }

        void IApizrInternalGlobalRegistryBuilder.AddAliasingManagerFor<TAliasingManager, TAliasedManager>()
        {
            Services.AddOrReplaceSingleton(typeof(TAliasingManager), serviceProvider => serviceProvider.GetRequiredService<TAliasedManager>());
            Registry.AddOrUpdateManager(typeof(TAliasingManager));
        }
    }
}
