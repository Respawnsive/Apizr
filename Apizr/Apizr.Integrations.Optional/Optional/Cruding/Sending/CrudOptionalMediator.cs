using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding.Sending
{
    public class CrudOptionalMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : ICrudOptionalMediator<
            TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> where TApiEntity : class
    {
        private readonly IMediator _mediator;

        public CrudOptionalMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Create

        #region SendCreateOptionalCommand

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload) =>
            _mediator.Send(new CreateOptionalCommand<TApiEntity>(payload));

        public Task<Option<TApiEntity, ApizrException>>
            SendCreateOptionalCommand(TApiEntity payload, Context context) =>
            _mediator.Send(new CreateOptionalCommand<TApiEntity>(payload, context));

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateOptionalCommand<TApiEntity>(payload), cancellationToken);

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateOptionalCommand<TApiEntity>(payload, context), cancellationToken);

        #endregion

        #region SendCreateOptionalCommand<TModelEntity>

        public Task<Option<TModelEntity, ApizrException>>
            SendCreateOptionalCommand<TModelEntity>(TModelEntity payload) =>
            _mediator.Send(new CreateOptionalCommand<TModelEntity>(payload));

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload,
            Context context) => _mediator.Send(new CreateOptionalCommand<TModelEntity>(payload, context));

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateOptionalCommand<TModelEntity>(payload), cancellationToken);

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload,
            Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new CreateOptionalCommand<TModelEntity>(payload, context), cancellationToken);

        #endregion

        #endregion

        #region ReadAll

        #region SendReadAllOptionalQuery

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery() =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>());

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(Context context) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(context));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(priority));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery(int priority, Context context) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(priority, context));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(priority),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(context),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority,
            Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(priority, context),
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelEntityReadAllResult>

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>() =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>());

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(Context context) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(context));

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(),
                cancellationToken);

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(int priority) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(priority));

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(int priority, Context context) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(priority, context));

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(int priority, CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(priority: priority),
                cancellationToken);

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(Context context, CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(context),
                cancellationToken);

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(int priority, Context context,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(priority, context),
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery(TReadAllParams)

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery(TReadAllParams readAllParams, Context context) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, context));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>>
            SendReadAllOptionalQuery(TReadAllParams readAllParams, int priority, Context context) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context));

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, context),
                cancellationToken);

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority, context),
                cancellationToken);

        #endregion

        #region SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams)

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams));

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Context context) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, context));

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams),
                cancellationToken);

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, int priority) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority));

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, int priority,
                Context context) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority, context));

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, int priority,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority),
                cancellationToken);

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, Context context,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, context),
                cancellationToken);

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, int priority,
                Context context,
                CancellationToken cancellationToken) =>
            _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority, context),
                cancellationToken);

        #endregion

        #endregion

        #region Read

        #region SendReadOptionalQuery

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>>
            SendReadOptionalQuery(TApiEntityKey key, Context context) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key), cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority, Context context) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context));

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority), cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, context), cancellationToken);

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority, Context context, CancellationToken cancellationToken) => _mediator.Send(
            new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority, context), cancellationToken);

        #endregion

        #region SendReadOptionalQuery<TModelEntity>

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity>(TApiEntityKey key, Context context) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key), cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key,
            int priority) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>>
            SendReadOptionalQuery<TModelEntity>(TApiEntityKey key, int priority, Context context) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context));

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key,
            int priority, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority), cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, Context context, CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, context), cancellationToken);

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, int priority, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority, context),
                cancellationToken);

        #endregion

        #endregion

        #region Update

        #region SendUpdateOptionalCommand

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, payload));

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload,
            Context context) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, payload, context));

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, payload),
                cancellationToken);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload,
            Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, payload, context),
                cancellationToken);

        #endregion

        #region SendUpdateOptionalCommand<TModelEntity>

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, payload));

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, Context context) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, payload, context));

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, payload),
                cancellationToken);

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, payload, context),
                cancellationToken);

        #endregion

        #endregion

        #region Delete

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key) =>
            _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key));

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key, Context context) =>
            _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key, context));

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key,
            CancellationToken cancellationToken) =>
            _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key),
                cancellationToken);

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key, Context context,
            CancellationToken cancellationToken) =>
            _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key, context),
                cancellationToken);

        #endregion
    }
}