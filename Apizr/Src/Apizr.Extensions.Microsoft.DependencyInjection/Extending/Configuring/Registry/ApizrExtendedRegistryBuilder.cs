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
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(apiEntityType, typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType, Type apiEntityKeyType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(apiEntityType, apiEntityKeyType, typeof(IEnumerable<>), typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType, Type apiEntityKeyType,
            Type apiEntityReadAllResultType,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(apiEntityType, apiEntityType, apiEntityReadAllResultType, typeof(IDictionary<string, object>),
                typeof(ApizrManager<>), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType, Type apiEntityKeyType,
            Type apiEntityReadAllResultType,
            Type apiEntityReadAllParamsType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddCrudManagerFor(apiEntityType, apiEntityType, apiEntityReadAllResultType, apiEntityReadAllParamsType, typeof(ApizrManager<>),
                optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType, Type apiEntityKeyType, Type apiEntityReadAllResultType,
            Type apiEntityReadAllParamsType, Type apizrManagerImplementationType, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        {
            var readAllResultFullType = apiEntityReadAllResultType.MakeGenericTypeIfNeeded(apiEntityType);
            var crudApiType = typeof(ICrudApi<,,,>).MakeGenericType(apiEntityType, apiEntityKeyType, readAllResultFullType, apiEntityReadAllParamsType);

            return AddManagerFor(crudApiType, apizrManagerImplementationType, optionsBuilder);
        }

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type[] assemblyMarkerTypes,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder) =>
            AddCrudManagerFor(typeof(ApizrManager<>),
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray(), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Assembly[] assemblies,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder) =>
            AddCrudManagerFor(typeof(ApizrManager<>),
                assemblies, optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerImplementationType,
            Type[] assemblyMarkerTypes, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder) =>
            AddCrudManagerFor(apizrManagerImplementationType,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray(), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerImplementationType,
            Assembly[] assemblies,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder)
        {
            if (assemblies?.Length is not > 0)
                throw new ArgumentException(
                    $"No assemblies found to scan.", nameof(assemblies));

            var apiOrEntityTypes = assemblies
                .Distinct()
                .SelectMany(assembly => assembly.GetTypes()) // All assembly types
                .Select(type => new
                {
                    Type = type,
                    Attribute = type.GetCustomAttributes<AutoRegisterAttribute>(true).FirstOrDefault()
                })
                .Where(item => item.Attribute != null) // Those actually decorated
                .Select(item => item.Attribute.WebApiType ?? item.Type) // Get the parameter type first, otherwise the decorated one
                .Where(type => type.IsInterface || (type.IsClass && !type.IsAbstract)) // Keep only api interfaces and entity classes
                .ToList();

            foreach (var apiOrEntityType in apiOrEntityTypes)
            {
                if (apiOrEntityType.IsInterface)
                    AddManagerFor(apiOrEntityType, apizrManagerImplementationType, optionsBuilder);
                else
                    AddCrudManagerFor(apiOrEntityType, typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>), apizrManagerImplementationType, optionsBuilder);
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
        public IApizrExtendedRegistryBuilder AddManagerFor(Type[] assemblyMarkerTypes,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddManagerFor(typeof(ApizrManager<>),
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray(), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(Assembly[] assemblies,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddManagerFor(typeof(ApizrManager<>),
                assemblies, optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerImplementationType,
            Type[] assemblyMarkerTypes,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) =>
            AddManagerFor(apizrManagerImplementationType,
                assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray(), optionsBuilder);

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerImplementationType, Assembly[] assemblies,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        {
            if (assemblies?.Length is not > 0)
                throw new ArgumentException(
                    $"No assemblies found to scan.", nameof(assemblies));

            var webApiTypes = assemblies
                .Distinct()
                .SelectMany(assembly => assembly.GetTypes()) // All assembly types
                .Select(type => new
                {
                    Type = type,
                    Attribute = type.GetCustomAttributes<AutoRegisterAttribute>(true).FirstOrDefault()
                })
                .Where(item => item.Attribute != null) // Those actually decorated
                .Select(item => item.Attribute.WebApiType ?? item.Type) // Get the parameter type first, otherwise the decorated one
                .Where(type => type.IsInterface) // Keep only interfaces
                .ToList();

            foreach (var webApiType in webApiTypes)
                AddManagerFor(webApiType, apizrManagerImplementationType, optionsBuilder);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedRegistryBuilder AddManagerFor(
            Type webApiType, Type apizrManagerImplementationType,
            Action<IApizrExtendedProperOptionsBuilder> properOptionsBuilder = null)
        {
            var properOptions = ServiceCollectionExtensions.CreateProperOptions(CommonOptions, webApiType, apizrManagerImplementationType, properOptionsBuilder);
            var apizrManagerServiceType = Services.AddApizrManagerFor(CommonOptions, properOptions, ServiceCollectionExtensions.CreateManagerOptions(CommonOptions, properOptions));

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
