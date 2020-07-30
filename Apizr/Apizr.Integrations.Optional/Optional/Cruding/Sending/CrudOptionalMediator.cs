using System.Threading;
using System.Threading.Tasks;
using Fusillade;
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
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new CreateOptionalCommand<TApiEntity>(payload, priority), cancellationToken);
        }

        public Task<Option<TModelEntity, ApizrException>> SendCreateOptionalCommand<TModelEntity>(TModelEntity payload,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new CreateOptionalCommand<TModelEntity>(payload, priority), cancellationToken);
        }

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(priority: priority),
                cancellationToken);
        }

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(CancellationToken cancellationToken = default,
                Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(priority: priority),
                cancellationToken);
        }

        public Task<Option<TReadAllResult, ApizrException<TReadAllResult>>> SendReadAllOptionalQuery(
            TReadAllParams readAllParams, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadAllOptionalQuery<TReadAllParams, TReadAllResult>(readAllParams, priority),
                cancellationToken);
        }

        public Task<Option<TModelEntityReadAllResult, ApizrException<TModelEntityReadAllResult>>>
            SendReadAllOptionalQuery<TModelEntityReadAllResult>(TReadAllParams readAllParams,
                CancellationToken cancellationToken = default, Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(
                new ReadAllOptionalQuery<TReadAllParams, TModelEntityReadAllResult>(readAllParams, priority),
                cancellationToken);
        }

        public Task<Option<TApiEntity, ApizrException<TApiEntity>>> SendReadOptionalQuery(TApiEntityKey key,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadOptionalQuery<TApiEntity, TApiEntityKey>(key, priority), cancellationToken);
        }

        public Task<Option<TModelEntity, ApizrException<TModelEntity>>> SendReadOptionalQuery<TModelEntity>(
            TApiEntityKey key, CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new ReadOptionalQuery<TModelEntity, TApiEntityKey>(key, priority), cancellationToken);
        }

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand(TApiEntityKey key, TApiEntity payload,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TApiEntity>(key, payload, priority),
                cancellationToken);
        }

        public Task<Option<Unit, ApizrException>> SendUpdateOptionalCommand<TModelEntity>(TApiEntityKey key,
            TModelEntity payload,
            CancellationToken cancellationToken = default, Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new UpdateOptionalCommand<TApiEntityKey, TModelEntity>(key, payload, priority),
                cancellationToken);
        }

        public Task<Option<Unit, ApizrException>> SendDeleteOptionalCommand(TApiEntityKey key,
            CancellationToken cancellationToken = default,
            Priority priority = Priority.UserInitiated)
        {
            return _mediator.Send(new DeleteOptionalCommand<TApiEntity, TApiEntityKey>(key, priority),
                cancellationToken);
        }
    }
}