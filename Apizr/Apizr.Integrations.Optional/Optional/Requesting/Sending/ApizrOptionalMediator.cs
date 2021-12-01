using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Sending
{
    public class ApizrOptionalMediator : IApizrOptionalMediator
    {
        private readonly IMediator _mediator;

        public ApizrOptionalMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Unit

        #region SendFor<TWebApi>

        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<TWebApi, Task>> executeApiMethod) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod));

        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod, context));

        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod), token);

        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context = null, CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod, context), token);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData));

        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData), token);

        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context = null) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context));

        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context), token);

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod));

        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod, context));

        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod), token);

        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod, context), token);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod));

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, context));

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod),
                token);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, context),
                token);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData));

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context));

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken token = default) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData),
                token);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context),
                token);

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData));

        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken token = default) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData),
                token);

        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData, context));

        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, CancellationToken token = default) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData, context),
                token);

        #endregion 

        #endregion
    }

    public class ApizrOptionalMediator<TWebApi> : IApizrOptionalMediator<TWebApi>
    {
        private readonly IApizrOptionalMediator _apizrMediator;

        public ApizrOptionalMediator(IApizrOptionalMediator apizrMediator)
        {
            _apizrMediator = apizrMediator;
        }

        #region Unit

        #region SendFor

        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod);

        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, context);

        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, token);

        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null, CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, context, token);

        #endregion

        #region SendFor<TModelData, TApiData>

        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData);

        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, token);

        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context);

        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, token);

        #endregion

        #endregion

        #region Result

        #region SendFor<TApiData>

        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod);

        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, context);

        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, token);

        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, context, token);

        #endregion

        #region SendFor<TModelData, TApiData>

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, context);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, token);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, context, token);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, token);

        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, token);

        #endregion

        #region SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData);

        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, token);

        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, context);

        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, context, token);

        #endregion

        #endregion
    }
}