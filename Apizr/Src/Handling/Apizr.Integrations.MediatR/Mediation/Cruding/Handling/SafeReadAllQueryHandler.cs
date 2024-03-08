using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    /// <summary>
    /// The ReadAll query handler
    /// </summary>
    /// <typeparam name="TApiEntity"></typeparam>
    /// <typeparam name="TApiEntityKey">The api entity type</typeparam>
    /// <typeparam name="TModelReadAllResult">The returned model result type</typeparam>
    /// <typeparam name="TApiReadAllResult">The received api result type</typeparam>
    /// <typeparam name="TReadAllParams">The query parameters type</typeparam>
    public class SafeReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult,
        TReadAllParams> :
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult,
            TReadAllParams, SafeReadAllQuery<TReadAllParams, TModelReadAllResult>, IApizrResponse<TModelReadAllResult>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TApiEntity : class
    {
        public SafeReadAllQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiReadAllResult, TReadAllParams>> crudApiManager)
            : base(crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse<TModelReadAllResult>> Handle(
            SafeReadAllQuery<TReadAllParams, TModelReadAllResult> request, CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelReadAllResult, TApiReadAllResult>(
                    (options, api) => api.SafeReadAll(request.Parameters, options), request.OptionsBuilder);
    }

    /// <summary>
    /// The ReadAll query handler
    /// </summary>
    /// <typeparam name="TApiEntity"></typeparam>
    /// <typeparam name="TApiEntityKey">The api entity type</typeparam>
    /// <typeparam name="TModelReadAllResult">The returned model result type</typeparam>
    /// <typeparam name="TApiReadAllResult">The received api result type</typeparam>
    public class SafeReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult> :
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult,
            SafeReadAllQuery<TModelReadAllResult>, IApizrResponse<TModelReadAllResult>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TApiEntity : class
    {

        public SafeReadAllQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiReadAllResult, IDictionary<string, object>>>
                crudApiManager) : base(crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse<TModelReadAllResult>> Handle(SafeReadAllQuery<TModelReadAllResult> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelReadAllResult, TApiReadAllResult>(
                    (options, api) => api.SafeReadAll(request.Parameters, options), request.OptionsBuilder);
    }
}
