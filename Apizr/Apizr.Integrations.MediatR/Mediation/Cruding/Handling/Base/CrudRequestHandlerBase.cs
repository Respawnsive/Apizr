using System;
using Apizr.Mapping;
using Apizr.Requesting;

namespace Apizr.Mediation.Cruding.Handling.Base
{
    public abstract class CrudRequestHandlerBase<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams> where TApiEntity : class
    {
        protected readonly IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> CrudApiManager;
        protected readonly IMappingHandler MappingHandler;

        protected CrudRequestHandlerBase(IApizrManager<ICrudApi<TApiEntity, TApiEntityKey, TReadAllResult, TReadAllParams>> crudApiManager, IMappingHandler mappingHandler)
        {
            CrudApiManager = crudApiManager;
            MappingHandler = mappingHandler;
        }

        protected virtual TDestination Map<TSource, TDestination>(TSource source)
        {
            TDestination destination = default;
            try
            {
                if (typeof(TSource) == typeof(TDestination))
                    destination = (TDestination)Convert.ChangeType(source, typeof(TDestination));
                else if (MappingHandler.GetType() != typeof(VoidMappingHandler))
                    destination = MappingHandler.Map<TSource, TDestination>(source);
            }
            catch (Exception e)
            {
                throw new ApizrException(e);
            }

            if (destination == null)
                throw new ApizrException(new InvalidOperationException($"Unable to convert {typeof(TSource).Name} to {typeof(TDestination).Name} without providing an {typeof(IMappingHandler).Name} interface mapping implementation"));

            return destination;
        }
    }
}
