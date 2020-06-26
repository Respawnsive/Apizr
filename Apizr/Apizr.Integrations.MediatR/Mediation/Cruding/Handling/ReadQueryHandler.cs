using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Querying;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class ReadQueryHandler<T, TKey, TReadAllResult> : IQueryHandler<ReadQuery<T, TKey>, T> where T : class
    {
        private readonly IApizrManager<ICrudApi<T, TKey, TReadAllResult>> _crudApiManager;

        public ReadQueryHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult>> crudApiManager)
        {
            _crudApiManager = crudApiManager;
        }

        public virtual Task<T> Handle(ReadQuery<T, TKey> request, CancellationToken cancellationToken)
        {
            return _crudApiManager.ExecuteAsync((ct, api) => api.Read(request.Key, ct), cancellationToken);
        }
    }
}
