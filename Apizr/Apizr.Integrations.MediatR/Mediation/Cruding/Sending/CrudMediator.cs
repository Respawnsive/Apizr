using System.Threading;
using System.Threading.Tasks;
using Fusillade;
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

        public Task<TApiEntity> SendCreateCommand(TApiEntity payload, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new CreateCommand<TApiEntity>(payload, priority), cancellationToken);
        }

        public Task<TModelEntity> SendCreateCommand<TModelEntity>(TModelEntity payload, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new CreateCommand<TModelEntity>(payload, priority), cancellationToken);
        }

        public Task<TReadAllResult> SendReadAllQuery(CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadAllQuery<TReadAllResult>(priority: priority), cancellationToken);
        }

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadAllQuery<TModelEntityReadAllResult>(priority: priority), cancellationToken);
        }

        public Task<TReadAllResult> SendReadAllQuery(TReadAllParams readAllParams,
            CancellationToken cancellationToken = default, Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadAllQuery<TReadAllParams, TReadAllResult>(readAllParams, priority), cancellationToken);
        }

        public Task<TModelEntityReadAllResult> SendReadAllQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
            CancellationToken cancellationToken = default, Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority), cancellationToken);
        }

        public Task<TApiEntity> SendReadQuery(TApiEntityKey key, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadQuery<TApiEntity, TApiEntityKey>(key, priority), cancellationToken);
        }

        public Task<TModelEntity> SendReadQuery<TModelEntity>(TApiEntityKey key, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadQuery<TModelEntity, TApiEntityKey>(key, priority), cancellationToken);
        }

        public Task SendUpdateCommand(TApiEntityKey key, TApiEntity payload, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new UpdateCommand<TApiEntityKey, TApiEntity>(key, payload, priority), cancellationToken);
        }

        public Task SendUpdateCommand<TModelEntity>(TApiEntityKey key, TModelEntity payload,
            CancellationToken cancellationToken = default, Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new UpdateCommand<TApiEntityKey, TModelEntity>(key, payload, priority), cancellationToken);
        }

        public Task SendDeleteCommand(TApiEntityKey key, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new DeleteCommand<TApiEntity, TApiEntityKey>(key, priority), cancellationToken);
        }
    }
}