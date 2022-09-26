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
    /// <inheritdoc />
    public class ApizrOptionalMediator : IApizrOptionalMediator
    {
        private readonly IMediator _mediator;

        public ApizrOptionalMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Unit

        #region SendFor<TWebApi>

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<TWebApi, Task>> executeApiMethod) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod));

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod, context));

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod), token);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context = null, CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod, context), token);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData));

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData), token);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context = null) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context));

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context = null,
            CancellationToken token = default) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context), token);

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod, clearCache));

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod, context, clearCache));

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod, clearCache), token);

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, CancellationToken token = default, bool clearCache = false) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod, context, clearCache), token);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, clearCache));

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, context, clearCache));

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, clearCache),
                token);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, context, clearCache),
                token);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            bool clearCache = false) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, clearCache));

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, clearCache));

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken token = default, bool clearCache = false) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, clearCache),
                token);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default, bool clearCache = false) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, clearCache),
                token);

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData, clearCache));

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken token = default, bool clearCache = false) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData, clearCache),
                token);

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData, context, clearCache));

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context = null,
            CancellationToken token = default, bool clearCache = false) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData, context, clearCache),
                token);

        #endregion 

        #endregion
    }

    /// <inheritdoc />
    public class ApizrOptionalMediator<TWebApi> : IApizrOptionalMediator<TWebApi>
    {
        private readonly IApizrOptionalMediator _apizrMediator;

        public ApizrOptionalMediator(IApizrOptionalMediator apizrMediator)
        {
            _apizrMediator = apizrMediator;
        }

        #region Unit

        #region SendFor

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, context);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, token);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null, CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, context, token);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, token);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, token);

        #endregion

        #endregion

        #region Result

        #region SendFor<TApiData>

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, clearCache);

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, context, clearCache);

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, token, clearCache);

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, CancellationToken token = default, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, context, token, clearCache);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, context, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, token, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, context, token, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken token = default, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, token, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, token, clearCache);

        #endregion

        #region SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken token = default, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, token, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, context, clearCache);

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context = null,
            CancellationToken token = default, bool clearCache = false)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, context, token, clearCache);

        #endregion

        #endregion
    }
}