using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
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
            UpdateCommand<TApiEntityKey, TModelEntity>, Unit, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public UpdateCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<Unit> Handle(UpdateCommand<TApiEntityKey, TModelEntity> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>(
                    (options, api, apiEntity) =>
                        api.Update(request.Key, apiEntity, options),
                    request.RequestData,
                    request.OptionsBuilder).ContinueWith(_ => Unit.Value, cancellationToken);
    }

    /// <summary>
    /// The Update command handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class UpdateCommandHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> :
        UpdateCommandHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, UpdateCommand<TModelEntity>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public UpdateCommandHandler(
            IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<Unit> Handle(UpdateCommand<TModelEntity> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>(
                    (options, api, apiEntity) =>
                        api.Update(request.Key, apiEntity, options),
                    request.RequestData,
                    request.OptionsBuilder).ContinueWith(_ => Unit.Value, cancellationToken);
    }
}
