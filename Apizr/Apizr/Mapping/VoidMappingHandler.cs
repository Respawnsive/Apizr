using System;

namespace Apizr.Mapping
{
    /// <summary>
    /// Void mapping should never be used and will just be ignored
    /// </summary>
    public class VoidMappingHandler : IMappingHandler
    {
        /// <inheritdoc />
        public TDestination Map<TDestination>(object source) => (TDestination) source;

        /// <inheritdoc />
        public TDestination Map<TSource, TDestination>(TSource source) => (TDestination)Convert.ChangeType(source, typeof(TDestination));

        /// <inheritdoc />
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) => (TDestination) Convert.ChangeType(source, typeof(TDestination));

        /// <inheritdoc />
        public object Map(object source, Type sourceType, Type destinationType) => Convert.ChangeType(source, destinationType);

        /// <inheritdoc />
        public object Map(object source, object destination, Type sourceType, Type destinationType) => Convert.ChangeType(source, destinationType);
    }
}