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
    public class ReadQueryHandler<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> :
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams,
            ReadQuery<TModelEntity, TApiEntityKey>, TModelEntity, IApizrCatchResultRequestOptions, IApizrCatchResultRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public ReadQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(
            crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<TModelEntity> Handle(ReadQuery<TModelEntity, TApiEntityKey> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>(
                    (options, api) =>
                        api.Read(request.Key, options, options.CancellationToken),
                    request.OptionsBuilder);
    }

    /// <summary>
    /// The Read query handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    public class ReadQueryHandler<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams> :
        ReadQueryHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, ReadQuery<TModelEntity>,
            TModelEntity, IApizrCatchResultRequestOptions, IApizrCatchResultRequestOptionsBuilder>
        where TModelEntity : class
        where TApiEntity : class
    {
        public ReadQueryHandler(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager)
            : base(crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<TModelEntity> Handle(ReadQuery<TModelEntity> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelEntity, TApiEntity>(
                    (options, api) =>
                        api.Read(request.Key, options, options.CancellationToken),
                    request.OptionsBuilder);
    }
}
