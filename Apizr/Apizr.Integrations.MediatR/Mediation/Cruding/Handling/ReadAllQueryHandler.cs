using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class ReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult,
        TReadAllParams> :
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult,
            TReadAllParams, ReadAllQuery<TReadAllParams, TModelEntityReadAllResult>, TModelEntityReadAllResult>
        where TApiEntity : class
    {
        public ReadAllQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TReadAllParams>> crudApiManager)
            : base(crudApiManager)
        {
        }

        public override async Task<TModelEntityReadAllResult> Handle(
            ReadAllQuery<TReadAllParams, TModelEntityReadAllResult> request, CancellationToken cancellationToken)
        {
            return await CrudApiManager
                .ExecuteAsync<TModelEntityReadAllResult, TApiEntityReadAllResult>(
                    (ctx, ct, api) => api.ReadAll(request.Parameters, request.Priority, ctx, ct), request.Context,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public class ReadAllQueryHandler<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult> :
        ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult,
            ReadAllQuery<TModelEntityReadAllResult>, TModelEntityReadAllResult>
        where TApiEntity : class
    {

        public ReadAllQueryHandler(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>>
                crudApiManager) : base(crudApiManager)
        {
        }

        public override async Task<TModelEntityReadAllResult> Handle(ReadAllQuery<TModelEntityReadAllResult> request,
            CancellationToken cancellationToken)
        {
            return await CrudApiManager
                .ExecuteAsync<TModelEntityReadAllResult, TApiEntityReadAllResult>(
                    (ctx, ct, api) => api.ReadAll(request.Parameters, request.Priority, ctx, ct), request.Context,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
