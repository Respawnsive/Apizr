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

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(bool clearCache = false) => _mediator.Send(new ReadAllQuery<TReadAllResult>(clearCache));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(Context context, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(context, clearCache));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(CancellationToken cancellationToken,
            bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(clearCache), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority, clearCache));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority, Context context,
            bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority, context, clearCache));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority, clearCache), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(context, clearCache), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult>(int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllResult>(priority, context, clearCache), cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult>

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(clearCache));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(Context context,
            bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(context, clearCache));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(clearCache), cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(int priority,
            bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(priority, clearCache));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(priority, clearCache), cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(int priority,
            Context context, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(priority, context, clearCache));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(context, clearCache), cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult>(int priority,
            Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TModelReadAllResult>(priority, context, clearCache), cancellationToken);

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, clearCache));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            Context context, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, context, clearCache));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, clearCache), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, clearCache));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context, clearCache));

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, clearCache),
                cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, context, clearCache), cancellationToken);

        public Task<TReadAllResult> SendReadAllQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) => _mediator.Send(
            new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context, clearCache), cancellationToken);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams)

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, clearCache));

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, context, clearCache));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, clearCache),
                cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, clearCache));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority, Context context, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, context, clearCache));

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, clearCache),
                cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, context, clearCache),
                cancellationToken);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, context, clearCache),
                cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, clearCache));

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, context, clearCache));

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, clearCache), cancellationToken);

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority, clearCache));

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, context, clearCache), cancellationToken);

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, int priority,
            Context context, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority, context, clearCache));

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority, clearCache), cancellationToken);

        public Task<TApiEntity> SendReadQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, int priority,
            Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority, context, clearCache), cancellationToken);

        #endregion

        #region SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, clearCache));

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, context, clearCache));

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, clearCache), cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority, clearCache));

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, context, clearCache), cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority, context, clearCache));

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority, clearCache), cancellationToken);

        public Task<TModelEntity> SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority, context, clearCache), cancellationToken);

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

        public Task<TReadAllResult> SendReadAllQuery(bool clearCache = false) => _apizrMediator.SendReadAllQuery<TReadAllResult>(clearCache);

        public Task<TReadAllResult> SendReadAllQuery(Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(context, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(cancellationToken, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(int priority, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(priority, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(int priority, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(priority, context, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(priority, cancellationToken, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(Context context, CancellationToken cancellationToken,
            bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(context, cancellationToken, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(int priority, Context context, CancellationToken cancellationToken,
            bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult>(priority, context, cancellationToken, clearCache);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult>

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(context, clearCache);

        public Task<TModelReadAllResult>
            SendReadAllQuery<TModelReadAllResult>(CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(cancellationToken, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(int priority, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(priority, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(priority, cancellationToken, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(int priority, Context context,
            bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(priority, context, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(context, cancellationToken, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(int priority,
            Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult>(priority, context, cancellationToken, clearCache);

        #endregion

        #region SendReadAllQuery(TReadAllParams)

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context,
            bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, context, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, cancellationToken, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, int priority, Context context,
            bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, context, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, priority,
                cancellationToken, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, context, cancellationToken, clearCache);

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams, int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) => 
            _apizrMediator.SendReadAllQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, context, cancellationToken, clearCache);

        #endregion

        #region SendReadAllQuery<TModelReadAllResult>(TReadAllParams)

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, context, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams,
                cancellationToken, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            int priority, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, context, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority,
                cancellationToken, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, context,
                cancellationToken, clearCache);

        public Task<TModelReadAllResult> SendReadAllQuery<TModelReadAllResult>(TReadAllParams readAllParams,
            int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, context,
                cancellationToken, clearCache);

        #endregion

        #endregion

        #region Read

        #region SendReadQuery

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, clearCache);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, context, clearCache);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, cancellationToken, clearCache);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, priority, clearCache);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, Context context, CancellationToken cancellationToken,
            bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, context, cancellationToken, clearCache);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, int priority, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, priority, context, clearCache);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, priority, cancellationToken, clearCache);

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TApiEntity, TApiEntityKey>(key, priority, context, cancellationToken, clearCache);

        #endregion

        #region SendReadQuery<TModelEntity>

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, clearCache);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context,
            bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, context, clearCache);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, cancellationToken, clearCache);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority, clearCache);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, context, cancellationToken, clearCache);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, int priority, Context context,
            bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority, context, clearCache);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority, cancellationToken, clearCache);

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadQuery<TModelEntity, TApiEntity, TApiEntityKey>(key, priority, context, cancellationToken, clearCache);

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