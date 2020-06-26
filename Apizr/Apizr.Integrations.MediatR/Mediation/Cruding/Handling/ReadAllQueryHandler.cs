using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Querying;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class ReadAllQueryHandler<T, TKey, TReadAllResult> : IQueryHandler<ReadAllQuery<TReadAllResult>, TReadAllResult> where T : class
    {
        private readonly IApizrManager<ICrudApi<T, TKey, TReadAllResult>> _crudApiManager;

        public ReadAllQueryHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult>> crudApiManager)
        {
            _crudApiManager = crudApiManager;
        }

        public virtual Task<TReadAllResult> Handle(ReadAllQuery<TReadAllResult> request, CancellationToken cancellationToken)
        {
            return _crudApiManager.ExecuteAsync((ct, api) => api.ReadAll(ct), cancellationToken);
        }
    }
}
