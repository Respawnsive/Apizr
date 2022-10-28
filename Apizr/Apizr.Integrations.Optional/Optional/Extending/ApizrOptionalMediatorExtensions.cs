using MediatR;
using Optional;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Apizr.Optional.Requesting.Sending;

namespace Apizr.Optional.Extending
{
    public static class ApizrOptionalMediatorExtensions
    {
        #region Unit

        #region SendFor<TWebApi>

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendFor<TWebApi>(this IApizrOptionalMediator mediator,
            Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null) =>
            mediator.SendFor<TWebApi>((options, api) => executeApiMethod.Compile()(options.Context, api),
                options => options.WithContext(context));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendFor<TWebApi>(this IApizrOptionalMediator mediator,
            Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default) =>
            mediator.SendFor<TWebApi>((options, api) => executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.CancelWith(token));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendFor<TWebApi>(this IApizrOptionalMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null,
            CancellationToken token = default) =>
            mediator.SendFor<TWebApi>(
                (options, api) => executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithContext(context).CancelWith(token));

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(
            this IApizrOptionalMediator mediator, Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (api, apiData) =>
                    executeApiMethod.Compile()(api, apiData), modelData);

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="token">A cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(this IApizrOptionalMediator mediator,
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.CancellationToken, api, apiData), modelData,
                options => options.CancelWith(token));

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a Polly Context and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(this IApizrOptionalMediator mediator, Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, api, apiData), modelData,
                options => options.WithContext(context));

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request, a Polly Context and cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="token">A cancellation token</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(
            this IApizrOptionalMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken token = default) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData), modelData,
                options => options.WithContext(context).CancelWith(token));

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            this IApizrOptionalMediator mediator, Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            bool clearCache = false) =>
            mediator.SendFor<TWebApi, TApiData>(
                api =>
                    executeApiMethod.Compile()(api),
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            this IApizrOptionalMediator mediator, Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TApiData>(
                (options, api) =>
                    executeApiMethod.Compile()(options.Context, api),
                options => options.WithContext(context).ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            this IApizrOptionalMediator mediator,
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TApiData>(
                (options, api) =>
                    executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.CancelWith(token).ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            this IApizrOptionalMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TApiData>(
                (options, api) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithContext(context).CancelWith(token).ClearCache(clearCache));

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(this IApizrOptionalMediator mediator,
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                api =>
                    executeApiMethod.Compile()(api),
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            this IApizrOptionalMediator mediator,
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api) =>
                    executeApiMethod.Compile()(options.Context, api),
                options => options.WithContext(context).ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            this IApizrOptionalMediator mediator,
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api) =>
                    executeApiMethod.Compile()(options.CancellationToken, api),
                options => options.CancelWith(token).ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            this IApizrOptionalMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api),
                options => options.WithContext(context).CancelWith(token).ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            this IApizrOptionalMediator mediator,
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(api, apiData), modelData,
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            this IApizrOptionalMediator mediator,
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, api, apiData), modelData,
                options => options.WithContext(context).ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(this IApizrOptionalMediator mediator,
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken token = default, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.CancellationToken, api, apiData), modelData,
                options => options.CancelWith(token).ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            this IApizrOptionalMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelData, TApiData>(
                (options, api, apiData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiData), modelData,
                options => options.WithContext(context).CancelWith(token).ClearCache(clearCache));

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(this IApizrOptionalMediator mediator,
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (api, apiRequestData) =>
                    executeApiMethod.Compile()(api, apiRequestData),
                modelRequestData,
                options => options.ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning an optional mapped result
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
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi,
            TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(this IApizrOptionalMediator mediator,
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData,
            CancellationToken token = default, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiRequestData) =>
                    executeApiMethod.Compile()(options.CancellationToken, api, apiRequestData),
                modelRequestData,
                options => options.CancelWith(token).ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning an optional mapped result
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
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(this IApizrOptionalMediator mediator,
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiRequestData) =>
                    executeApiMethod.Compile()(options.Context, api, apiRequestData),
                modelRequestData,
                options => options.WithContext(context).ClearCache(clearCache));

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning an optional mapped result
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
        /// <returns></returns>
        [Obsolete("Use the one with the request options builder parameter instead")]
        public static Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi,
            TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(this IApizrOptionalMediator mediator,
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context = null,
            CancellationToken token = default, bool clearCache = false) =>
            mediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (options, api, apiRequestData) =>
                    executeApiMethod.Compile()(options.Context, options.CancellationToken, api, apiRequestData),
                modelRequestData,
                options => options.WithContext(context).CancelWith(token).ClearCache(clearCache));

        #endregion

        #endregion
    }
}
