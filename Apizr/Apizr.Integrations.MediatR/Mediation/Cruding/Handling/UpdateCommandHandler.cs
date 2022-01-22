using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling
{
    public class UpdateCommandHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> :
        UpdateCommandHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams,
            UpdateCommand<TApiEntityKey, TModelEntity>, Unit>
        where TModelEntity : class
        where TApiEntity : class
    {
        public UpdateCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        public override async Task<Unit> Handle(UpdateCommand<TApiEntityKey, TModelEntity> request,
            CancellationToken cancellationToken)
        {
            await CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>(
                    (ctx, ct, api, apiEntity) => api.Update(request.Key, apiEntity, ctx, ct), request.RequestData,
                    request.Context, cancellationToken, request.OnException)
                .ConfigureAwait(false);

            return Unit.Value;
        }
    }

    public class UpdateCommandHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> :
        UpdateCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, UpdateCommand<TModelEntity>>
        where TModelEntity : class
        where TApiEntity : class
    {
        public UpdateCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        public override async Task<Unit> Handle(UpdateCommand<TModelEntity> request,
            CancellationToken cancellationToken)
        {
            await CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>(
                    (ctx, ct, api, apiEntity) => api.Update(request.Key, apiEntity, ctx, ct), request.RequestData,
                    request.Context, cancellationToken, request.OnException)
                .ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
