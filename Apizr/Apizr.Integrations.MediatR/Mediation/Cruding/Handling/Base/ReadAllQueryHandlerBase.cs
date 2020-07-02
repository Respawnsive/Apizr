using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Querying;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class ReadAllQueryHandlerBase<T, TKey, TReadAllResult, TReadAllParams, TQuery, TQueryResult> :
        CrudRequestHandlerBase<T, TKey, TReadAllResult, TReadAllParams>,
        IQueryHandler<TQuery, TQueryResult> where T : class
        where TQuery : ReadAllQueryBase<TReadAllParams, TQueryResult>
    {
        protected ReadAllQueryHandlerBase(
            IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }

    public abstract class ReadAllQueryHandlerBase<T, TKey, TReadAllResult, TQuery, TQueryResult> :
        CrudRequestHandlerBase<T, TKey, TReadAllResult, IDictionary<string, object>>,
        IQueryHandler<TQuery, TQueryResult> where T : class where TQuery : ReadAllQueryBase<TQueryResult>
    {
        protected ReadAllQueryHandlerBase(
            IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> crudApiManager) : base(
            crudApiManager)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
