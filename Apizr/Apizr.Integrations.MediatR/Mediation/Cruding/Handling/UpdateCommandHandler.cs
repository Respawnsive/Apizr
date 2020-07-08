using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling
{
    public class UpdateCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        UpdateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, UpdateCommand<TApiEntityKey, TModelEntity>, Unit>
        where TModelEntity : class
        where TApiEntity : class
    {
        public UpdateCommandHandler(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override Task<Unit> Handle(UpdateCommand<TApiEntityKey, TModelEntity> request, CancellationToken cancellationToken)
        {
            return CrudApiManager
                .ExecuteAsync((ct, api) => api.Update(request.Key, Map<TModelEntity, TApiEntity>(request.Payload), ct), cancellationToken)
                .ContinueWith(t => Unit.Value, cancellationToken);
        }
    }

    public class UpdateCommandHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> : 
        UpdateCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, UpdateCommand<TModelEntity>>
        where TModelEntity : class
        where TApiEntity : class
    {
        public UpdateCommandHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override Task<Unit> Handle(UpdateCommand<TModelEntity> request, CancellationToken cancellationToken)
        {
            return CrudApiManager
                .ExecuteAsync((ct, api) => api.Update(request.Key, Map<TModelEntity, TApiEntity>(request.Payload), ct), cancellationToken)
                .ContinueWith(t => Unit.Value, cancellationToken);
        }
    }
}
