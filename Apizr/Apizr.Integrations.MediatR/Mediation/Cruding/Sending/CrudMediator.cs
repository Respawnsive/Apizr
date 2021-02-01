using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Apizr.Mediation.Cruding.Sending
{
    public class CrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : ICrudMediator<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> where TApiEntity : class
    {
        private readonly IMediator _mediator;

        public CrudMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new CreateCommand<TApiEntity>(payload), cancellationToken);
        }

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new CreateCommand<TModelEntity>(payload), cancellationToken);
        }

        public Task<TReadAllResult> SendReadAllQuery(CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllQuery<TReadAllResult>(), cancellationToken);
        }

        public Task<TReadAllResult> SendReadAllQuery(int priority,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllQuery<TReadAllResult>(priority: priority), cancellationToken);
        }

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(), cancellationToken);
        }

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(int priority,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(priority: priority), cancellationToken);
        }

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams), cancellationToken);
        }

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority), cancellationToken);
        }

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams, 
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams), cancellationToken);
        }

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            int priority,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority), cancellationToken);
        }

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key), cancellationToken);
        }

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key,
            int priority,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority), cancellationToken);
        }

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key), cancellationToken);
        }

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key,
            int priority, 
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority), cancellationToken);
        }

        public Task SendUpdateCommand(TApiEntityKey key,
            TApiEntity payload,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload), cancellationToken);
        }

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload), cancellationToken);
        }

        public Task SendDeleteCommand(TApiEntityKey key,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key), cancellationToken);
        }
    }
}