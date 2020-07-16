using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling
{
    public class DeleteCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, DeleteCommand<TModelEntity, TApiEntityKey>, Unit>
        where TModelEntity : class
        where TApiEntity : class
    {
        public DeleteCommandHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override Task<Unit> Handle(DeleteCommand<TModelEntity, TApiEntityKey> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.Delete(request.Key, ct), cancellationToken, request.Priority)
                .ContinueWith(t => Unit.Value, cancellationToken);
        }
    }

    public class DeleteCommandHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> : 
        DeleteCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, DeleteCommand<TModelEntity>, Unit>
        where TModelEntity : class
        where TApiEntity : class
    {
        public DeleteCommandHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override Task<Unit> Handle(DeleteCommand<TModelEntity> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.Delete(request.Key, ct), cancellationToken, request.Priority)
                .ContinueWith(t => Unit.Value, cancellationToken);
        }
    }
}
