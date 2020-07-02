using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class DeleteCommandHandlerBase<T, TKey, TReadAllResult, TReadAllParams, TCommand, TCommandResult> :
        CrudRequestHandlerBase<T, TKey, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, TCommandResult> 
        where T : class 
        where TCommand : DeleteCommandBase<T, TKey, TCommandResult>
    {
        protected DeleteCommandHandlerBase(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }

    public abstract class DeleteCommandHandlerBase<T, TReadAllResult, TReadAllParams, TCommand, TCommandResult> :
        CrudRequestHandlerBase<T, int, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, TCommandResult>
        where T : class
        where TCommand : DeleteCommandBase<T, int, TCommandResult>
    {
        protected DeleteCommandHandlerBase(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
