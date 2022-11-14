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
        
        /// <inheritdoc />
        public Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity entity,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new CreateCommand<TApiEntity>(entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity entity,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new CreateCommand<TModelEntity>(entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region ReadAll
        
        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region Read
        
        /// <inheritdoc />
        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region Update
        
        /// <inheritdoc />
        public Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TApiEntity entity, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
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
        
        /// <inheritdoc />
        public Task<TApiEntity> SendCreateCommand(TApiEntity entity,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendCreateCommand<TApiEntity>(entity, optionsBuilder);
        
        /// <inheritdoc />
        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity entity,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendCreateCommand<TModelEntity>(entity, optionsBuilder);
        
        #endregion

        #region ReadAll
        
        /// <inheritdoc />
        public Task<TReadAllResult>
            SendReadAllQuery(Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) => _apizrMediator.SendReadAllQuery<TReadAllResult>(optionsBuilder);
        
        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(optionsBuilder);
        
        /// <inheritdoc />
        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, optionsBuilder);
        
        /// <inheritdoc />
        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, optionsBuilder);
        
        #endregion

        #region Read
        
        /// <inheritdoc />
        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, optionsBuilder);
        
        /// <inheritdoc />
        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, optionsBuilder);
        
        #endregion

        #region Update
        
        /// <inheritdoc />
        public Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity entity, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendUpdateCommand<TApiEntity, TApiEntityKey>(key, entity, optionsBuilder);
        
        /// <inheritdoc />
        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity, optionsBuilder);
        
        #endregion

        #region Delete

        /// <inheritdoc />
        public Task SendDeleteCommand(TApiEntityKey key,
            Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendDeleteCommand<TApiEntity, TApiEntityKey>(key, optionsBuilder);
        
        #endregion
    }
}