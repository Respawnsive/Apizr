using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class UpdateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, TCommand, TCommandResult> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, TCommandResult>
        where TModelEntity : class
        where TApiEntity : class 
        where TCommand : UpdateCommandBase<TApiEntityKey, TModelEntity, TCommandResult>
    {
        protected UpdateCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }

    public abstract class UpdateCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, TCommand> : 
        CrudRequestHandlerBase<TApiEntity, int, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, Unit>
        where TModelEntity : class
        where TApiEntity : class 
        where TCommand : UpdateCommandBase<int, TModelEntity, Unit>
    {
        protected UpdateCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public abstract Task<Unit> Handle(TCommand request, CancellationToken cancellationToken);
    }

    public abstract class UpdateCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, TCommand, TCommandResult> : 
        CrudRequestHandlerBase<TApiEntity, int, TReadAllResult, TReadAllParams>,
        ICommandHandler<TCommand, TCommandResult>
        where TModelEntity : class
        where TApiEntity : class 
        where TCommand : UpdateCommandBase<int, TModelEntity, TCommandResult>
    {
        protected UpdateCommandHandlerBase(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
