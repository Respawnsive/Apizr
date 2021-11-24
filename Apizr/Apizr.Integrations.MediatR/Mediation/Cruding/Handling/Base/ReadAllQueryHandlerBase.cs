using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Querying;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TReadAllParams, TQuery, TQueryResult> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TReadAllParams>,
        IMediationQueryHandler<TQuery, TQueryResult>
        where TApiEntity : class
        where TQuery : ReadAllQueryBase<TReadAllParams, TQueryResult>
    {
        protected ReadAllQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }

    public abstract class ReadAllQueryHandlerBase< TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TQuery, TQueryResult> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>,
        IMediationQueryHandler<TQuery, TQueryResult>
        where TApiEntity : class 
        where TQuery : ReadAllQueryBase<TQueryResult>
    {
        protected ReadAllQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
