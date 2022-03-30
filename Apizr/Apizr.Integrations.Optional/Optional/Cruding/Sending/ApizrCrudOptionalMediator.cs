using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding.Sending
{
    public class ApizrCrudOptionalMediator : IApizrCrudOptionalMediator
    {
        private readonly IMediator _mediator;

        public ApizrCrudOptionalMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Create

        #region SendCreateOptionalCommand

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(TApiEntity entity) =>
            _mediator.Send(new CreateOptionalCommand<TApiEntity>(entity));

        public Task<Option<TApiEntity, ApizrException>>
            SendCreateOptionalCommand<TApiEntity>(TApiEntity entity, Context context) =>
            _mediator.Send(new CreateOptionalCommand<TApiEntity>(entity, context));

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(TApiEntity entity,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateOptionalCommand<TApiEntity>(entity), cancellationToken);

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand<TApiEntity>(TApiEntity entity, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateOptionalCommand<TApiEntity>(entity, context), cancellationToken);

        #endregion

        #region SendCreateOptionalCommand<TModelEntity>

        public Task<Option<TModelEntity, ApizrException>>
            SendCreateOptionalCommand<TModelEntity, TApiEntity>(TModelEntity entity) =>
            _mediator.Send(new CreateOptionalCommand<TModelEntity>(entity));

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(TModelEntity entity,
            Context context) => _mediator.Send(new CreateOptionalCommand<TModelEntity>(entity, context));

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(TModelEntity entity,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateOptionalCommand<TModelEntity>(entity), cancellationToken);

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity, TApiEntity>(TModelEntity entity,
            Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateOptionalCommand<TModelEntity>(entity, context), cancellationToken);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllOptionalQuery

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(
            bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(clearCache));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(Context context, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(context, clearCache));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(clearCache),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(priority, clearCache));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(
            int priority, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(priority, context, clearCache));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(priority, clearCache),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(
            Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(context, clearCache),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority,
            Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(priority, context, clearCache),
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>> SendReadAllOptionalQuery<
            TModelReadAllResult, TApiReadAllResult>(bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(clearCache));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(Context context, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(context, clearCache));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(CancellationToken cancellationToken,
                bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(clearCache),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(priority, clearCache));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, Context context, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(priority, context, clearCache));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(priority: priority, clearCache),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(context, clearCache),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(priority, context, clearCache),
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery(TReadAllParams)

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult,
            TReadAllParams>(TReadAllParams readAllParams, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, clearCache));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, context, clearCache));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, clearCache),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, clearCache));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority,
                Context context, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context, clearCache));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult,
            TReadAllParams>(TReadAllParams readAllParams,
            int priority, CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, clearCache),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, context, clearCache),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult,
            TReadAllParams>(TReadAllParams readAllParams, int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context, clearCache),
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams)

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, clearCache));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                TReadAllParams readAllParams, Context context, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, context, clearCache));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, clearCache),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(
                TReadAllParams readAllParams, int priority, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, clearCache));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority,
                Context context, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, context, clearCache));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>> SendReadAllOptionalQuery<
            TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, clearCache),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, context, clearCache),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>> SendReadAllOptionalQuery<
            TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority,
            Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, context, clearCache),
                cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadOptionalQuery

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, clearCache));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(
            TApiEntityKey key, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context, clearCache));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, clearCache), cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(
            TApiEntityKey key,
            int priority, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, clearCache));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(
            TApiEntityKey key,
            int priority, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context, clearCache));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, clearCache), cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context, clearCache), cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(
            TApiEntityKey key,
            int priority, Context context, CancellationToken cancellationToken, bool clearCache = false) => _mediator.Send(
            new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context, clearCache), cancellationToken);

        #endregion

        #region SendReadOptionalQuery<TModelEntity>

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, clearCache));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity,
            TApiEntityKey>(TApiEntityKey key, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context, clearCache));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key, CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, clearCache), cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity,
            TApiEntityKey>(TApiEntityKey key,
            int priority, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, clearCache));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity,
            TApiEntityKey>(TApiEntityKey key, int priority, Context context, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context, clearCache));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity,
            TApiEntityKey>(TApiEntityKey key,
            int priority, CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, clearCache), cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key, Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context, clearCache), cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context, clearCache),
                cancellationToken);

        #endregion

        #endregion

        #region Update

        #region SendUpdateOptionalCommand

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity entity) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, entity));

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity entity,
            Context context) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, entity, context));

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity entity,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, entity),
                cancellationToken);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, TApiEntity entity,
            Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, entity, context),
                cancellationToken);

        #endregion

        #region SendUpdateOptionalCommand<TModelEntity>

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, entity));

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, Context context) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, entity, context));

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, entity),
                cancellationToken);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key,
            TModelEntity entity, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, entity, context),
                cancellationToken);

        #endregion

        #endregion

        #region Delete

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key) =>
            _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key));

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context) =>
            _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key, context));

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key),
                cancellationToken);

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key, context),
                cancellationToken);

        #endregion
    }


    public class ApizrCrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : IApizrCrudOptionalMediator<
            TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> where TApiEntity : class
    {
        private readonly IApizrCrudOptionalMediator _apizrMediator;

        public ApizrCrudOptionalMediator(IApizrCrudOptionalMediator apizrMediator)
        {
            _apizrMediator = apizrMediator;
        }

        #region Create

        #region SendCreateOptionalCommand

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity entity) =>
            _apizrMediator.SendCreateOptionalCommand<TApiEntity>(entity);

        public Task<Option<TApiEntity, ApizrException>>
            SendCreateOptionalCommand(TApiEntity entity, Context context) =>
            _apizrMediator.SendCreateOptionalCommand<TApiEntity>(entity, context);

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity entity,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendCreateOptionalCommand<TApiEntity>(entity, cancellationToken);

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity entity, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendCreateOptionalCommand<TApiEntity>(entity, context, cancellationToken);

        #endregion

        #region SendCreateOptionalCommand<TModelEntity>

        public Task<Option<TModelEntity, ApizrException>>
            SendCreateOptionalCommand<TModelEntity>(TModelEntity entity) =>
            _apizrMediator.SendCreateOptionalCommand<TModelEntity>(entity);

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity entity,
            Context context) => _apizrMediator.SendCreateOptionalCommand<TModelEntity>(entity, context);

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity entity,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendCreateOptionalCommand<TModelEntity>(entity, cancellationToken);

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity entity,
            Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendCreateOptionalCommand<TModelEntity>(entity, context, cancellationToken);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllOptionalQuery

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(Context context,
            bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(context, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(cancellationToken, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority,
            bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(priority, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery(int priority, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(priority, context, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(priority, cancellationToken, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(context,
                cancellationToken, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority,
            Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(priority, context, cancellationToken, clearCache);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(context, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(cancellationToken, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(int priority, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(priority, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(int priority, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(priority, context, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(int priority, CancellationToken cancellationToken,
                bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(priority,
                cancellationToken, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(context,
                cancellationToken, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(int priority, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(priority, context,
                cancellationToken, clearCache);

        #endregion

        #region SendReadAllOptionalQuery(TReadAllParams)

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, context, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams,
                cancellationToken, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, int priority, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, context, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority,
                cancellationToken, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, context,
                cancellationToken, clearCache);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, context,
                cancellationToken, clearCache);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams)

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, Context context,
                bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, context, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams,
                cancellationToken, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, int priority,
                bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, int priority,
                Context context, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, context, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, int priority,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority,
                cancellationToken, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, context,
                cancellationToken, clearCache);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, int priority,
                Context context,
                CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, context,
                cancellationToken, clearCache);

        #endregion

        #endregion

        #region Read

        #region SendReadOptionalQuery

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, clearCache);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            Context context, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context, clearCache);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, cancellationToken, clearCache);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, clearCache);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context, clearCache);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, cancellationToken, clearCache);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context, cancellationToken, clearCache);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority, Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context, cancellationToken, clearCache);

        #endregion

        #region SendReadOptionalQuery<TModelEntity>

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, clearCache);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context, clearCache);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, cancellationToken, clearCache);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key,
            int priority, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, clearCache);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity>(TApiEntityKey key, int priority, Context context, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context, clearCache);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key,
            int priority, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, cancellationToken, clearCache);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, Context context, CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context, cancellationToken, clearCache);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken, bool clearCache = false) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context,
                cancellationToken, clearCache);

        #endregion

        #endregion

        #region Update

        #region SendUpdateOptionalCommand

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity entity) =>
            _apizrMediator.SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(key, entity);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity entity,
            Context context) =>
            _apizrMediator.SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(key, entity, context);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity entity,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(key, entity,
                cancellationToken);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity entity,
            Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendUpdateOptionalCommand<TApiEntity, TApiEntityKey>(key, entity, context,
                cancellationToken);

        #endregion

        #region SendUpdateOptionalCommand<TModelEntity>

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity) =>
            _apizrMediator.SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, Context context) =>
            _apizrMediator.SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity, context);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, CancellationToken cancellationToken) =>
            _apizrMediator.SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity,
                cancellationToken);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity entity, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendUpdateOptionalCommand<TModelEntity, TApiEntity, TApiEntityKey>(key, entity, context,
                cancellationToken);

        #endregion

        #endregion

        #region Delete

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key) =>
            _apizrMediator.SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(key);

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key, Context context) =>
            _apizrMediator.SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(key, context);

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(key,
                cancellationToken);

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendDeleteOptionalCommand<TApiEntity, TApiEntityKey>(key, context,
                cancellationToken);

        #endregion
    }
}