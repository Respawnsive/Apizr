using System;
using System.Collections.Generic;
using System.Reflection;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Proper;
using Apizr.Connecting;
using Apizr.Mapping;
using Apizr.Requesting;
using Polly.Registry;

namespace Apizr.Configuring.Registry
{
    /// <summary>
    /// Registry builder options available for static registrations
    /// </summary>
    public class ApizrRegistryBuilder : IApizrRegistryBuilder, IApizrInternalRegistryBuilder<IApizrProperOptionsBuilder>
    {
        /// <summary>
        /// The registry
        /// </summary>
        protected readonly ApizrRegistry Registry;

        /// <summary>
        /// The common options
        /// </summary>
        protected readonly IApizrCommonOptions CommonOptions;

        internal ApizrRegistryBuilder(IApizrCommonOptions commonOptions, ApizrRegistry mainRegistry = null)
        {
            CommonOptions = commonOptions;
            Registry = mainRegistry ?? new ApizrRegistry();
        }

        #region Registry

        /// <inheritdoc />
        public IApizrRegistry ApizrRegistry => Registry;

        /// <inheritdoc />
        public IApizrRegistryBuilder AddGroup(Action<IApizrRegistryBuilder> registryGroupBuilder,
            Action<IApizrCommonOptionsBuilder> commonOptionsBuilder = null)
        {
            ApizrBuilder.Instance.CreateRegistry(registryGroupBuilder, CommonOptions, commonOptionsBuilder, Registry);

            return this;
        }

        #endregion

        #region Crud

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T>(Action<IApizrProperOptionsBuilder> optionsBuilder = null) where T : class =>
            AddCrudManagerFor<T, int, IEnumerable<T>, IDictionary<string, object>,
                ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T, TKey>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null) where T : class =>
            AddCrudManagerFor<T, TKey, IEnumerable<T>, IDictionary<string, object>,
                ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), properOptionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T, TKey,
            TReadAllResult>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class =>
            AddCrudManagerFor<T, TKey, TReadAllResult, IDictionary<string, object>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), properOptionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult,
            TReadAllParams>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class =>
            AddCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), properOptionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Func<ILazyFactory<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
                IMappingHandler, ILazyFactory<IReadOnlyPolicyRegistry<string>>, IApizrManagerOptions<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>,
                TApizrManager> apizrManagerFactory,
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>
        {
            var crudedType = typeof(T);

            var crudAttribute = crudedType.GetCustomAttribute<CrudEntityAttribute>();
            if (crudAttribute != null)
            {
                if (properOptionsBuilder == null)
                    properOptionsBuilder = builder => builder.WithBaseAddress(crudAttribute.BaseUri);
                else
                    properOptionsBuilder += builder => builder.WithBaseAddress(crudAttribute.BaseUri, ApizrDuplicateStrategy.Ignore);
            }

            return AddManagerFor(apizrManagerFactory, properOptionsBuilder);
        }

        #endregion

        #region General

        /// <inheritdoc />
        public IApizrRegistryBuilder AddManagerFor<TWebApi>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null) =>
            AddManagerFor<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, lazyPolicyRegistry, apizrOptions) =>
                    new ApizrManager<TWebApi>(lazyWebApi, connectivityHandler, cacheHandler, mappingHandler,
                        lazyPolicyRegistry, apizrOptions), properOptionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddManagerFor<TWebApi, TApizrManager>(Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                ILazyFactory<IReadOnlyPolicyRegistry<string>>, IApizrManagerOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi>
        {
            var properOptions = ApizrBuilder.Instance.CreateProperOptions<TWebApi>(CommonOptions, properOptionsBuilder);
            var managerFactory = new Func<IApizrManager<TWebApi>>(() => ApizrBuilder.Instance.CreateManagerFor(apizrManagerFactory, CommonOptions, properOptions));
            Registry.AddOrUpdateManagerFor(managerFactory);

            return this;
        }

        #endregion

        #region Internal

        /// <inheritdoc />
        void IApizrInternalRegistryBuilder<IApizrProperOptionsBuilder>.AddWrappingManagerFor<TWebApi, TWrappingManager>(
            Func<IApizrManager<TWebApi>, TWrappingManager> wrappingManagerFactory,
            Action<IApizrProperOptionsBuilder> optionsBuilder = null)
        {
            AddManagerFor<TWebApi>(optionsBuilder);
            var managerFactory = new Func<IApizrManager>(() => wrappingManagerFactory.Invoke(Registry.GetManagerFor<TWebApi>()));

            Registry.AddOrUpdateManager(typeof(TWrappingManager), managerFactory);
        }

        /// <inheritdoc />
        void IApizrInternalGlobalRegistryBuilder.AddAliasingManagerFor<TAliasingManager, TAliasedManager>() =>
            Registry.AddOrUpdateManager(typeof(TAliasingManager),
                () => Registry.ConcurrentRegistry[typeof(TAliasedManager)].Invoke());

        #endregion
    }
}
