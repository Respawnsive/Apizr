using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class CreateCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> :
        CreateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams,
            CreateCommand<TModelEntity>, TModelEntity>
        where TModelEntity : class
        where TApiEntity : class
    {
        public CreateCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        public override async Task<TModelEntity> Handle(CreateCommand<TModelEntity> request,
            CancellationToken cancellationToken)
        {
            return await CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>((ctx, ct, api, apiEntity) => api.Create(apiEntity, ctx, ct),
                    request.RequestData, request.Context, cancellationToken, true, request.OnException)
                .ConfigureAwait(false);
        }
    }
}
