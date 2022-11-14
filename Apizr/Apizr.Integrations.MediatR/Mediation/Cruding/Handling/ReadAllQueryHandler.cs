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
    public class ReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult,
        TReadAllParams> :
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult,
            TReadAllParams, ReadAllQuery<TReadAllParams, TModelReadAllResult>, TModelReadAllResult, IApizrCatchResultRequestOptions, IApizrCatchResultRequestOptionsBuilder>
        where TApiEntity : class
    {
        public ReadAllQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiReadAllResult, TReadAllParams>> crudApiManager)
            : base(crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<TModelReadAllResult> Handle(
            ReadAllQuery<TReadAllParams, TModelReadAllResult> request, CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelReadAllResult, TApiReadAllResult>(
                    (options, api) => api.ReadAll(request.Parameters, options,
                        options.CancellationToken), request.OptionsBuilder);
    }

    /// <summary>
    /// The ReadAll query handler
    /// </summary>
    /// <typeparam name="TApiEntity"></typeparam>
    /// <typeparam name="TApiEntityKey">The api entity type</typeparam>
    /// <typeparam name="TModelReadAllResult">The returned model result type</typeparam>
    /// <typeparam name="TApiReadAllResult">The received api result type</typeparam>
    public class ReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult> :
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult,
            ReadAllQuery<TModelReadAllResult>, TModelReadAllResult, IApizrCatchResultRequestOptions, IApizrCatchResultRequestOptionsBuilder>
        where TApiEntity : class
    {

        public ReadAllQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiReadAllResult, IDictionary<string, object>>>
                crudApiManager) : base(crudApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<TModelReadAllResult> Handle(ReadAllQuery<TModelReadAllResult> request,
            CancellationToken cancellationToken) =>
            CrudApiManager
                .ExecuteAsync<TModelReadAllResult, TApiReadAllResult>(
                    (options, api) => api.ReadAll(request.Parameters, options,
                        options.CancellationToken), request.OptionsBuilder);
    }
}
