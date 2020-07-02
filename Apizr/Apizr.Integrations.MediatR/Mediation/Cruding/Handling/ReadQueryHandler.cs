using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling
{
    public class ReadQueryHandler<T, TKey, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<T, TKey, TReadAllResult, TReadAllParams, ReadQuery<T, TKey>, T> 
        where T : class
    {
        public ReadQueryHandler(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<T> Handle(ReadQuery<T, TKey> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.Read(request.Key, ct), cancellationToken);
        }
    }

    public class ReadQueryHandler<T, TReadAllResult, TReadAllParams> : 
        ReadQueryHandlerBase<T, TReadAllResult, TReadAllParams, ReadQuery<T>, T> 
        where T : class
    {
        public ReadQueryHandler(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public override Task<T> Handle(ReadQuery<T> request, CancellationToken cancellationToken)
        {
            return CrudApiManager.ExecuteAsync((ct, api) => api.Read(request.Key, ct), cancellationToken);
        }
    }
}
