using Apizr.Caching;
using Apizr.Configuring.Proper;
using Apizr.Connecting;
using Apizr.Mapping;
using Polly.Registry;
using System;
using Apizr.Requesting;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistryBuilder<out TApizrRegistry, out TApizrRegistryBuilder, out TApizrProperOptionsBuilder> : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder>
        where TApizrRegistry : IApizrEnumerableRegistry
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder>
        where TApizrProperOptionsBuilder : IApizrProperOptionsBuilderBase
    {
        #region Crud
        
        /// <summary>
        /// Create a <see cref="TApizrManager"/> instance for a managed crud api for <see cref="T"/> object (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{ICrudApi}"/> implementation</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <param name="apizrManagerFactory">The custom manager implementation instance factory</param>
        /// <param name="properOptionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddCrudFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Func<ILazyFactory<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
                IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrOptionsBase,
                TApizrManager> apizrManagerFactory,
            Action<TApizrProperOptionsBuilder> properOptionsBuilder = null)
            where T : class
            where TReadAllParams : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>;

        #endregion

        #region General
        
        /// <summary>
        /// Create a <see cref="TApizrManager"/> instance for a managed <see cref="TWebApi"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="apizrManagerFactory">The custom manager implementation instance factory</param>
        /// <param name="properOptionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddFor<TWebApi, TApizrManager>(
            Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
                IReadOnlyPolicyRegistry<string>, IApizrOptions<TWebApi>, TApizrManager> apizrManagerFactory,
            Action<TApizrProperOptionsBuilder> properOptionsBuilder = null)
            where TApizrManager : IApizrManager<TWebApi>; 

        #endregion
    }

    public interface IApizrRegistryBuilder : IApizrRegistryBuilder<IApizrRegistry, IApizrRegistryBuilder, IApizrProperOptionsBuilder>
    {
    }
}
