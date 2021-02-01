using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using MediatR;

namespace Apizr.Mediation.Requesting.Sending
{
    public class Mediator<TWebApi> : IMediator<TWebApi>
    {
        private readonly IMediator _mediator;

        public Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task SendFor(Expression<Func<TWebApi, Task>> executeApiMethod)
        {
            return _mediator.Send(new ExecuteRequest<TWebApi>(executeApiMethod));
        }

        public Task SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default)
        {
            return _mediator.Send(new ExecuteRequest<TWebApi>(executeApiMethod), token);
        }

        public Task<TApiResponse> SendFor<TApiResponse>(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod)
        {
            return _mediator.Send(new ExecuteRequest<TWebApi, TApiResponse>(executeApiMethod));
        }

        public Task<TApiResponse> SendFor<TApiResponse>(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default)
        {
            return _mediator.Send(new ExecuteRequest<TWebApi, TApiResponse>(executeApiMethod), token);
        }

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod)
        {
            return _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod));
        }

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, CancellationToken token = default)
        {
            return _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod), token);
        }

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod)
        {
            return _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod));
        }

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default)
        {
            return _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod), token);
        }
    }
}
