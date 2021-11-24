using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Querying;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class ReadQueryHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, TQuery, TQueryResult> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>,
        IMediationQueryHandler<TQuery, TQueryResult>
        where TModelEntity : class 
        where TApiEntity : class 
        where TQuery : ReadQueryBase<TQueryResult, TApiEntityKey>
    {
        protected ReadQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }

    public abstract class ReadQueryHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, TQuery, TQueryResult> : CrudRequestHandlerBase<TApiEntity, int, TReadAllResult, TReadAllParams>,
        IMediationQueryHandler<TQuery, TQueryResult> where TApiEntity : class where TQuery : ReadQueryBase<TQueryResult>
    {
        protected ReadQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
