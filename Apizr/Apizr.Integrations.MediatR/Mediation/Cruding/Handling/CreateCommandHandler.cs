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

        public override async Task<TModelEntity> Handle(CreateCommand<TModelEntity> request, CancellationToken cancellationToken)
        {
            var result = await CrudApiManager
                .ExecuteAsync((ctx, ct, api) => api.Create(Map<TModelEntity, TApiEntity>(request.Payload), ctx, ct), request.Context, cancellationToken)
                .ConfigureAwait(false);

            return Map<TApiEntity, TModelEntity>(result);
        }
    }
}
