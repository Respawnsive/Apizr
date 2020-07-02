using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class UpdateCommandHandlerBase<T, TKey, TReadAllResult, TReadAllParams, TCommand, TCommandResult> : CrudRequestHandlerBase<T, TKey, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, TCommandResult> where T : class where TCommand : UpdateCommandBase<TKey, T, TCommandResult>
    {
        protected UpdateCommandHandlerBase(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }

    public abstract class UpdateCommandHandlerBase<T, TReadAllResult, TReadAllParams, TCommand> : CrudRequestHandlerBase<T, int, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, Unit> where T : class where TCommand : UpdateCommandBase<int, T, Unit>
    {
        protected UpdateCommandHandlerBase(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<Unit> Handle(TCommand request, CancellationToken cancellationToken);
    }

    public abstract class UpdateCommandHandlerBase<T, TReadAllResult, TReadAllParams, TCommand, TCommandResult> : CrudRequestHandlerBase<T, int, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, TCommandResult> where T : class where TCommand : UpdateCommandBase<int, T, TCommandResult>
    {
        protected UpdateCommandHandlerBase(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
