using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
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
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi>(executeApiMethod, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task SendFor<TWebApi>(Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi>(executeApiMethod, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteUnitRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <inheritdoc />
        public Task<TApiData> SendFor<TWebApi, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TApiData>(executeApiMethod, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TApiData> SendFor<TWebApi, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TApiData>(executeApiMethod, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder),
                CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                    executeApiMethod, modelRequestData, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _mediator.Send(
                new ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                    executeApiMethod, modelRequestData, optionsBuilder), CreateRequestOptionsBuilder<TWebApi>(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #endregion
        
        private static IApizrRequestOptionsBuilder
            CreateRequestOptionsBuilder<TWebApi>(Action<IApizrRequestOptionsBuilder> optionsBuilder) =>
            ApizrManager<TWebApi>.CreateRequestOptionsBuilder(optionsBuilder);
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
        public Task SendFor(Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task SendFor(Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi>(executeApiMethod, optionsBuilder);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        #endregion

        #endregion

        #region Result

        #region SendFor<TApiData>

        /// <inheritdoc />
        public Task<TApiData> SendFor<TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<TApiData> SendFor<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TApiData>(executeApiMethod, optionsBuilder);

        #endregion

        #region SendFor<TModelData, TApiData>

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, optionsBuilder);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        /// <inheritdoc />
        public Task<TModelData> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelData, TApiData>(executeApiMethod, modelData, optionsBuilder);

        #endregion

        #region SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        /// <inheritdoc />
        public Task<TModelResultData> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => _apizrMediator.SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                executeApiMethod, modelRequestData, optionsBuilder);

        #endregion

        #endregion
    }
}
