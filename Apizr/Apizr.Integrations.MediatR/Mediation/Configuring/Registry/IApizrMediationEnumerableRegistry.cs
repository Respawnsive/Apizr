using System;
using System.Collections.Generic;
using Apizr.Configuring.Registry;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting.Sending;

namespace Apizr.Mediation.Configuring.Registry
{
    public interface IApizrMediationEnumerableRegistry : IEnumerable<KeyValuePair<Type, Func<IApizrMediatorBase>>>, IApizrEnumerableRegistryBase
    {
        /// <summary>
        /// Get a Crud mediator instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        IApizrCrudMediator<T, int, IEnumerable<T>, IDictionary<string, object>> GetCrudFor<T>() where T : class;

        /// <summary>
        /// Get a Crud mediator instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetCrudFor<T, TKey>() where T : class;

        /// <summary>
        /// Get a Crud mediator instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetCrudFor<T, TKey,
            TReadAllResult>() where T : class;

        /// <summary>
        /// Get a Crud mediator instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> GetCrudFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        /// <summary>
        /// Get an api mediator instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        IApizrMediator<TWebApi> GetFor<TWebApi>();

        /// <summary>
        /// Try to get a Crud mediator instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T>(out IApizrCrudMediator<T, int, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud mediator instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T, TKey>(out IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud mediator instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T, TKey, TReadAllResult>(out IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud mediator instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class;

        /// <summary>
        /// Try to get an api mediator instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        bool TryGetFor<TWebApi>(out IApizrMediator<TWebApi> mediator);
    }
}
