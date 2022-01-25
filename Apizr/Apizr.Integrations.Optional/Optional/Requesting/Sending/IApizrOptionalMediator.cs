using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Sending
{
    /// <summary>
    /// Apizr mediator to send request using MediatR by calling expression and returning optional result
    /// </summary>
    public interface IApizrOptionalMediator : IApizrOptionalMediatorBase
    {
        #region Unit

        #region SendFor<TWebApi>

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<TWebApi, Task>> executeApiMethod);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null,
            CancellationToken token = default);

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
        Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData);

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
        Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default);

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
        Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null);

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
        Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken token = default);

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
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false,
            CancellationToken token = default);

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
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false);

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
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false,
            CancellationToken token = default);

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
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false);

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
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">A cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache = false,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">A cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null, bool clearCache = false,
            CancellationToken token = default);

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
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false);

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
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false,
            CancellationToken token = default);

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
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false);

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
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context = null, bool clearCache = false,
            CancellationToken token = default);

        #endregion

        #endregion
    }

    /// <summary>
    /// <see cref="IApizrOptionalMediator"/> but dedicated to <typeparamref name="TWebApi"/> with optional result, getting all shorter
    /// </summary>
    public interface IApizrOptionalMediator<TWebApi> : IApizrOptionalMediatorBase
    {
        #region Unit

        #region SendFor<TWebApi>

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null,
            CancellationToken token = default);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and returning an optional result
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData);

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="token">A cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a Polly Context and returning an optional result
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request, a Polly Context and cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="token">A cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken token = default);

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning an optional result
        /// </summary>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning an optional result
        /// </summary>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false,
            CancellationToken token = default);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">A cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache = false,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">A cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null, bool clearCache = false,
            CancellationToken token = default);

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false,
            CancellationToken token = default);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context = null, bool clearCache = false,
            CancellationToken token = default);

        #endregion

        #endregion
    }
}