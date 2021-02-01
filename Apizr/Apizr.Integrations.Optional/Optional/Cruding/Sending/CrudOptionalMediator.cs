using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Optional;

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

        public Task<Option<TApiEntity, ApizrException>> SendCreateOptionalCommand(TApiEntity payload,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new CreateOptionalCommand<TApiEntity>(payload), cancellationToken);
        }

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new CreateOptionalCommand<TModelEntity>(payload), cancellationToken);
        }

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(),
                cancellationToken);
        }

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(int priority, 
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(priority: priority),
                cancellationToken);
        }

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken = default)
        {
            return _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(),
                cancellationToken);
        }

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(int priority, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(priority: priority),
                cancellationToken);
        }

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams),
                cancellationToken);
        }

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams,
            int priority, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority),
                cancellationToken);
        }

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams),
                cancellationToken);
        }

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, int priority,
                CancellationToken cancellationToken = default)
        {
            return _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority),
                cancellationToken);
        }

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key, 
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key), cancellationToken);
        }

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority), cancellationToken);
        }

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key), cancellationToken);
        }

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key,
            int priority, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority), cancellationToken);
        }

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, payload),
                cancellationToken);
        }

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, payload),
                cancellationToken);
        }

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key),
                cancellationToken);
        }
    }
}