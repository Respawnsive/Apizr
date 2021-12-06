using System;
using System.Collections.Generic;
using Apizr.Requesting;

namespace Apizr.Configuring.Registry
{
    public interface IApizrEnumerableRegistry : IEnumerable<KeyValuePair<Type, Func<IApizrManager>>>, IApizrEnumerableRegistryBase
    {
        /// <summary>
        /// Get a Crud manager instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> GetCrudFor<T>() where T : class;

        /// <summary>
        /// Get a Crud manager instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> GetCrudFor<T, TKey>() where T : class;

        /// <summary>
        /// Get a Crud manager instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> GetCrudFor<T, TKey,
            TReadAllResult>() where T : class;

        /// <summary>
        /// Get a Crud manager instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> GetCrudFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        /// <summary>
        /// Get an api manager instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        IApizrManager<TWebApi> GetFor<TWebApi>();

        /// <summary>
        /// Try to get a Crud manager instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T>(out IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> manager) where T : class;

        /// <summary>
        /// Try to get a Crud manager instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T, TKey>(out IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> manager) where T : class;

        /// <summary>
        /// Try to get a Crud manager instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T, TKey, TReadAllResult>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> manager) where T : class;

        /// <summary>
        /// Try to get a Crud manager instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> manager) where T : class;

        /// <summary>
        /// Try to get an api manager instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        bool TryGetFor<TWebApi>(out IApizrManager<TWebApi> manager);
    }
}
