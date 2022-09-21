using Apizr.Mediation.Requesting.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    /// <summary>
    /// The base CRUD request handler
    /// </summary>
    /// <typeparam name="TApiEntity">The api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The api entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The returned result type</typeparam>
    /// <typeparam name="TReadAllParams">The read all params</typeparam>
    public abstract class
        CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> : RequestHandlerBase
        where TApiEntity : class
    {
        protected readonly IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> CrudApiManager;

        protected CrudRequestHandlerBase(
            IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager) : base()
        {
            CrudApiManager = crudApiManager;
        }
    }
}
