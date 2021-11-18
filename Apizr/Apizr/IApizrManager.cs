using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Mapping;
using Polly;

namespace Apizr
{
    public interface IApizrManager
    {}

    /// <summary>
    /// The manager encapsulating <see cref="TWebApi"/>'s task
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
        IApizrOptionsBase Options { get; }

        #region ExecuteAsync

        #region Task

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<TWebApi, Task>> executeApiMethod);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken cancellationToken = default);

        #endregion

        #region Task<T>

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(
            Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(Expression<Func<Context, TWebApi, Task<TResult>>> executeApiMethod,
            Context context = null);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, Context context = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData,
            Context context = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a managed <see cref="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, CancellationToken cancellationToken = default);

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
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The specific <see cref="TWebApi"/>'s task to clear cache for</param>
        /// <returns></returns>
        Task<bool> ClearCacheAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod);

        /// <summary>
        /// Clear the cache of a specific request
        /// </summary>
        /// <typeparam name="TResult">The <see cref="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The specific <see cref="TWebApi"/>'s task to clear cache for</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        Task<bool> ClearCacheAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken = default); 

        #endregion
    }
}
