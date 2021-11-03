using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Sending
{
    public class Mediator<TWebApi> : IMediator<TWebApi>
    {
        private readonly IMediator _mediator;

        public Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region SendFor

        public Task SendFor(Expression<Func<TWebApi, Task>> executeApiMethod) =>
            _mediator.Send(new ExecuteRequest<TWebApi>(executeApiMethod));

        public Task SendFor(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null) =>
            _mediator.Send(new ExecuteRequest<TWebApi>(executeApiMethod, context));

        public Task SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteRequest<TWebApi>(executeApiMethod), token);

        public Task SendFor(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null, CancellationToken token = default) =>
            _mediator.Send(new ExecuteRequest<TWebApi>(executeApiMethod, context), token);

        #endregion

        #region SendFor<TApiResponse>

        public Task<TApiResponse>
            SendFor<TApiResponse>(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TApiResponse>(executeApiMethod));

        public Task<TApiResponse> SendFor<TApiResponse>(
            Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TApiResponse>(executeApiMethod, context));

        public Task<TApiResponse> SendFor<TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TApiResponse>(executeApiMethod), token);

        public Task<TApiResponse> SendFor<TApiResponse>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TApiResponse>(executeApiMethod, context), token);

        #endregion

        #region SendFor<TModelResponse, TApiResponse>

        public Task<TModelResponse>
            SendFor<TModelResponse, TApiResponse>(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod));

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod, context));

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod), token);

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod, context), token);

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod));

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Context context = null) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod, context));

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod), token);

        public Task<TModelResponse> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod, context), token);

        #endregion
    }
}
