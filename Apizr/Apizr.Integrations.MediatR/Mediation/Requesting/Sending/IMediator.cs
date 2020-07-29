using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Requesting.Sending
{
    /// <summary>
    /// <see cref="IMediator"/> but dedicated to <see cref="TWebApi"/>, getting all shorter
    /// </summary>
    /// <typeparam name="TWebApi">The api interface to play with mediation</typeparam>
    public interface IMediator<TWebApi>
    {
        Task SendFor(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        Task SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);

        Task<TApiResponse> SendFor<TApiResponse>(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        Task<TApiResponse> SendFor<TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);

        Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);

        Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);
    }
}
