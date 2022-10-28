using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Apizr.Extending
{
    public static class ApizrManagerExtensions
    {
        #region ExecuteAsync

        #region Task

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task ExecuteAsync<TWebApi>(this IApizrManager<TWebApi> manager, Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<Exception> onException)
            => manager.ExecuteAsync(api => executeApiMethod.Compile()(api),
                options => options.Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager, Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<Exception> onException)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (api, apiData) =>
                    executeApiMethod.Compile()(api, apiData), modelData,
                options => options.Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task ExecuteAsync<TWebApi>(this IApizrManager<TWebApi> manager, Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken cancellationToken = default, Action<Exception> onException = null)
            => manager.ExecuteAsync(
                (options, api) =>
                    executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.CancelWith(cancellationToken)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task ExecuteAsync<TWebApi>(this IApizrManager<TWebApi> manager, Expression<Func<Context, TWebApi, Task>> executeApiMethod,
            Context context = null, Action<Exception> onException = null)
            => manager.ExecuteAsync(
                (options, api) =>
                    executeApiMethod.Compile()(options.Context, api),
                options => options.WithContext(context)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken cancellationToken = default, Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.CancellationToken, api, apiData), modelData,
                options => options.CancelWith(cancellationToken)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            Context context = null, Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, api, apiData), modelData,
                options => options.WithContext(context)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task ExecuteAsync<TWebApi>(this IApizrManager<TWebApi> manager, Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null, CancellationToken cancellationToken = default, Action<Exception> onException = null)
            => manager.ExecuteAsync(
                (options, api) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithContext(context)
                    .CancelWith(cancellationToken)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken cancellationToken = default, Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData), modelData,
                options => options.WithContext(context)
                    .CancelWith(cancellationToken)
                    .Catch(onException));

        #endregion

        #region Task<T>

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> ExecuteAsync<TWebApi, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache)
            => manager.ExecuteAsync<TApiData>(api => executeApiMethod.Compile()(api),
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> ExecuteAsync<TWebApi, TApiData>(this IApizrManager<TWebApi> manager, 
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<Exception> onException)
            => manager.ExecuteAsync<TApiData>(api => executeApiMethod.Compile()(api),
                options => options.Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> ExecuteAsync<TWebApi, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            bool clearCache, Action<Exception> onException)
            => manager.ExecuteAsync<TApiData>(api => executeApiMethod.Compile()(api),
                options => options.ClearCache(clearCache).Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> ExecuteAsync<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache)
            => manager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelRequestData,
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> ExecuteAsync<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<Exception> onException)
            => manager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelRequestData,
                options => options.Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> ExecuteAsync<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException)
            => manager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelRequestData,
                options => options.ClearCache(clearCache).Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelData,
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager, 
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Action<Exception> onException)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelData,
                options => options.Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            bool clearCache, Action<Exception> onException)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelData,
                options => options.ClearCache(clearCache).Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache)
            => manager.ExecuteAsync<TModelData, TApiData>(
                api => executeApiMethod.Compile()(api),
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<Exception> onException)
            => manager.ExecuteAsync<TModelData, TApiData>(
                api => executeApiMethod.Compile()(api),
                options => options.Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache,
            Action<Exception> onException)
            => manager.ExecuteAsync<TModelData, TApiData>(
                api => executeApiMethod.Compile()(api),
                options => options.ClearCache(clearCache).Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> ExecuteAsync<TWebApi, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken cancellationToken = default, bool clearCache = false,
            Action<Exception> onException = null)
            => manager.ExecuteAsync<TApiData>(
                (options, api) => executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.CancelWith(cancellationToken).ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> ExecuteAsync<TWebApi, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false, Action<Exception> onException = null)
            => manager.ExecuteAsync<TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, api),
                options => options.WithContext(context).ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> ExecuteAsync<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(this IApizrManager<TWebApi> manager,
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken cancellationToken = default, bool clearCache = false,
            Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
                TModelRequestData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.CancellationToken, api, apiData),
                modelRequestData,
                options => options.CancelWith(cancellationToken).ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken cancellationToken = default, bool clearCache = false,
            Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.CancellationToken, api, apiData),
                modelData,
                options => options.CancelWith(cancellationToken).ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken cancellationToken = default, bool clearCache = false, Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.CancelWith(cancellationToken).ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> ExecuteAsync<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false,
            Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
                TModelRequestData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.Context, api, apiData),
                modelRequestData,
                options => options.WithContext(context).ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false, Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.Context, api, apiData), modelData,
                options => options.WithContext(context).ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false, Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, api),
                options => options.WithContext(context).ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApiData">The <typeparamref name="TWebApi"/>'s task result</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/>'s task to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> ExecuteAsync<TWebApi, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, CancellationToken cancellationToken = default, bool clearCache = false,
            Action<Exception> onException = null)
            => manager.ExecuteAsync<TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithContext(context).CancelWith(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelResultData">The mapped model result type</typeparam>
        /// <typeparam name="TApiResultData">The api result type</typeparam>
        /// <typeparam name="TApiRequestData">The mapped api request data type</typeparam>
        /// <typeparam name="TModelRequestData">The model request data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> ExecuteAsync<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context = null,
            CancellationToken cancellationToken = default, bool clearCache = false,
            Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
                TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData),
                modelRequestData,
                options => options.WithContext(context).CancelWith(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken cancellationToken = default, bool clearCache = false,
            Action<Exception> onException = null)
            => manager.ExecuteAsync<TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData), modelData,
                options => options.WithContext(context).ClearCache(clearCache)
                    .Catch(onException));

        /// <summary>
        /// Execute a managed <typeparamref name="TWebApi"/>'s task returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TModelData">The model data type</typeparam>
        /// <typeparam name="TApiData">The api data type</typeparam>
        /// <param name="manager">The api manager</param>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> ExecuteAsync<TWebApi, TModelData, TApiData>(this IApizrManager<TWebApi> manager,
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken cancellationToken = default, bool clearCache = false, Action<Exception> onException = null) =>
            manager.ExecuteAsync<TModelData, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithContext(context)
                    .CancelWith(cancellationToken)
                    .ClearCache(clearCache)
                    .Catch(onException));

        #endregion 

        #endregion
    }
}
