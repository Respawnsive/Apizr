using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class ReadAllQueryHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        ReadAllQueryHandlerBase<T, TKey, TReadAllResult, TReadAllParams, ReadAllQuery<TReadAllParams, TReadAllResult>, TReadAllResult> 
        where T : class
    {
        public ReadAllQueryHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<TReadAllResult> Handle(ReadAllQuery<TReadAllParams, TReadAllResult> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.ReadAll(request.Parameters, ct), cancellationToken);
        }
    }

    public class ReadAllQueryHandler<T, TKey, TReadAllResult> : 
        ReadAllQueryHandlerBase<T, TKey, TReadAllResult, ReadAllQuery<TReadAllResult>, TReadAllResult> 
        where T : class
    {
        public ReadAllQueryHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<TReadAllResult> Handle(ReadAllQuery<TReadAllResult> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.ReadAll(request.Parameters, ct), cancellationToken);
        }
    }
}
