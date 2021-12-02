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

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>() =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>());

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(Context context) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(context));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(priority));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult>(int priority, Context context) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(priority, context));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(priority),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(context),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult>(int priority,
            Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllResult>(priority, context),
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>() =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>());

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(Context context) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(context));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(priority));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, Context context) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(priority, context));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(priority: priority),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(Context context, CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(context),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult>(int priority, Context context,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TModelReadAllResult>(priority, context),
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery(TReadAllParams)

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, context));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority, Context context) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams,
            int priority, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, context),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(
            TReadAllParams readAllParams, int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context),
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams)

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, context));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority,
                Context context) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, context));

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, context),
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult, TApiReadAllResult, TReadAllParams>(TReadAllParams readAllParams, int priority,
                Context context,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelReadAllResult>(readAllParams, priority, context),
                cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadOptionalQuery

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key), cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Context context) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority), cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context), cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery<TApiEntity, TApiEntityKey>(TApiEntityKey key,
            int priority, Context context, CancellationToken cancellationToken) => _mediator.Send(
            new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context), cancellationToken);

        #endregion

        #region SendReadOptionalQuery<TModelEntity>

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, Context context) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key), cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key,
            int priority) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(TApiEntityKey key, int priority, Context context) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key,
            int priority, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority), cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context), cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity, TApiEntity, TApiEntityKey>(
            TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context),
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

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery() =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>();

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(Context context) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(context);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(priority);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery(int priority, Context context) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(priority, context);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(priority, cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(context,
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority,
            Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult>(priority, context, cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>() =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>();

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(Context context) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(context);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(int priority) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(priority);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(int priority, Context context) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(priority, context);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(int priority, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(priority,
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(context,
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(int priority, Context context,
                CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult>(priority, context,
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery(TReadAllParams)

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery(TReadAllParams readAllParams, Context context) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, context);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams,
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery(TReadAllParams readAllParams, int priority, Context context) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, context);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority,
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, context,
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, int priority, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TReadAllResult, TReadAllParams>(readAllParams, priority, context,
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams)

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, Context context) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, context);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams,
                CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams,
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, int priority) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, int priority,
                Context context) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, context);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, int priority,
                CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority,
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, context,
                cancellationToken);

        public Task<Option<TModelReadAllResult, ApizrException<TModelReadAllResult>>>
            SendReadAllOptionalQuery<TModelReadAllResult>(TReadAllParams readAllParams, int priority,
                Context context,
                CancellationToken cancellationToken) =>
            _apizrMediator.SendReadAllOptionalQuery<TModelReadAllResult, TReadAllResult, TReadAllParams>(readAllParams, priority, context,
                cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadOptionalQuery

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery(TApiEntityKey key, Context context) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority, Context context) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context, cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority, Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context, cancellationToken);

        #endregion

        #region SendReadOptionalQuery<TModelEntity>

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity>(TApiEntityKey key, Context context) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key,
            int priority) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity>(TApiEntityKey key, int priority, Context context) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key,
            int priority, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, Context context, CancellationToken cancellationToken) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context, cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken) =>
            _apizrMediator.SendReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context,
                cancellationToken);

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