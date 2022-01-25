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
        IApizrOptionsBase Options { get; }

        #region ExecuteAsync

        #region Task

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task ExecuteAsync(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null,
            CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken cancellationToken = default, Action<Exception> onException = null);

        #endregion

        #region Task<T>

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> ExecuteAsync<TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> ExecuteAsync<TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> ExecuteAsync<TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache = false, CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> ExecuteAsync<TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null, bool clearCache = false,
            CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData,
            Context context = null, bool clearCache = false, CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            Context context = null, bool clearCache = false, CancellationToken cancellationToken = default, Action<Exception> onException = null);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false, CancellationToken cancellationToken = default, Action<Exception> onException = null);

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
