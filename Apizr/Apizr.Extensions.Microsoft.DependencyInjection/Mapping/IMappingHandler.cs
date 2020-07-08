// Copied from https://github.com/AutoMapper/AutoMapper/blob/master/src/AutoMapper/IMapper.cs

using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Mapping
{
    public interface IMappingHandler
    {
        /// <summary>
        /// Execute a mapping from the source object to a new destination object.
        /// The source type is inferred from the source object.
        /// </summary>
        /// <typeparam name="TDestination">Destination type to create</typeparam>
        /// <param name="source">Source object to map from</param>
        /// <returns>Mapped destination object</returns>
        TDestination Map<TDestination>(object source);
        /// <summary>
        /// Execute a mapping from the source object to a new destination object.
        /// </summary>
        /// <typeparam name="TSource">Source type to use, regardless of the runtime type</typeparam>
        /// <typeparam name="TDestination">Destination type to create</typeparam>
        /// <param name="source">Source object to map from</param>
        /// <returns>Mapped destination object</returns>
        TDestination Map<TSource, TDestination>(TSource source);
        /// <summary>
        /// Execute a mapping from the source object to the existing destination object.
        /// </summary>
        /// <typeparam name="TSource">Source type to use</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Source object to map from</param>
        /// <param name="destination">Destination object to map into</param>
        /// <returns>The mapped destination object, same instance as the <paramref name="destination"/> object</returns>
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
        /// <summary>
        /// Execute a mapping from the source object to a new destination object with explicit <see cref="System.Type"/> objects
        /// </summary>
        /// <param name="source">Source object to map from</param>
        /// <param name="sourceType">Source type to use</param>
        /// <param name="destinationType">Destination type to create</param>
        /// <returns>Mapped destination object</returns>
        object Map(object source, Type sourceType, Type destinationType);
        /// <summary>
        /// Execute a mapping from the source object to existing destination object with explicit <see cref="System.Type"/> objects
        /// </summary>
        /// <param name="source">Source object to map from</param>
        /// <param name="destination">Destination object to map into</param>
        /// <param name="sourceType">Source type to use</param>
        /// <param name="destinationType">Destination type to use</param>
        /// <returns>Mapped destination object, same instance as the <paramref name="destination"/> object</returns>
        object Map(object source, object destination, Type sourceType, Type destinationType);
    }
}
