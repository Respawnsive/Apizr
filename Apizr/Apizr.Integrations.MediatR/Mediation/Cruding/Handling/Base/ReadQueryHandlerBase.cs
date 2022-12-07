using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Querying;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    /// <summary>
    /// The base Read query handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    /// <typeparam name="TQuery">The query type to handle</typeparam>
    /// <typeparam name="TQueryResult">The query result type to handle</typeparam>
    public abstract class ReadQueryHandlerBase<TModelEntity, TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, TQuery, TQueryResult, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams, TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IMediationQueryHandler<TQuery, TQueryResult>
        where TModelEntity : class 
        where TApiEntity : class 
        where TQuery : ReadQueryBase<TQueryResult, TApiEntityKey, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected ReadQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the Read query
        /// </summary>
        /// <param name="request">The Read query</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The base Read query handler
    /// </summary>
    /// <typeparam name="TModelEntity">The model entity type</typeparam>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params type</typeparam>
    /// <typeparam name="TQuery">The query type to handle</typeparam>
    /// <typeparam name="TQueryResult">The query result type to handle</typeparam>
    public abstract class ReadQueryHandlerBase<TModelEntity, TApiEntity, TReadAllResult, TReadAllParams, TQuery, TQueryResult, TApizrRequestOptions, TApizrRequestOptionsBuilder> : 
        CrudRequestHandlerBase<TApiEntity, int, TReadAllResult, TReadAllParams, TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IMediationQueryHandler<TQuery, TQueryResult> 
        where TApiEntity : class 
        where TQuery : ReadQueryBase<TQueryResult, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected ReadQueryHandlerBase(IApizrManager<ICrudApi<TApiEntity, int, TReadAllResult, TReadAllParams>> crudApiManager) : base(crudApiManager)
        {
        }

        /// <summary>
        /// Handling the Read query
        /// </summary>
        /// <param name="request">The Read query</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
