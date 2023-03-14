using System;
using System.Collections.Generic;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Registry;
using Apizr.Connecting;
using Apizr.Mapping;
using Apizr.Requesting;
using Polly.Registry;

namespace Apizr;

/// <summary>
/// The builder
/// </summary>
public interface IApizrBuilder
{
    #region Registry

    /// <summary>
    /// Create a registry with all managers built with both common and proper options
    /// </summary>
    /// <param name="registryBuilder">The registry builder with access to proper options</param>
    /// <param name="commonOptionsBuilder">The common options shared by all managers</param>
    /// <returns></returns>
    IApizrRegistry CreateRegistry(Action<IApizrRegistryBuilder> registryBuilder,
        Action<IApizrCommonOptionsBuilder> commonOptionsBuilder = null);

    #endregion

    #region Crud


    /// <summary>
    /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
    /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
    /// and ReadAll query parameters of type IDictionary{string,object}
    /// </summary>
    /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> CreateCrudManagerFor<T>(
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null) where T : class;

    /// <summary>
    /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
    /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
    /// and ReadAll query parameters of type IDictionary{string,object}
    /// </summary>
    /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
    /// <typeparam name="TKey">The object key type (primitive)</typeparam>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey>(
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null) where T : class;

    /// <summary>
    /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
    /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
    /// and ReadAll query parameters of type IDictionary{string,object}
    /// </summary>
    /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
    /// <typeparam name="TKey">The object key type (primitive)</typeparam>
    /// <typeparam name="TReadAllResult">"ReadAll" query result type
    /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> CreateCrudManagerFor<T, TKey,
        TReadAllResult>(
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where T : class;

    /// <summary>
    /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
    /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
    /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
    /// </summary>
    /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
    /// <typeparam name="TKey">The object key type (primitive)</typeparam>
    /// <typeparam name="TReadAllResult">"ReadAll" query result type
    /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
    /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> CreateCrudManagerFor<T, TKey, TReadAllResult,
        TReadAllParams>(
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where T : class;

    /// <summary>
    /// Create a <typeparamref name="TApizrManager"/> instance for a managed crud api for <typeparamref name="T"/> object (class), 
    /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
    /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
    /// </summary>
    /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
    /// <typeparam name="TKey">The object key type (primitive)</typeparam>
    /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{ICrudApi}"/> implementation</typeparam>
    /// <typeparam name="TReadAllResult">"ReadAll" query result type
    /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
    /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
    /// <param name="apizrManagerFactory">The custom manager implementation instance factory</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    TApizrManager CreateCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
        Func<ILazyFactory<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>, IConnectivityHandler, ICacheHandler,
            IMappingHandler, IReadOnlyPolicyRegistry<string>, IApizrManagerOptions<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>,
            TApizrManager> apizrManagerFactory,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where T : class
        where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>;

    #endregion

    #region General

    /// <summary>
    /// Create a <see cref="ApizrManager{TWebApi}"/> instance
    /// </summary>
    /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    IApizrManager<TWebApi> CreateManagerFor<TWebApi>(
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null);

    /// <summary>
    /// Create a <typeparamref name="TApizrManager"/> instance for a managed <typeparamref name="TWebApi"/>
    /// </summary>
    /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
    /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
    /// <param name="apizrManagerFactory">The custom manager implementation instance factory</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    TApizrManager CreateManagerFor<TWebApi, TApizrManager>(
        Func<ILazyFactory<TWebApi>, IConnectivityHandler, ICacheHandler, IMappingHandler,
            IReadOnlyPolicyRegistry<string>, IApizrManagerOptions<TWebApi>, TApizrManager> apizrManagerFactory,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TApizrManager : IApizrManager<TWebApi>;

    #endregion
}