using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class CreateCommandHandlerBase<TModelEntity, TApiEntity, TTApiEntityKey, TReadAllResult,
        TReadAllParams, TCommand, TCommandResult> :
        CrudRequestHandlerBase<TApiEntity, TTApiEntityKey, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, TCommandResult> 
        where TModelEntity : class
        where TApiEntity : class
        where TCommand : CreateCommandBase<TModelEntity, TCommandResult>
    {
        protected CreateCommandHandlerBase(
            IApizrManager<ICrudApi<TApiEntity, TTApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager,
            IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
