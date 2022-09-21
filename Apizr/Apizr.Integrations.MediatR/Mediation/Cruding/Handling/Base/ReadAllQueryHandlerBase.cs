using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Querying;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    /// <summary>
    /// The base ReadAll query handler
    /// </summary>
    /// <typeparam name="TApiEntity"></typeparam>
    /// <typeparam name="TApiEntityKey">The api entity type</typeparam>
    /// <typeparam name="TModelEntityReadAllResult">The returned model result type</typeparam>
    /// <typeparam name="TApiEntityReadAllResult">The received api result type</typeparam>
    /// <typeparam name="TReadAllParams">The query parameters type</typeparam>
    /// <typeparam name="TQuery">The query type to handle</typeparam>
    /// <typeparam name="TQueryResult">The query result type to handle</typeparam>
    public abstract class ReadAllQueryHandlerBase<TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TReadAllParams, TQuery, TQueryResult> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TReadAllParams>,
        IMediationQueryHandler<TQuery, TQueryResult>
        where TApiEntity : class
        where TQuery : ReadAllQueryBase<TReadAllParams, TQueryResult>
    {
        protected ReadAllQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the ReadAll query
        /// </summary>
        /// <param name="request">The ReadAll query</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The base ReadAll query handler
    /// </summary>
    /// <typeparam name="TApiEntity"></typeparam>
    /// <typeparam name="TApiEntityKey">The api entity type</typeparam>
    /// <typeparam name="TModelEntityReadAllResult">The returned model result type</typeparam>
    /// <typeparam name="TApiEntityReadAllResult">The received api result type</typeparam>
    /// <typeparam name="TQuery">The query type to handle</typeparam>
    /// <typeparam name="TQueryResult">The query result type to handle</typeparam>
    public abstract class ReadAllQueryHandlerBase< TApiEntity, TApiEntityKey, TModelEntityReadAllResult, TApiEntityReadAllResult, TQuery, TQueryResult> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>,
        IMediationQueryHandler<TQuery, TQueryResult>
        where TApiEntity : class 
        where TQuery : ReadAllQueryBase<TQueryResult>
    {
        protected ReadAllQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, IDictionary<string, object>>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the ReadAll query
        /// </summary>
        /// <param name="request">The ReadAll query</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
