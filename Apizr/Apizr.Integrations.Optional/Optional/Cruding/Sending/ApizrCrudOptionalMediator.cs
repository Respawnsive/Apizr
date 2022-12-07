using System;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Sending;
using MediatR;
using Optional;

namespace Apizr.Optional.Cruding.Sending
{
    /// <summary>
    /// Apizr mediator dedicated to cruding and with optional result
    /// </summary>
    public class ApizrCrudOptionalMediator : ApizrMediatorBase, IApizrCrudOptionalMediator
    {
        private readonly IMediator _mediator;

        public ApizrCrudOptionalMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Create
        
        /// <inheritdoc />
        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(TApiEntity entity,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new CreateOptionalCommand<TApiEntity>(entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(
            TModelEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new CreateOptionalCommand<TModelEntity>(entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region ReadAll

        /// <inheritdoc />
        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>> SendReadAllOptionalQuery<
            TModelReadAllResult, TApiReadAllResult>(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult,
            TReadAllParams>(TReadAllParams readAllParams,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                TReadAllParams readAllParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region Read
        
        /// <inheritdoc />
        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(
            TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity,
            TApiEntityKey>(TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region Update
        
        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(
            TApiEntityKey key, TApiEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key,
            TModelEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, entity, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);
        
        #endregion

        #region Delete

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(
            TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key, optionsBuilder),
                CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion
    }

    /// <summary>
    /// Apizr mediator dedicated to cruding and with optional result
    /// </summary>
    public class ApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : IApizrCrudOptionalMediator<
            TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> where TApiEntity : class
    {
        private readonly IApizrCrudOptionalMediator _apizrMediator;

        public ApizrCrudOptionalMediator(IApizrCrudOptionalMediator apizrMediator)
        {
            _apizrMediator = apizrMediator;
        }

        #region Create
        
        /// <inheritdoc />
        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity entity,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendCreateOptionalCommand<TApiEntity>(entity, optionsBuilder);
        
        /// <inheritdoc />
        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity entity,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendCreateOptionalCommand<TModelEntity>(entity, optionsBuilder);
        
        #endregion

        #region ReadAll
        
        /// <inheritdoc />
        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(optionsBuilder);
        
        /// <inheritdoc />
        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(
                Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(optionsBuilder);
        
        /// <inheritdoc />
        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, optionsBuilder);
        
        /// <inheritdoc />
        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams,
                Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, optionsBuilder);
        
        #endregion

        #region Read
        
        /// <inheritdoc />
        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, optionsBuilder);
        
        /// <inheritdoc />
        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, optionsBuilder);
        
        #endregion

        #region Update
        
        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity entity,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(key, entity, optionsBuilder);
        
        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity, optionsBuilder);
        
        #endregion

        #region Delete

        /// <inheritdoc />
        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            _apizrMediator.SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(key, optionsBuilder);
        
        #endregion
    }
}