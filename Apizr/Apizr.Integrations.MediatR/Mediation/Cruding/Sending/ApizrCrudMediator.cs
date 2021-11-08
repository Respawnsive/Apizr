using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding.Sending
{
    public class ApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> where TApiEntity : class
    {
        private readonly IMediator _mediator;

        public ApizrCrudMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Create

        #region SendCreateCommand

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload) =>
            _mediator.Send(new CreateCommand<TApiEntity>(payload));

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload, Context context) =>
            _mediator.Send(new CreateCommand<TApiEntity>(payload, context));

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateCommand<TApiEntity>(payload), cancellationToken);

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateCommand<TApiEntity>(payload, context), cancellationToken);

        #endregion

        #region SendCreateCommand<TModelEntity>

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload) =>
            _mediator.Send(new CreateCommand<TModelEntity>(payload));

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, Context context) =>
            _mediator.Send(new CreateCommand<TModelEntity>(payload, context));

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateCommand<TModelEntity>(payload), cancellationToken);

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateCommand<TModelEntity>(payload, context), cancellationToken);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        public Task<TReadAllResult> SendReadAllQuery() => _mediator.Send(new ReadAllQuery<TReadAllResult>());

        public Task<TReadAllResult> SendReadAllQuery(Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(context));

        public Task<TReadAllResult> SendReadAllQuery(CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(int priority) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority));

        public Task<TReadAllResult> SendReadAllQuery(int priority, Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority, context));

        public Task<TReadAllResult> SendReadAllQuery(int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(context), cancellationToken);

        public Task<TReadAllResult>
            SendReadAllQuery(int priority, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority, context), cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelEntityReadAllResult>

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>() =>
            _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>());

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(Context context) =>
            _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(context));

        public Task<TModelEntityReadAllResult>
            SendReadAllQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(), cancellationToken);

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority) =>
            _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(priority));

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(priority), cancellationToken);

        public Task<TModelEntityReadAllResult>
            SendReadAllQuery<TModelEntityReadAllResult>(int priority, Context context) =>
            _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(priority, context));

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(context), cancellationToken);

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority,
            Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(priority, context), cancellationToken);

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams));

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, context));

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority));

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, int priority, Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context));

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority),
                cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, context), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, int priority, Context context,
            CancellationToken cancellationToken) => _mediator.Send(
            new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context), cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams)

        public Task<TModelEntityReadAllResult>
            SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams));

        public Task<TModelEntityReadAllResult>
            SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, context));

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams),
                cancellationToken);

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority));

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context) =>
            _mediator.Send(
                new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority, context));

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority),
                cancellationToken);

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, context),
                cancellationToken);

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority, context),
                cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key));

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, Context context) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, context));

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key), cancellationToken);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority));

        public Task<TApiEntity>
            SendReadQuery(TApiEntityKey key, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, context), cancellationToken);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, int priority, Context context) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority, context));

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority), cancellationToken);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority, context), cancellationToken);

        #endregion

        #region SendReadQuery<TModelEntity>

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key));

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, context));

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key), cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority));

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, context), cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, int priority, Context context) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority, context));

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority), cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority, context), cancellationToken);

        #endregion

        #endregion

        #region Update

        #region SendUpdateCommand

        public Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity payload) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload));

        public Task SendUpdateCommand(TApiEntityKey key, TApiEntity payload, Context context) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload, context));

        public Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload), cancellationToken);

        public Task SendUpdateCommand(TApiEntityKey key, TApiEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload, context), cancellationToken);

        #endregion

        #region SendUpdateCommand<TModelEntity>

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload));

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key, TModelEntity payload, Context context) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload, context));

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload), cancellationToken);

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key, TModelEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload, context), cancellationToken);

        #endregion

        #endregion

        #region Delete

        public Task SendDeleteCommand(TApiEntityKey key) =>
            _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key));

        public Task SendDeleteCommand(TApiEntityKey key, Context context) =>
            _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key, context));

        public Task SendDeleteCommand(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key), cancellationToken);

        public Task SendDeleteCommand(TApiEntityKey key, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key), cancellationToken);

        #endregion
    }
}