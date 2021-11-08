using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Requesting.Sending;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Sending
{
    public interface IApizrOptionalMediator : IApizrMediator
    {

    }

    /// <summary>
    /// <see cref="IMediator"/> but dedicated to <see cref="TWebApi"/> with optional result, getting all shorter
    /// </summary>
    public interface IApizrOptionalMediator<TWebApi> : IApizrOptionalMediator
    {
        #region SendFor

        /// <summary>
        /// Send an api call command to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod);

        /// <summary>
        /// Send an api call command to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null);

        /// <summary>
        /// Send a cancellable api call command to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default);

        /// <summary>
        /// Send a cancellable api call command to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context = null,
            CancellationToken token = default);

        #endregion

        #region SendFor<TApiResponse>

        /// <summary>
        /// Send an api call query to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <returns></returns>
        Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod);

        /// <summary>
        /// Send an api call query to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null);

        /// <summary>
        /// Send a cancellable api call query to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default);

        /// <summary>
        /// Send a cancellable api call query to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null,
            CancellationToken token = default);

        #endregion

        #region SendFor<TModelResponse, TApiResponse>

        /// <summary>
        /// Send an api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod);

        /// <summary>
        /// Send an api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null);

        /// <summary>
        /// Send a cancellable api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default);

        /// <summary>
        /// Send a mapped api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod);

        /// <summary>
        /// Send a cancellable api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null,
            CancellationToken token = default);

        /// <summary>
        /// Send a mapped api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod, Context context = null);

        /// <summary>
        /// Send a cancellable mapped api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default);

        /// <summary>
        /// Send a cancellable mapped api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="context">The Polly context</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod, Context context = null,
            CancellationToken token = default); 

        #endregion
    }
}