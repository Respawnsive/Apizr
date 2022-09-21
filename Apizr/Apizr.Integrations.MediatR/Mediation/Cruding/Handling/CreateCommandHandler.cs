using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    /// <summary>
    /// The Create command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params</typeparam>
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

        /// <inheritdoc />
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
