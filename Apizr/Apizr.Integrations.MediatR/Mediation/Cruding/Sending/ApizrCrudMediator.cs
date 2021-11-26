using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding.Sending
{
    public class ApizrCrudMediator : IApizrCrudMediator
    {
        private readonly IMediator _mediator;

        public ApizrCrudMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Create

        #region SendCreateCommand

        public Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity payload) =>
            _mediator.Send(new CreateCommand<TApiEntity>(payload));

        public Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity payload, Context context) =>
            _mediator.Send(new CreateCommand<TApiEntity>(payload, context));

        public Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateCommand<TApiEntity>(payload), cancellationToken);

        public Task<TApiEntity> SendCreateCommand<TApiEntity>(TApiEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateCommand<TApiEntity>(payload, context), cancellationToken);

        #endregion

        #region SendCreateCommand<TModelEntity>

        public Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity payload) =>
            _mediator.Send(new CreateCommand<TModelEntity>(payload));

        public Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity payload, Context context) =>
            _mediator.Send(new CreateCommand<TModelEntity>(payload, context));

        public Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateCommand<TModelEntity>(payload), cancellationToken);

        public Task<TModelEntity> SendCreateCommand<TModelEntity, TApiEntity>(TModelEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateCommand<TModelEntity>(payload, context), cancellationToken);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>() => _mediator.Send(new ReadAllQuery<TReadAllResult>());

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(context));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority, Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority, context));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(context), cancellationToken);

        public Task<TReadAllResult>
            SendReadAllQuery<TReadAllResult>(int priority, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority, context), cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult>

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>() =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>());

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(Context context) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(context));

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TReadAllResult>(CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(), cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(int priority) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(priority));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(priority), cancellationToken);

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TReadAllResult>(int priority, Context context) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(priority, context));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(context), cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(int priority,
            Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(priority, context), cancellationToken);

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, context));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority, Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority),
                cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, context), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority, Context context,
            CancellationToken cancellationToken) => _mediator.Send(
            new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context), cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams)

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams readAllParams) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams));

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, context));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams),
                cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority, Context context) =>
            _mediator.Send(
                new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, context));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority),
                cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, context),
                cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, context),
                cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key));

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, context));

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key), cancellationToken);

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority));

        public Task<TApiEntity>
            SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, context), cancellationToken);

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, int priority, Context context) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority, context));

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority), cancellationToken);

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority, context), cancellationToken);

        #endregion

        #region SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key));

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, context));

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key), cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority));

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, context), cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, int priority, Context context) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority, context));

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority), cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority, context), cancellationToken);

        #endregion

        #endregion

        #region Update

        #region SendUpdateCommand<TApiEntity, TApiEntityKey>

        public Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TApiEntity payload) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload));

        public Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity payload, Context context) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload, context));

        public Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TApiEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload), cancellationToken);

        public Task SendUpdateCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload, context), cancellationToken);

        #endregion

        #region SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>

        public Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity payload) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload));

        public Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, TModelEntity payload, Context context) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload, context));

        public Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload), cancellationToken);

        public Task SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, TModelEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload, context), cancellationToken);

        #endregion

        #endregion

        #region Delete

        public Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key) =>
            _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key));

        public Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context) =>
            _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key, context));

        public Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key), cancellationToken);

        public Task SendDeleteCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key), cancellationToken);

        #endregion
    }

    public class ApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : IApizrCrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> where TApiEntity : class
    {
        private readonly IApizrCrudMediator _apizrMediator;

        public ApizrCrudMediator(IApizrCrudMediator apizrMediator)
        {
            _apizrMediator = apizrMediator;
        }

        #region Create

        #region SendCreateCommand

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload) =>
            _apizrMediator.SendCreateCommand<TApiEntity>(payload);

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload, Context context) =>
            _apizrMediator.SendCreateCommand<TApiEntity>(payload, context);

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendCreateCommand<TApiEntity>(payload, cancellationToken);

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendCreateCommand<TApiEntity>(payload, context, cancellationToken);

        #endregion

        #region SendCreateCommand<TModelEntity>

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload) =>
            _apizrMediator.SendCreateCommand<TModelEntity>(payload);

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, Context context) =>
            _apizrMediator.SendCreateCommand<TModelEntity>(payload, context);

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendCreateCommand<TModelEntity>(payload, cancellationToken);

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendCreateCommand<TModelEntity>(payload, context, cancellationToken);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllQuery

        public Task<TReadAllResult> SendReadAllQuery() => _apizrMediator.SendReadAllQuery<TReadAllResult>();

        public Task<TReadAllResult> SendReadAllQuery(Context context) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(context);

        public Task<TReadAllResult> SendReadAllQuery(CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(int priority) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(priority);

        public Task<TReadAllResult> SendReadAllQuery(int priority, Context context) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(priority, context);

        public Task<TReadAllResult> SendReadAllQuery(int priority,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(priority, cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(context, cancellationToken);

        public Task<TReadAllResult>
            SendReadAllQuery(int priority, Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(priority, context, cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult>

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>() =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>();

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(Context context) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(context);

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult>(CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(int priority) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(priority);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(int priority,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(priority, cancellationToken);

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult>(int priority, Context context) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(priority, context);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(context, cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(int priority,
            Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(priority, context, cancellationToken);

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, context);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, priority);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, int priority, Context context) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, context);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, priority,
                cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, context, cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, int priority, Context context,
            CancellationToken cancellationToken) => 
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, context, cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult>(TReadAllParams)

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams);

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams, Context context) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, context);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams,
                cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            int priority) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, context);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority,
                cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, context,
                cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, context,
                cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, Context context) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, context);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, cancellationToken);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, priority);

        public Task<TApiEntity>
            SendReadQuery(TApiEntityKey key, Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, context, cancellationToken);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, int priority, Context context) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, priority, context);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, priority, cancellationToken);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, priority, context, cancellationToken);

        #endregion

        #region SendReadQuery<TModelEntity>

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, context);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, context, cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, int priority, Context context) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority, context);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority, cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority, context, cancellationToken);

        #endregion

        #endregion

        #region Update

        #region SendUpdateCommand

        public Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity payload) =>
            _apizrMediator.SendUpdateCommand<TApiEntity, TApiEntityKey>(key, payload);

        public Task SendUpdateCommand(TApiEntityKey key, TApiEntity payload, Context context) =>
            _apizrMediator.SendUpdateCommand<TApiEntity, TApiEntityKey>(key, payload, context);

        public Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity payload,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendUpdateCommand<TApiEntity, TApiEntityKey>(key, payload, cancellationToken);

        public Task SendUpdateCommand(TApiEntityKey key, TApiEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendUpdateCommand<TApiEntity, TApiEntityKey>(key, payload, context, cancellationToken);

        #endregion

        #region SendUpdateCommand<TModelEntity>

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload) =>
            _apizrMediator.SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, payload);

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key, TModelEntity payload, Context context) =>
            _apizrMediator.SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, payload, context);

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, payload, cancellationToken);

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key, TModelEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendUpdateCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, payload, context, cancellationToken);

        #endregion

        #endregion

        #region Delete

        public Task SendDeleteCommand(TApiEntityKey key) =>
            _apizrMediator.SendDeleteCommand<TApiEntity, TApiEntityKey>(key);

        public Task SendDeleteCommand(TApiEntityKey key, Context context) =>
            _apizrMediator.SendDeleteCommand<TApiEntity, TApiEntityKey>(key, context);

        public Task SendDeleteCommand(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendDeleteCommand<TApiEntity, TApiEntityKey>(key, cancellationToken);

        public Task SendDeleteCommand(TApiEntityKey key, Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendDeleteCommand<TApiEntity, TApiEntityKey>(key, cancellationToken);

        #endregion
    }
}