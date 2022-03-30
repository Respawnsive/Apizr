using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class ReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult,
        TReadAllParams> :
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult,
            TReadAllParams, ReadAllQuery<TReadAllParams, TModelReadAllResult>, TModelReadAllResult>
        where TApiEntity : class
    {
        public ReadAllQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiReadAllResult, TReadAllParams>> crudApiManager)
            : base(crudApiManager)
        {
        }

        public override async Task<TModelReadAllResult> Handle(
            ReadAllQuery<TReadAllParams, TModelReadAllResult> request, CancellationToken cancellationToken)
        {
            return await CrudApiManager
                .ExecuteAsync<TModelReadAllResult, TApiReadAllResult>(
                    (ctx, ct, api) => api.ReadAll(request.Parameters, request.Priority, ctx, ct), request.Context,
                    cancellationToken, request.ClearCache, request.OnException)
                .ConfigureAwait(false);
        }
    }

    public class ReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult> :
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelReadAllResult, TApiReadAllResult,
            ReadAllQuery<TModelReadAllResult>, TModelReadAllResult>
        where TApiEntity : class
    {

        public ReadAllQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiReadAllResult, IDictionary<string, object>>>
                crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<TModelReadAllResult> Handle(ReadAllQuery<TModelReadAllResult> request,
            CancellationToken cancellationToken)
        {
            return await CrudApiManager
                .ExecuteAsync<TModelReadAllResult, TApiReadAllResult>(
                    (ctx, ct, api) => api.ReadAll(request.Parameters, request.Priority, ctx, ct), request.Context,
                    cancellationToken, request.ClearCache, request.OnException)
                .ConfigureAwait(false);
        }
    }
}
