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
    public interface IOptionalMediator<TWebApi>
    {

        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);

        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated);

        Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default,
            Priority priority = Priority.UserInitiated);
    }
}
