using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Request;

namespace Apizr
{
    /// <summary>
    /// The manager definition
    /// </summary>
    public interface IApizrManager
    {}

    /// <summary>
    /// The manager encapsulating <typeparamref name="TWebApi"/>'s task
    /// </summary>
    /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
    public interface IApizrManager<TWebApi> : IApizrManager
    {
        /// <summary>
        /// Original Refit Api without any Apizr management. You should use ExecuteAsync instead to get all the Apizr goodness :)
        /// </summary>
        TWebApi Api { get; }

        /// <summary>
        /// Basic Apizr options
        /// </summary>
        IApizrManagerOptionsBase Options { get; }

        #region ExecuteAsync

        #region Task

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task ExecuteAsync(
            Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task ExecuteAsync(
            Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #region Task<T>

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TApiData> ExecuteAsync<TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TApiData> ExecuteAsync<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, 
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, 
            TModelRequestData modelRequestData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #endregion

        #region ClearCacheAsync

        /// <summary>
        /// Clear all cache
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<bool> ClearCacheAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Clear the cache of a specific request
        /// </summary>
        /// <typeparam name="TResult">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The specific <typeparamref name="TWebApi"/>'s task to clear cache for</param>
        /// <returns></returns>
        Task<bool> ClearCacheAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod);

        /// <summary>
        /// Clear the cache of a specific request
        /// </summary>
        /// <typeparam name="TResult">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The specific <typeparamref name="TWebApi"/>'s task to clear cache for</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<bool> ClearCacheAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken = default); 

        #endregion
    }
}
