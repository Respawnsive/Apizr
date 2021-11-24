using Apizr.Mapping;
using Apizr.Mediation.Requesting.Handling.Base;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
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
