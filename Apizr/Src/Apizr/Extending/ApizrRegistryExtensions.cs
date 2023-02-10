using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Registry;
using Apizr.Configuring.Request;

namespace Apizr.Extending
{
    public static class ApizrRegistryExtensions
    {
        #region ExecuteAsync

        #region Task

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task ExecuteAsync<TWebApi>(this IApizrEnumerableRegistry registry,
            Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>().ExecuteAsync(executeApiMethod, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task ExecuteAsync<TWebApi>(this IApizrEnumerableRegistry registry,
            Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>().ExecuteAsync(executeApiMethod, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrEnumerableRegistry registry,
            Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>()
                .ExecuteAsync<TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrEnumerableRegistry registry,
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>()
                .ExecuteAsync<TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        #endregion

        #region Task<T>

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TApiData> ExecuteAsync<TWebApi, TApiData>(this IApizrEnumerableRegistry registry,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>().ExecuteAsync<TApiData>(executeApiMethod, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelResultData> ExecuteAsync<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrEnumerableRegistry registry,
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>()
                .ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(executeApiMethod,
                    modelRequestData, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(
            this IApizrEnumerableRegistry registry,
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>()
                .ExecuteAsync<TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(
            this IApizrEnumerableRegistry registry,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>().ExecuteAsync<TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TApiData> ExecuteAsync<TWebApi, TApiData>(
            this IApizrEnumerableRegistry registry,
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>().ExecuteAsync<TApiData>(executeApiMethod, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelResultData> ExecuteAsync<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrEnumerableRegistry registry,
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>()
                .ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(executeApiMethod,
                    modelRequestData, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(
            this IApizrEnumerableRegistry registry,
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>()
                .ExecuteAsync<TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(
            this IApizrEnumerableRegistry registry,
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetManagerFor<TWebApi>().ExecuteAsync<TModelData, TApiData>(executeApiMethod, optionsBuilder);

        #endregion

        #endregion

        #region ClearCacheAsync

        /// <summary>
        /// Clear all cache
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public static Task<bool> ClearCacheAsync<TWebApi>(this IApizrEnumerableRegistry registry,
            CancellationToken cancellationToken = default)
            => registry.GetManagerFor<TWebApi>().ClearCacheAsync(cancellationToken);

        /// <summary>
        /// Clear the cache of a specific request
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TResult">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The specific <typeparamref name="TWebApi"/>'s task to clear cache for</param>
        /// <returns></returns>
        public static Task<bool> ClearCacheAsync<TWebApi, TResult>(this IApizrEnumerableRegistry registry,
            Expression<Func<TWebApi, Task<TResult>>> executeApiMethod)
            => registry.GetManagerFor<TWebApi>().ClearCacheAsync<TResult>(executeApiMethod);

        /// <summary>
        /// Clear the cache of a specific request
        /// </summary>
        /// <typeparam name="TWebApi">The web api to manage</typeparam>
        /// <typeparam name="TResult">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="executeApiMethod">The specific <typeparamref name="TWebApi"/>'s task to clear cache for</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public static Task<bool> ClearCacheAsync<TWebApi, TResult>(this IApizrEnumerableRegistry registry,
            Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod,
            CancellationToken cancellationToken = default)
            => registry.GetManagerFor<TWebApi>().ClearCacheAsync<TResult>(executeApiMethod, cancellationToken);

        #endregion
    }
}
