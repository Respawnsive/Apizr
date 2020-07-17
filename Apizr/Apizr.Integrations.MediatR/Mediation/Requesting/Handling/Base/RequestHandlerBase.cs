using System;
using Apizr.Mapping;
using Apizr.Requesting;

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

            if (destination == null)
                throw new ApizrException(new InvalidOperationException($"Unable to convert {typeof(TSource).Name} to {typeof(TDestination).Name} without providing an {typeof(IMappingHandler).Name} interface mapping implementation"));

            return destination;
        }
    }
}
