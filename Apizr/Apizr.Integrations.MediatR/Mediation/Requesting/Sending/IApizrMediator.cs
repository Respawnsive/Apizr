using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Sending
{
    /// <summary>
    /// Apizr mediator to send request using MediatR by calling expression
    /// </summary>
    public interface IApizrMediator : IApizrMediatorBase
    {
        #region Unit

        #region SendFor<TWebApi>

        /// <summary>
        /// Send an api call to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor<TWebApi>(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor<TWebApi>(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor<TWebApi>(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor<TWebApi>(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, Action<Exception> onException = null);

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
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<Exception> onException = null);

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
        Task SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default, Action<Exception> onException = null);

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
        Task SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null, Action<Exception> onException = null);

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
        Task SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken token = default, Action<Exception> onException = null);

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
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> SendFor<TWebApi, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TApiData> SendFor<TWebApi, TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> SendFor<TWebApi, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> SendFor<TWebApi, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

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
        Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

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
        Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false, Action<Exception> onException = null);

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
        Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

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
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, Action<Exception> onException = null);

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
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

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
        Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false, Action<Exception> onException = null);

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
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context = null, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        #endregion

        #endregion
    }

    /// <summary>
    /// <see cref="IApizrMediator"/> but dedicated to <typeparamref name="TWebApi"/>, getting all shorter
    /// </summary>
    /// <typeparam name="TWebApi">The api interface to play with MediatR</typeparam>
    public interface IApizrMediator<TWebApi> : IApizrMediatorBase
    {
        #region Unit

        #region SendFor

        /// <summary>
        /// Send an api call to Apizr using MediatR
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, Action<Exception> onException = null);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a cancellation token
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and a Polly Context
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor<TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request, a Polly Context and cancellation token
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception (default: null = throwing)</param>
        /// <returns></returns>
        Task SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Context context = null, CancellationToken token = default, Action<Exception> onException = null);

        #endregion

        #endregion

        #region Result

        #region SendFor<TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR
        /// </summary>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> SendFor<TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context
        /// </summary>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> SendFor<TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token
        /// </summary>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> SendFor<TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token
        /// </summary>
        /// <typeparam name="TApiData">The api response</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TApiData> SendFor<TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        #endregion

        #region SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a cancellation token and returning a mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="clearCache">Clear request cache before executing (default: false)</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and a Polly Context and returning a mapped result
        /// </summary>
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
        Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false, Action<Exception> onException = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request, a Polly Context and a cancellation token and returning a mapped result
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
        /// <param name="onException">Handle exception and return cached result (default: null = throwing)</param>
        /// <returns></returns>
        Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context = null, bool clearCache = false,
            CancellationToken token = default, Action<Exception> onException = null);

        #endregion

        #endregion
    }
}