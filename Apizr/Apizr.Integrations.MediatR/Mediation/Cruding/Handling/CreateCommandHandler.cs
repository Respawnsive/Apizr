using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class CreateCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : 
        CreateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, CreateCommand<TModelEntity>, TModelEntity>
        where TModelEntity : class
        where TApiEntity : class
    {
        public CreateCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager,
            IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public override Task<TModelEntity> Handle(CreateCommand<TModelEntity> request, CancellationToken cancellationToken)
        {
            return CrudApiManager
                .ExecuteAsync((ct, api) => api.Create(Map<TModelEntity, TApiEntity>(request.Payload), ct), cancellationToken)
                .ContinueWith(task => Map<TApiEntity, TModelEntity>(task.Result), cancellationToken);
        }
    }
}
