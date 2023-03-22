using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Sending;
using MediatR;
using Optional;

namespace Apizr.Optional.Requesting.Sending
{
    /// <summary>
    /// Apizr mediator to send request using MediatR by calling expression and returning optional result
    /// </summary>
    public class ApizrOptionalMediator : ApizrMediatorBase, IApizrOptionalMediator, IApizrInternalMediator
    {
        private readonly IMediator _mediator;

        public ApizrOptionalMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Unit

        #region SendFor<TWebApi>

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ExecuteOptionalUnitRequest<TWebApi>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData,
                    optionsBuilder), CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData,
                    optionsBuilder), CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ExecuteOptionalResultRequest<TWebApi, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData,
                    optionsBuilder), CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData,
                    optionsBuilder), CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
                    TModelRequestData>(executeApiMethod, modelRequestData, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #endregion

        #region Internal

        /// <inheritdoc />
        Task<TResponse> IApizrInternalMediator.Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            => _mediator.Send(request, cancellationToken); 

        #endregion
    }

    /// <summary>
    /// <see cref="IApizrOptionalMediator"/> but dedicated to <typeparamref name="TWebApi"/> with optional result, getting all shorter
    /// </summary>
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
        public Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor(
            Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, optionsBuilder);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);
        
        #endregion

        #endregion

        #region Result

        #region SendFor<TApiData>

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, optionsBuilder);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        #endregion

        #region SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        /// <inheritdoc />
        public Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData,
            TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        #endregion

        #endregion
    }
}