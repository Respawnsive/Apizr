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
    public class ApizrRegistryBuilder : IApizrRegistryBuilder
    {
        protected readonly ApizrRegistry Registry = new ApizrRegistry();
        protected readonly IApizrCommonOptions CommonOptions;

        internal ApizrRegistryBuilder(IApizrCommonOptions commonOptions)
        {
            CommonOptions = commonOptions;
        }

        public IApizrRegistry ApizrRegistry => Registry;

        #region Crud

        public IApizrRegistryBuilder AddCrudFor<T>(Action<IApizrProperOptionsBuilder> optionsBuilder = null) where T : class =>
            AddFor<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), optionsBuilder);

        public IApizrRegistryBuilder AddCrudFor<T, TKey>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null) where T : class =>
            AddFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), properOptionsBuilder);

        public IApizrRegistryBuilder AddCrudFor<T, TKey,
            TReadAllResult>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class =>
            AddFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), properOptionsBuilder);

        public IApizrRegistryBuilder AddCrudFor<T, TKey, TReadAllResult,
            TReadAllParams>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class =>
            AddFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>,
                ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(lazyWebApi,
                        connectivityHandler,
                        cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), properOptionsBuilder);

        public IApizrRegistryBuilder AddCrudFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Func<ILazyFactory<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
                IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrOptionsBase,
                TApizrManager> apizrManagerFactory,
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class
            where TReadAllParams : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> =>
            AddFor(apizrManagerFactory, properOptionsBuilder);

        #endregion

        #region General

        public IApizrRegistryBuilder AddFor<TWebApi>(
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null) =>
            AddFor<TWebApi, ApizrManager<TWebApi>>(
                (lazyWebApi, connectivityHandler, cacheHandler, mappingHandler, policyRegistry, apizrOptions) =>
                    new ApizrManager<TWebApi>(lazyWebApi, connectivityHandler, cacheHandler, mappingHandler,
                        policyRegistry, apizrOptions), properOptionsBuilder);

        public IApizrRegistryBuilder AddFor<TWebApi, TApizrManager>(Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                IReadOnlyPolicyRegistry<string>, IApizrOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<IApizrProperOptionsBuilder> properOptionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi>
        {
            var properOptions = Apizr.CreateApizrProperOptions<TWebApi>(CommonOptions, properOptionsBuilder);
            var managerFactory = new Func<IApizrManager<TWebApi>>(() => Apizr.CreateFor(apizrManagerFactory, CommonOptions, properOptions));
            Registry.AddOrUpdateFor(managerFactory);

            return this;
        } 

        #endregion
    }
}
