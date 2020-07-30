using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Fusillade;
using MediatR;
using Optional;

namespace Apizr.Optional.Requesting.Sending
{
    /// <summary>
    /// <see cref="IMediator"/> but dedicated to <see cref="TWebApi"/> with optional result, getting all shorter
    /// </summary>
    public interface IOptionalMediator<TWebApi>
    {
        /// <summary>
        /// Send an api call command to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        /// <summary>
        /// Send a cancellable api call command to Apizr with MediatR returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);

        /// <summary>
        /// Send an api call query to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        /// <summary>
        /// Send a cancellable api call query to Apizr with MediatR returning an optional result
        /// </summary>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);

        /// <summary>
        /// Send an api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        /// <summary>
        /// Send a cancellable api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);

        /// <summary>
        /// Send a mapped api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        /// <summary>
        /// Send a cancellable mapped api call query to Apizr with MediatR returning a mapped optional result
        /// </summary>
        /// <typeparam name="TModelResponse">The mapped model response</typeparam>
        /// <typeparam name="TApiResponse">The api response</typeparam>
        /// <param name="executeApiMethod">The <see cref="TWebApi"/> call to execute</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="priority">The execution priority</param>
        /// <returns></returns>
        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);
    }
}