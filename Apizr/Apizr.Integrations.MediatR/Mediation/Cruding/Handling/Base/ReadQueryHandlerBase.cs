using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Querying;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class ReadQueryHandlerBase<T, TKey, TReadAllResult, TReadAllParams, TQuery, TQueryResult> :
        CrudRequestHandlerBase<T, TKey, TReadAllResult, TReadAllParams>,
        IQueryHandler<TQuery, TQueryResult> where T : class where TQuery : ReadQueryBase<TQueryResult, TKey>
    {
        protected ReadQueryHandlerBase(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager)
            : base(crudApiManager)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }

    public abstract class ReadQueryHandlerBase<T, TReadAllResult, TReadAllParams, TQuery, TQueryResult> : CrudRequestHandlerBase<T, int, TReadAllResult, TReadAllParams>,
        IQueryHandler<TQuery, TQueryResult> where T : class where TQuery : ReadQueryBase<TQueryResult>
    {
        protected ReadQueryHandlerBase(IApizrManager<ICrudApi<T, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
