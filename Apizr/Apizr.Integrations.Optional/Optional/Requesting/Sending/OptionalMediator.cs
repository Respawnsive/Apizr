using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using MediatR;
using Optional;

namespace Apizr.Optional.Requesting.Sending
{
    public class OptionalMediator<TWebApi> : IOptionalMediator<TWebApi>
    {
        private readonly IMediator _mediator;

        public OptionalMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod)
        {
            return _mediator.Send(new ExecuteOptionalRequest<TWebApi>(executeApiMethod));
        }

        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken token = default)
        {
            return _mediator.Send(new ExecuteOptionalRequest<TWebApi>(executeApiMethod), token);
        }

        public Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod)
        {
            return _mediator.Send(new ExecuteOptionalRequest<TWebApi, TApiResponse>(executeApiMethod));
        }

        public Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, CancellationToken token = default)
        {
            return _mediator.Send(new ExecuteOptionalRequest<TWebApi, TApiResponse>(executeApiMethod), token);
        }

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod)
        {
            return _mediator.Send(new ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod));
        }

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, CancellationToken token = default)
        {
            return _mediator.Send(new ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod), token);
        }

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod)
        {
            return _mediator.Send(new ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod));
        }

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod, CancellationToken token = default)
        {
            return _mediator.Send(new ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod), token);
        }
    }
}