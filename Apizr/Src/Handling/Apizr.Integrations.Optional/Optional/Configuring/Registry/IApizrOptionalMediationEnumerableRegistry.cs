using System;
using System.Collections.Generic;
using Apizr.Optional.Cruding.Sending;
using Apizr.Optional.Requesting.Sending;

namespace Apizr.Optional.Configuring.Registry
{
    /// <summary>
    /// Registry options available for extended registrations with optional mediation
    /// </summary>
    public interface IApizrOptionalMediationEnumerableRegistry : IEnumerable<KeyValuePair<Type, Func<IApizrOptionalMediatorBase>>>
    {
        #region Contains

        /// <summary>
        /// Optional mediators count
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Check if registry contains an optional mediator for <typeparamref name="T"/> entity type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <returns></returns>
        bool ContainsCrudOptionalMediatorFor<T>() where T : class;

        /// <summary>
        /// Check if registry contains an optional mediator for <typeparamref name="T"/> entity type with <typeparamref name="TKey"/> key type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <returns></returns>
        bool ContainsCrudOptionalMediatorFor<T, TKey>() where T : class;

        /// <summary>
        /// Check if registry contains an optional mediator for <typeparamref name="T"/> entity type with <typeparamref name="TKey"/> key type and <typeparamref name="TReadAllResult"/> ReadAll result type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll result type</typeparam>
        /// <returns></returns>
        bool ContainsCrudOptionalMediatorFor<T, TKey, TReadAllResult>() where T : class;

        /// <summary>
        /// Check if registry contains an optional mediator for <typeparamref name="T"/> entity type with <typeparamref name="TKey"/> key type,
        /// <typeparamref name="TReadAllResult"/> ReadAll result type and <typeparamref name="TReadAllParams"/> ReadAll params type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll params type</typeparam>
        /// <returns></returns>
        bool ContainsCrudOptionalMediatorFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class;

        /// <summary>
        /// Check if registry contains an optional mediator for <typeparamref name="TWebApi"/> api type
        /// </summary>
        /// <typeparam name="TWebApi">The api type</typeparam>
        /// <returns></returns>
        bool ContainsOptionalMediatorFor<TWebApi>();

        #endregion

        #region Get

        /// <summary>
        /// Get a Crud optional mediator instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        IApizrCrudOptionalMediator<T, int, IEnumerable<T>, IDictionary<string, object>> GetCrudOptionalMediatorFor<T>() where T : class;

        /// <summary>
        /// Get a Crud optional mediator instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetCrudOptionalMediatorFor<T, TKey>() where T : class;

        /// <summary>
        /// Get a Crud optional mediator instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetCrudOptionalMediatorFor<T, TKey,
            TReadAllResult>() where T : class;

        /// <summary>
        /// Get a Crud optional mediator instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> GetCrudOptionalMediatorFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        /// <summary>
        /// Get an api optional mediator instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        IApizrOptionalMediator<TWebApi> GetOptionalMediatorFor<TWebApi>();

        #endregion

        #region TryGet

        /// <summary>
        /// Try to get a Crud optional mediator instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        bool TryGetCrudOptionalMediatorFor<T>(out IApizrCrudOptionalMediator<T, int, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud optional mediator instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        bool TryGetCrudOptionalMediatorFor<T, TKey>(out IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud optional mediator instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        bool TryGetCrudOptionalMediatorFor<T, TKey, TReadAllResult>(out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud optional mediator instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        bool TryGetCrudOptionalMediatorFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class;

        /// <summary>
        /// Try to get an api optional mediator instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        bool TryGetOptionalMediatorFor<TWebApi>(out IApizrOptionalMediator<TWebApi> mediator); 

        #endregion
    }
}
