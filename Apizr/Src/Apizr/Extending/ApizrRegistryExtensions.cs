using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Registry;
using Apizr.Configuring.Request;
using Refit;

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

        #region Crud

        #region Create

        /// <summary>
        /// Send a Create request
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="entity">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TApiEntity> CreateAsync<TApiEntity>(this IApizrEnumerableRegistry registry,
            TApiEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity>()
                .ExecuteAsync((options, api) => api.Create(entity, options), optionsBuilder);


        /// <summary>
        /// Send a mapped Create request, returning a mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="entity">The entity to create</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelEntity> CreateAsync<TModelEntity, TApiEntity>(this IApizrEnumerableRegistry registry,
            TModelEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity>()
                .ExecuteAsync<TModelEntity, TApiEntity>((options, api, apiEntity) => api.Create(apiEntity, options),
                    entity, optionsBuilder);

        #endregion

        #region ReadAll

        /// <summary>
        /// Send a ReadAll request
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TReadAllResult> ReadAllAsync<TApiEntity, TApiEntityKey, TReadAllResult>(
            this IApizrEnumerableRegistry registry, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity, TApiEntityKey, TReadAllResult>()
                .ExecuteAsync((options, api) => api.ReadAll(options), optionsBuilder);

        /// <summary>
        /// Send a ReadAll request, returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelReadAllResult> ReadAllAsync<TModelReadAllResult, TApiEntity, TApiEntityKey,
            TApiReadAllResult>(this IApizrEnumerableRegistry registry,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity, TApiEntityKey, TApiReadAllResult>()
                .ExecuteAsync<TModelReadAllResult, TApiReadAllResult>((options, api) => api.ReadAll(options),
                    optionsBuilder);

        /// <summary>
        /// Send a ReadAll request with some parameters
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <typeparam name="TReadAllResult">The api result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TReadAllResult> ReadAllAsync<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>(
            this IApizrEnumerableRegistry registry, TReadAllParams readAllParams,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>()
                .ExecuteAsync((options, api) => api.ReadAll(readAllParams, options), optionsBuilder);

        /// <summary>
        /// Send a ReadAll request with some parameters, returning mapped result
        /// </summary>
        /// <typeparam name="TModelReadAllResult">The model result type to map to</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <typeparam name="TApiReadAllResult">The api result type to map from</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="readAllParams">The read all filters</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelReadAllResult> ReadAllAsync<TModelReadAllResult, TApiEntity, TApiEntityKey,
            TApiReadAllResult, TReadAllParams>(this IApizrEnumerableRegistry registry, TReadAllParams readAllParams,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity, TApiEntityKey, TApiReadAllResult, TReadAllParams>()
                .ExecuteAsync<TModelReadAllResult, TApiReadAllResult>(
                    (options, api) => api.ReadAll(readAllParams, options),
                    optionsBuilder);

        #endregion

        #region Read

        /// <summary>
        /// Send a Read request
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TApiEntity> ReadAsync<TApiEntity, TApiEntityKey>(this IApizrEnumerableRegistry registry,
            TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity, TApiEntityKey>()
                .ExecuteAsync((options, api) => api.Read(key, options), optionsBuilder);

        /// <summary>
        /// Send a Read request, returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task<TModelEntity> ReadAsync<TModelEntity, TApiEntity, TApiEntityKey>(
            this IApizrEnumerableRegistry registry, TApiEntityKey key,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity, TApiEntityKey>()
                .ExecuteAsync<TModelEntity, TApiEntity>((options, api) => api.Read(key, options), optionsBuilder);

        #endregion

        #region Update

        /// <summary>
        /// Send an Update request
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task UpdateAsync<TApiEntity, TApiEntityKey>(this IApizrEnumerableRegistry registry,
            TApiEntityKey key,
            TApiEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity, TApiEntityKey>()
                .ExecuteAsync((options, api) => api.Update(key, entity, options), optionsBuilder);

        /// <summary>
        /// Send a mapped Update request, returning mapped result
        /// </summary>
        /// <typeparam name="TModelEntity">The model entity type to map from</typeparam>
        /// <typeparam name="TApiEntity">The api entity type to map to</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="key">The entity key</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task UpdateAsync<TModelEntity, TApiEntity, TApiEntityKey>(this IApizrEnumerableRegistry registry,
            TApiEntityKey key,
            TModelEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity, TApiEntityKey>()
                .ExecuteAsync<TModelEntity, TApiEntity>(
                    (options, api, apiEntity) => api.Update(key, apiEntity, options), entity, optionsBuilder);

        #endregion

        #region Delete

        /// <summary>
        /// Send an Delete request
        /// </summary>
        /// <typeparam name="TApiEntity">The api entity type</typeparam>
        /// <typeparam name="TApiEntityKey">The entity's crud key type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="key">The entity key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        public static Task DeleteAsync<TApiEntity, TApiEntityKey>(this IApizrEnumerableRegistry registry,
            TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TApiEntity : class
            => registry.GetCrudManagerFor<TApiEntity, TApiEntityKey>()
                .ExecuteAsync((options, api) => api.Delete(key, options), optionsBuilder);

        #endregion

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
