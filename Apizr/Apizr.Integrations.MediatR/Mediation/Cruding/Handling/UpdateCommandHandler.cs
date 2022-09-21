using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Cruding.Handling
{
    /// <summary>
    /// The Update command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
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

        /// <inheritdoc />
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

    /// <summary>
    /// The Update command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
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

        /// <inheritdoc />
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
