using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Apizr.Mediation.Requesting.Sending
{
    /// <summary>
    /// Apizr mediator extensions to send request using MediatR by calling expression
    /// </summary>
    public static class ApizrMediatorExtensions
    {
        #region Untyped

        #region Unit

        #region SendFor<TWebApi>

        /// <summary>
        /// Send an api call to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi>(this IApizrMediator mediator,
            Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException) =>
            mediator.SendFor<TWebApi>(executeApiMethod, options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi>(this IApizrMediator mediator,
            Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null,
            Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi>((options, api) => executeApiMethod.Compile()(options.Context, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi>(this IApizrMediator mediator,
            Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi>((options, api) => executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi>(this IApizrMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi>(
                (options, api) => executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<Exception> onException) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.CancellationToken, api, apiData),
                modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a Polly Context
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.Context, api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request, a Polly Context and cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken token = default, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator mediator,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<Exception> onException) =>
            mediator.SendFor<TWebApi, TApiData>(executeApiMethod, options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator mediator,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache,
            Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TApiData>(executeApiMethod, options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator mediator,
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TApiData>((options, api) => executeApiMethod.Compile()(options.Context, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator mediator,
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<Exception> onException) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache,
            Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<Exception> onException) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            bool clearCache, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.Context, api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.CancellationToken, api, apiData),
                modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData),
                modelData,
                options => options.WithExceptionCatcher(onException));

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator mediator,
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<Exception> onException) =>
            mediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator mediator,
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator mediator,
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.CancellationToken, api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator mediator,
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false,
            Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        #endregion

        #endregion

        #endregion

        #region Typed

        #region Unit

        #region SendFor<TWebApi>

        /// <summary>
        /// Send an api call to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException) =>
            mediator.SendFor(executeApiMethod, options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null,
            Action<Exception> onException = null) =>
            mediator.SendFor((options, api) => executeApiMethod.Compile()(options.Context, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default, Action<Exception> onException = null) =>
            mediator.SendFor((options, api) => executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, Action<Exception> onException = null) =>
            mediator.SendFor(
                (options, api) => executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="onException">Handle exception</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<Exception> onException) =>
            mediator.SendFor<TModelData, TApiData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.CancellationToken, api, apiData),
                modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a Polly Context
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.Context, api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request, a Polly Context and cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken token = default, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<Exception> onException) =>
            mediator.SendFor<TApiData>(executeApiMethod, options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache,
            Action<Exception> onException = null) =>
            mediator.SendFor<TApiData>(executeApiMethod, options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TApiData>((options, api) => executeApiMethod.Compile()(options.Context, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TApiData>(
                (options, api) => executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TApiData> SendFor<TWebApi, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<Exception> onException) =>
            mediator.SendFor<TModelData, TApiData>(executeApiMethod,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache,
            Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(executeApiMethod,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (options, api) => executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<Exception> onException) =>
            mediator.SendFor<TModelData, TApiData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            bool clearCache, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (api, apiData) => executeApiMethod.Compile()(api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.Context, api, apiData), modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (options, api, apiData) => executeApiMethod.Compile()(options.CancellationToken, api, apiData),
                modelData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData),
                modelData,
                options => options.WithExceptionCatcher(onException));

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="onException">Handle exception and return cached result</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<Exception> onException) =>
            mediator.SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator<TWebApi> mediator,
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException = null) =>
            mediator.SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator<TWebApi> mediator,
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.CancellationToken, api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false,
            Action<Exception> onException = null) =>
            mediator.SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            this IApizrMediator<TWebApi> mediator,
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null) =>
            mediator.SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData),
                modelRequestData,
                options => options.WithExceptionCatcher(onException));

        #endregion

        #endregion

        #endregion
    }
}