using System;
using System.Collections.Generic;
using Apizr.Configuring.Registry;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting.Sending;

namespace Apizr.Mediation.Configuring.Registry
{
    public interface IApizrMediationEnumerableRegistry : IEnumerable<KeyValuePair<Type, Func<IApizrMediatorBase>>>
    {
        #region Contains

        /// <summary>
        /// Mediators count
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Check if registry contains a mediator for <typeparamref name="T"/> entity type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <returns></returns>
        bool ContainsCrudMediatorFor<T>() where T : class;

        /// <summary>
        /// Check if registry contains a mediator for <typeparamref name="T"/> entity type with <typeparamref name="TKey"/> key type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <returns></returns>
        bool ContainsCrudMediatorFor<T, TKey>() where T : class;

        /// <summary>
        /// Check if registry contains a mediator for <typeparamref name="T"/> entity type with <typeparamref name="TKey"/> key type and <typeparamref name="TReadAllResult"/> ReadAll result type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll result type</typeparam>
        /// <returns></returns>
        bool ContainsCrudMediatorFor<T, TKey, TReadAllResult>() where T : class;

        /// <summary>
        /// Check if registry contains a mediator for <typeparamref name="T"/> entity type with <typeparamref name="TKey"/> key type,
        /// <typeparamref name="TReadAllResult"/> ReadAll result type and <typeparamref name="TReadAllParams"/> ReadAll params type
        /// </summary>
        /// <typeparam name="T">The entity type to manage</typeparam>
        /// <typeparam name="TKey">The entity key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll params type</typeparam>
        /// <returns></returns>
        bool ContainsCrudMediatorFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class;

        /// <summary>
        /// Check if registry contains a mediator for <typeparamref name="TWebApi"/> api type
        /// </summary>
        /// <typeparam name="TWebApi">The api type</typeparam>
        /// <returns></returns>
        bool ContainsMediatorFor<TWebApi>();

        #endregion

        #region Get

        /// <summary>
        /// Get a Crud mediator instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        IApizrCrudMediator<T, int, IEnumerable<T>, IDictionary<string, object>> GetCrudMediatorFor<T>() where T : class;

        /// <summary>
        /// Get a Crud mediator instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetCrudMediatorFor<T, TKey>() where T : class;

        /// <summary>
        /// Get a Crud mediator instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetCrudMediatorFor<T, TKey,
            TReadAllResult>() where T : class;

        /// <summary>
        /// Get a Crud mediator instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> GetCrudMediatorFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        /// <summary>
        /// Get an api mediator instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        IApizrMediator<TWebApi> GetMediatorFor<TWebApi>();

        #endregion

        #region TryGet

        /// <summary>
        /// Try to get a Crud mediator instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        bool TryGetCrudMediatorFor<T>(out IApizrCrudMediator<T, int, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud mediator instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        bool TryGetCrudMediatorFor<T, TKey>(out IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud mediator instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        bool TryGetCrudMediatorFor<T, TKey, TReadAllResult>(out IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud mediator instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        bool TryGetCrudMediatorFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class;

        /// <summary>
        /// Try to get an api mediator instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        bool TryGetMediatorFor<TWebApi>(out IApizrMediator<TWebApi> mediator); 

        #endregion
    }
}
