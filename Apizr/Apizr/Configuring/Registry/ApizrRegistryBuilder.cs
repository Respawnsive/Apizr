using System;
using System.Collections.Generic;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Connecting;
using Apizr.Mapping;
using Apizr.Requesting;
using Polly.Registry;

namespace Apizr.Configuring.Registry
{
    /// <inheritdoc />
    public class ApizrRegistryBuilder : IApizrRegistryBuilder
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
            ApizrBuilder.CreateRegistry(registryGroupBuilder, CommonOptions, commonOptionsBuilder, Registry);

            return this;
        }

        #endregion

        #region Crud

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T>(Action<IApizrProperOptionsBuilder> optionsBuilder = null) where T : class =>
            AddManagerFor<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), optionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T, TKey>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null) where T : class =>
            AddManagerFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), properOptionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T, TKey,
            TReadAllResult>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class =>
            AddManagerFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), properOptionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult,
            TReadAllParams>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class =>
            AddManagerFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), properOptionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Func<ILazyFactory<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
                IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrOptionsBase,
                TApizrManager> apizrManagerFactory,
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class
            where TReadAllParams : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            AddManagerFor(apizrManagerFactory, properOptionsBuilder);

        #endregion

        #region General

        /// <inheritdoc />
        public IApizrRegistryBuilder AddManagerFor<TWebApi>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null) =>
            AddManagerFor<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<TWebApi>(lazyWebApi, connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), properOptionsBuilder);

        /// <inheritdoc />
        public IApizrRegistryBuilder AddManagerFor<TWebApi, TApizrManager>(Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                IReadOnlyPolicyRegistry<string>, IApizrOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi>
        {
            var properOptions = ApizrBuilder.CreateProperOptions<TWebApi>(CommonOptions, properOptionsBuilder);
            var managerFactory = new Func<IApizrManager<TWebApi>>(() => ApizrBuilder.CreateManagerFor(apizrManagerFactory, CommonOptions, properOptions));
            Registry.AddOrUpdateManagerFor(managerFactory);

            return this;
        } 

        #endregion
    }
}
