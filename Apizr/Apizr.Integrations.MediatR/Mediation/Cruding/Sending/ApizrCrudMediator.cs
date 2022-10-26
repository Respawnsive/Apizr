using System;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Sending;
using MediatR;

namespace Apizr.Mediation.Cruding.Sending
{
    /// <summary>
    /// Apizr mediator dedicated to cruding
    /// </summary>
    public class ApizrCrudMediator : ApizrMediatorBase, IApizrCrudMediator
    {
        private readonly IMediator _mediator;

        public ApizrCrudMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Create

        #region SendCreateCommand

        /// <inheritdoc />
        public Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity entity,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new CreateCommand<TApiEntity>(entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region SendCreateCommand<TModelEntity>

        /// <inheritdoc />
        public Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity entity,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new CreateCommand<TModelEntity>(entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region SendReadAllQuery<TModelReadAllResult>

        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(int priority,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(priority, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region SendReadAllQuery(TReadAllParams)

        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams)

        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        /// <inheritdoc />
        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>

        /// <inheritdoc />
        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <inheritdoc />
        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #endregion

        #region Update

        #region SendUpdateCommand<TApiEntity, TApiEntityKey>

        /// <inheritdoc />
        public Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TApiEntity entity, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>

        /// <inheritdoc />
        public Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #endregion

        #region Delete

        /// <inheritdoc />
        public Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion
    }

    /// <summary>
    /// Apizr mediator dedicated to cruding
    /// </summary>
    public class ApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> where TApiEntity : class
    {
        private readonly IApizrCrudMediator _apizrMediator;

        public ApizrCrudMediator(IApizrCrudMediator apizrMediator)
        {
            _apizrMediator = apizrMediator;
        }

        #region Create

        #region SendCreateCommand

        /// <inheritdoc />
        public Task<TApiEntity> SendCreateCommand(TApiEntity entity,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendCreateCommand<TApiEntity>(entity, optionsBuilder);
        
        #endregion

        #region SendCreateCommand<TModelEntity>

        /// <inheritdoc />
        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity entity,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendCreateCommand<TModelEntity>(entity, optionsBuilder);
        
        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        /// <inheritdoc />
        public Task<TReadAllResult>
            SendReadAllQuery(Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) => _apizrMediator.SendReadAllQuery<TReadAllResult>(optionsBuilder);
        
        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery(int priority,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(priority, optionsBuilder);
        
        #endregion

        #region SendReadAllQuery<TModelReadAllResult>

        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(optionsBuilder);
        
        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(int priority,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(priority, optionsBuilder);
        
        #endregion

        #region SendReadAllQuery(TReadAllParams)

        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, optionsBuilder);
        
        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, optionsBuilder);
        
        #endregion

        #region SendReadAllQuery<TModelReadAllResult>(TReadAllParams)

        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, optionsBuilder);
        
        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            int priority, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, optionsBuilder);
        
        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        /// <inheritdoc />
        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, optionsBuilder);
        
        /// <inheritdoc />
        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, priority, optionsBuilder);
        
        #endregion

        #region SendReadQuery<TModelEntity>

        /// <inheritdoc />
        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, optionsBuilder);
        
        /// <inheritdoc />
        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority, optionsBuilder);
        
        #endregion

        #endregion

        #region Update

        #region SendUpdateCommand

        /// <inheritdoc />
        public Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity entity, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendUpdateCommand<TApiEntity, TApiEntityKey>(key, entity, optionsBuilder);
        
        #endregion

        #region SendUpdateCommand<TModelEntity>

        /// <inheritdoc />
        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity, optionsBuilder);
        
        #endregion

        #endregion

        #region Delete

        /// <inheritdoc />
        public Task SendDeleteCommand(TApiEntityKey key,
            Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendDeleteCommand<TApiEntity, TApiEntityKey>(key, optionsBuilder);
        
        #endregion
    }
}