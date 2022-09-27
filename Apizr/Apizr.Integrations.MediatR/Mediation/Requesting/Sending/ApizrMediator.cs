using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Sending
{
    /// <summary>
    /// Apizr mediator to send request using MediatR by calling expression
    /// </summary>
    public class ApizrMediator : IApizrMediator
    {
        private readonly IMediator _mediator;

        public ApizrMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Unit

        #region SendFor<TWebApi>

        /// <inheritdoc />
        public Task SendFor<TWebApi>(Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<Exception> onException = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi>(executeApiMethod, onException));

        /// <inheritdoc />
        public Task SendFor<TWebApi>(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null,
            Action<Exception> onException = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi>(executeApiMethod, context, onException));

        /// <inheritdoc />
        public Task SendFor<TWebApi>(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi>(executeApiMethod, onException), token);

        /// <inheritdoc />
        public Task SendFor<TWebApi>(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null, CancellationToken token = default, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi>(executeApiMethod, context, onException), token);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, onException));

        /// <inheritdoc />
        public Task SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, onException),
                token);

        /// <inheritdoc />
        public Task SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            Context context = null, Action<Exception> onException = null)
            => _mediator.Send(
                new ExecuteUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, onException));

        /// <inheritdoc />
        public Task SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default, Action<Exception> onException = null)
            => _mediator.Send(
                new ExecuteUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, onException), token);

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <inheritdoc />
        public Task<TApiData> SendFor<TWebApi, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TApiData>(executeApiMethod, clearCache, onException));

        /// <inheritdoc />
        public Task<TApiData> SendFor<TWebApi, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TApiData>(executeApiMethod, context, clearCache, onException));

        /// <inheritdoc />
        public Task<TApiData> SendFor<TWebApi, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default,
            bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TApiData>(executeApiMethod, clearCache, onException), token);

        /// <inheritdoc />
        public Task<TApiData> SendFor<TWebApi, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, CancellationToken token = default,
            bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TApiData>(executeApiMethod, context, clearCache, onException), token);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, clearCache));

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, context, clearCache, onException));

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, clearCache, onException), token);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, context, clearCache, onException),
                token);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, clearCache, onException));

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, clearCache, onException));

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, clearCache, onException),
                token);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, clearCache, onException), token);

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                    executeApiMethod, modelRequestData, clearCache, onException));

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken token = default, bool clearCache = false,
            Action<Exception> onException = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                    executeApiMethod, modelRequestData, clearCache, onException), token);

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false,
            Action<Exception> onException = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                    executeApiMethod, modelRequestData, context, clearCache, onException));

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                    executeApiMethod, modelRequestData, context, clearCache, onException), token);  

        #endregion

        #endregion
    }

    /// <summary>
    /// Apizr mediator to send request using MediatR by calling expression
    /// </summary>
    public class ApizrMediator<TWebApi> : IApizrMediator<TWebApi>
    {
        private readonly IApizrMediator _apizrMediator;

        public ApizrMediator(IApizrMediator apizrMediator)
        {
            _apizrMediator = apizrMediator;
        }

        #region Unit

        #region SendFor

        /// <inheritdoc />
        public Task SendFor(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, onException);

        /// <inheritdoc />
        public Task SendFor(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context = null,
            Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, context, onException);

        /// <inheritdoc />
        public Task SendFor(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken token = default, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, token, onException);

        /// <inheritdoc />
        public Task SendFor(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null, CancellationToken token = default, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, context, token, onException);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, onException);

        /// <inheritdoc />
        public Task SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken token = default, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, token, onException);

        /// <inheritdoc />
        public Task SendFor<TModelData, TApiData>(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, onException);

        /// <inheritdoc />
        public Task SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, token, onException);

        #endregion

        #endregion

        #region Result

        #region SendFor<TApiData>

        /// <inheritdoc />
        public Task<TApiData> SendFor<TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, clearCache, onException);

        /// <inheritdoc />
        public Task<TApiData> SendFor<TApiData>(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, context, clearCache, onException);

        /// <inheritdoc />
        public Task<TApiData> SendFor<TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, token, clearCache, onException);

        /// <inheritdoc />
        public Task<TApiData> SendFor<TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null, CancellationToken token = default,
            bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, context, token, clearCache, onException);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false,
            Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null,
            bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, context, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, token, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, context, token, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null, bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, token, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, context, token, clearCache, onException);

        #endregion

        #region SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken token = default, bool clearCache = false,
            Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, token, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, bool clearCache = false,
            Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, context, clearCache, onException);

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context = null,
            CancellationToken token = default, bool clearCache = false, Action<Exception> onException = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, context, token, clearCache, onException);  

        #endregion

        #endregion
    }
}
