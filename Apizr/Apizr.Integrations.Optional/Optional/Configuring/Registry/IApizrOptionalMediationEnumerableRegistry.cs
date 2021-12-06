using System;
using System.Collections.Generic;
using Apizr.Configuring.Registry;
using Apizr.Optional.Cruding.Sending;
using Apizr.Optional.Requesting.Sending;

namespace Apizr.Optional.Configuring.Registry
{
    public interface IApizrOptionalMediationEnumerableRegistry : IEnumerable<KeyValuePair<Type, Func<IApizrOptionalMediatorBase>>>, IApizrEnumerableRegistryBase
    {
        /// <summary>
        /// Get a Crud optional mediator instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        IApizrCrudOptionalMediator<T, int, IEnumerable<T>, IDictionary<string, object>> GetCrudFor<T>() where T : class;

        /// <summary>
        /// Get a Crud optional mediator instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetCrudFor<T, TKey>() where T : class;

        /// <summary>
        /// Get a Crud optional mediator instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetCrudFor<T, TKey,
            TReadAllResult>() where T : class;

        /// <summary>
        /// Get a Crud optional mediator instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> GetCrudFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        /// <summary>
        /// Get an api optional mediator instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        IApizrOptionalMediator<TWebApi> GetFor<TWebApi>();

        /// <summary>
        /// Try to get a Crud optional mediator instance for an entity type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T>(out IApizrCrudOptionalMediator<T, int, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud optional mediator instance for an entity type with a specific key type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T, TKey>(out IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud optional mediator instance for an entity type with a specific key type and ReadAll result type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T, TKey, TReadAllResult>(out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class;

        /// <summary>
        /// Try to get a Crud optional mediator instance for an entity type with a specific key type, ReadAll result type and ReadAll params type
        /// </summary>
        /// <typeparam name="T">The managed entity type</typeparam>
        /// <typeparam name="TKey">The entity's key type</typeparam>
        /// <typeparam name="TReadAllResult">The ReadAll request result type</typeparam>
        /// <typeparam name="TReadAllParams">The ReadAll request params type</typeparam>
        /// <returns></returns>
        bool TryGetCrudFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class;

        /// <summary>
        /// Try to get an api optional mediator instance
        /// </summary>
        /// <typeparam name="TWebApi">The managed api type</typeparam>
        /// <returns></returns>
        bool TryGetFor<TWebApi>(out IApizrOptionalMediator<TWebApi> mediator);
    }
}
