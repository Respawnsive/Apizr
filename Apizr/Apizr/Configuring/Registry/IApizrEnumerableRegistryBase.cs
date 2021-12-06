using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Configuring.Registry
{
    public interface IApizrEnumerableRegistryBase
    {
        /// <summary>
        /// Managers count
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Check if registry contains a manager for <see cref="T"/> entity type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <returns></returns>
        bool ContainsCrudFor<T>() where T : class;

        /// <summary>
        /// Check if registry contains a manager for <see cref="T"/> entity type with <see cref="TKey"/> key type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <returns></returns>
        bool ContainsCrudFor<T, TKey>() where T : class;

        /// <summary>
        /// Check if registry contains a manager for <see cref="T"/> entity type with <see cref="TKey"/> key type and <see cref="TReadAllResult"/> ReadAll result type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll result type</typeparam>
        /// <returns></returns>
        bool ContainsCrudFor<T, TKey, TReadAllResult>() where T : class;

        /// <summary>
        /// Check if registry contains a manager for <see cref="T"/> entity type with <see cref="TKey"/> key type,
        /// <see cref="TReadAllResult"/> ReadAll result type and <see cref="TReadAllParams"/> ReadAll params type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll params type</typeparam>
        /// <returns></returns>
        bool ContainsCrudFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class;

        /// <summary>
        /// Check if registry contains a manager for <see cref="TWebApi"/> api type
        /// </summary>
        /// <typeparam name="TWebApi">The api type</typeparam>
        /// <returns></returns>
        bool ContainsFor<TWebApi>();
    }
}
