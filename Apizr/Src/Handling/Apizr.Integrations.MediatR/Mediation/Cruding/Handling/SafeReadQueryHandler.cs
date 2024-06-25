using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    /// <summary>
    /// The Read query handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class SafeReadQueryHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> :
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams,
            SafeReadQuery<TModelEntity, TApiEntityKey>, IApizrResponse<TModelEntity>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public SafeReadQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse<TModelEntity>> Handle(SafeReadQuery<TModelEntity, TApiEntityKey> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>(
                    (options, api) =>
                        api.SafeRead(request.Key, options),
                    request.OptionsBuilder);
    }

    /// <summary>
    /// The Read query handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class SafeReadQueryHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> :
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, SafeReadQuery<TModelEntity>,
            IApizrResponse<TModelEntity>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public SafeReadQueryHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager)
            : base(crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse<TModelEntity>> Handle(SafeReadQuery<TModelEntity> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>(
                    (options, api) =>
                        api.SafeRead(request.Key, options),
                    request.OptionsBuilder);
    }
}
