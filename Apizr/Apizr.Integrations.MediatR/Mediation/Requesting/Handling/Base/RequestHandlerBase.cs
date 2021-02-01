using System;
using Apizr.Mapping;

namespace Apizr.Mediation.Requesting.Handling.Base
{
    public abstract class RequestHandlerBase
    {
        protected readonly IMappingHandler MappingHandler;

        protected RequestHandlerBase(IMappingHandler mappingHandler)
        {
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

            return destination;
        }
    }
}
