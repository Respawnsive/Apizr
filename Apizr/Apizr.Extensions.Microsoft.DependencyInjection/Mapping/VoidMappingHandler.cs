using System;

namespace Apizr.Mapping
{
    /// <summary>
    /// Void mapping should never be used and will just be ignored
    /// </summary>
    public class VoidMappingHandler : IMappingHandler
    {
        public TDestination Map<TDestination>(object source) => (TDestination) source;

        public TDestination Map<TSource, TDestination>(TSource source) => (TDestination)Convert.ChangeType(source, typeof(TDestination));

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) => (TDestination) Convert.ChangeType(source, typeof(TDestination));

        public object Map(object source, Type sourceType, Type destinationType) => Convert.ChangeType(source, destinationType);

        public object Map(object source, object destination, Type sourceType, Type destinationType) => Convert.ChangeType(source, destinationType);
    }
}