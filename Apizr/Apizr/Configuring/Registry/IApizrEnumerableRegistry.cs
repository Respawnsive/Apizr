using System;
using System.Collections.Generic;
using Apizr.Requesting;

namespace Apizr.Configuring.Registry
{
    public interface IApizrEnumerableRegistry : IEnumerable<KeyValuePair<Type, Func<IApizrManager>>>
    {
        #region Contains

        /// <summary>
        /// Managers count
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="T"/> entity type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <returns></returns>
        bool ContainsCrudManagerFor<T>() where T : class;

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="T"/> entity type with <typeparamref name="TKey"/> key type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <returns></returns>
        bool ContainsCrudManagerFor<T, TKey>() where T : class;

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="T"/> entity type with <typeparamref name="TKey"/> key type and <typeparamref name="TReadAllResult"/> ReadAll result type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll result type</typeparam>
        /// <returns></returns>
        bool ContainsCrudManagerFor<T, TKey, TReadAllResult>() where T : class; 

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="T"/> entity type with <typeparamref name="TKey"/> key type,
        /// <typeparamref name="TReadAllResult"/> ReadAll result type and <typeparamref name="TReadAllParams"/> ReadAll params type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll params type</typeparam>
        /// <returns></returns>
        bool ContainsCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class;

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="TWebApi"/> api type
        /// </summary>
        /// <typeparam name="TWebApi">The api type</typeparam>
        /// <returns></returns>
        bool ContainsManagerFor<TWebApi>();

        #endregion

        #region Get

        /// <summary>
        /// Get a Crud manager instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> GetCrudManagerFor<T>() where T : class;

        /// <summary>
        /// Get a Crud manager instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> GetCrudManagerFor<T, TKey>() where T : class;

        /// <summary>
        /// Get a Crud manager instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> GetCrudManagerFor<T, TKey,
            TReadAllResult>() where T : class;

        /// <summary>
        /// Get a Crud manager instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> GetCrudManagerFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        /// <summary>
        /// Get an api manager instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        IApizrManager<TWebApi> GetManagerFor<TWebApi>();

        #endregion

        #region TryGet

        /// <summary>
        /// Try to get a Crud manager instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        bool TryGetCrudManagerFor<T>(out IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> manager) where T : class;

        /// <summary>
        /// Try to get a Crud manager instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        bool TryGetCrudManagerFor<T, TKey>(out IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> manager) where T : class;

        /// <summary>
        /// Try to get a Crud manager instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        bool TryGetCrudManagerFor<T, TKey, TReadAllResult>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> manager) where T : class;

        /// <summary>
        /// Try to get a Crud manager instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        bool TryGetCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> manager) where T : class;

        /// <summary>
        /// Try to get an api manager instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        bool TryGetManagerFor<TWebApi>(out IApizrManager<TWebApi> manager); 

        #endregion
    }
}
