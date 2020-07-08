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
        IQueryHandler<TQuery, TQueryResult>
        where TApiEntity : class
        where TQuery : ReadAllQueryBase<TReadAllParams, TQueryResult>
    {
        protected ReadAllQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }

    public abstract class ReadAllQueryHandlerBase< TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TQuery, TQueryResult> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>,
        IQueryHandler<TQuery, TQueryResult>
        where TApiEntity : class 
        where TQuery : ReadAllQueryBase<TQueryResult>
    {
        protected ReadAllQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>> crudApiManager, IMappingHandler mappingHandler) : base(crudApiManager, mappingHandler)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
