using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class CrudRequestHandlerBase<T, TKey, TReadAllResult, TReadAllParams> where T : class
    {
        protected readonly IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> CrudApiManager;

        protected CrudRequestHandlerBase(IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> crudApiManager)
        {
            CrudApiManager = crudApiManager;
        }
    }
}
