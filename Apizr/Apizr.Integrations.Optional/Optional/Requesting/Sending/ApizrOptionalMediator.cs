﻿using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Sending
{
    public class ApizrOptionalMediator<TWebApi> : IApizrOptionalMediator<TWebApi>
    {
        private readonly IMediator _mediator;

        public ApizrOptionalMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region SendFor

        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod));

        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<Context, TWebApi, Task>> executeApiMethod,
            Context context = null) => _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod, context));

        public Task<Option<Unit, ApizrException>> SendFor(
            Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod), token);

        public Task<Option<Unit, ApizrException>> SendFor(
            Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod, context), token);

        #endregion

        #region SendFor<TApiResponse>

        public Task<Option<TApiResponse, ApizrException<TApiResponse>>>
            SendFor<TApiResponse>(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) =>
            _mediator.Send(new ExecuteOptionalRequest<TWebApi, TApiResponse>(executeApiMethod));

        public Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null) =>
            _mediator.Send(new ExecuteOptionalRequest<TWebApi, TApiResponse>(executeApiMethod, context));

        public Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalRequest<TWebApi, TApiResponse>(executeApiMethod), token);

        public Task<Option<TApiResponse, ApizrException<TApiResponse>>> SendFor<TApiResponse>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalRequest<TWebApi, TApiResponse>(executeApiMethod, context), token);

        #endregion

        #region SendFor<TModelResponse, TApiResponse>

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>>
            SendFor<TModelResponse, TApiResponse>(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod));

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod, context));

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod), token);

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod));

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod, context),
                token);

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Context context = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod, context));

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod), token);

        public Task<Option<TModelResponse, ApizrException<TModelResponse>>> SendFor<TModelResponse, TApiResponse>(
            Expression<Func<Context, CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse>(executeApiMethod, context),
                token);

        #endregion
    }
}