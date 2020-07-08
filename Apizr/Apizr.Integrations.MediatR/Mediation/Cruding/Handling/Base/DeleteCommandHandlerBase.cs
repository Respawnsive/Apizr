using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class DeleteCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, TCommand, TCommandResult> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, TCommandResult>
        where TModelEntity : class
        where TApiEntity : class 
        where TCommand : DeleteCommandBase<TModelEntity, TApiEntityKey, TCommandResult>
    {
        protected DeleteCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }

    public abstract class DeleteCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, TCommand, TCommandResult> :
        CrudRequestHandlerBase<TApiEntity, int, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, TCommandResult>
        where TModelEntity : class
        where TApiEntity : class
        where TCommand : DeleteCommandBase<TModelEntity, int, TCommandResult>
    {
        protected DeleteCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
