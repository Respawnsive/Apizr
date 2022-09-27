using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting.Sending;

namespace Apizr.Mediation.Configuring.Registry
{
    /// <summary>
    /// Registry options available for extended registrations with mediation
    /// </summary>
    public abstract class ApizrMediationRegistryBase : IApizrMediationEnumerableRegistry
    {
        protected readonly IDictionary<Type, Func<IApizrMediatorBase>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IApizrMediatorBase>>();

        protected ApizrMediationRegistryBase()
        {

        }

        /// <summary>
        /// Get factory enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Type, Func<IApizrMediatorBase>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        /// <summary>
        /// Get factory enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public IApizrCrudMediator<T, int, IEnumerable<T>, IDictionary<string, object>> GetCrudMediatorFor<T>() where T : class
            => GetCrudMediatorFor<T, int, IEnumerable<T>, IDictionary<string, object>>();

        /// <inheritdoc />
        public IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetCrudMediatorFor<T, TKey>() where T : class
            => GetCrudMediatorFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        /// <inheritdoc />
        public IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetCrudMediatorFor<T, TKey, TReadAllResult>() where T : class
            => GetCrudMediatorFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        /// <inheritdoc />
        public IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> GetCrudMediatorFor<T, TKey, TReadAllResult, TReadAllParams>()
            where T : class =>
            (IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>)ConcurrentRegistry[
                typeof(IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>)].Invoke();

        /// <inheritdoc />
        public IApizrMediator<TWebApi> GetMediatorFor<TWebApi>()
            => (IApizrMediator<TWebApi>)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        /// <inheritdoc />
        public bool TryGetCrudMediatorFor<T>(out IApizrCrudMediator<T, int, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudMediatorFor<T, int, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        /// <inheritdoc />
        public bool TryGetCrudMediatorFor<T, TKey>(out IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudMediatorFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        /// <inheritdoc />
        public bool TryGetCrudMediatorFor<T, TKey, TReadAllResult>(
            out IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudMediatorFor<T, TKey, TReadAllResult, IDictionary<string, object>>(out mediator);

        /// <inheritdoc />
        public bool TryGetCrudMediatorFor<T, TKey, TReadAllResult, TReadAllParams>(
            out IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>), out var mediatorFactory))
            {
                mediator = default;
                return false;
            }

            mediator = (IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>)mediatorFactory.Invoke();
            return true;
        }

        /// <inheritdoc />
        public bool TryGetMediatorFor<TWebApi>(out IApizrMediator<TWebApi> mediator)
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(TWebApi), out var mediatorFactory))
            {
                mediator = default;
                return false;
            }

            mediator = (IApizrMediator<TWebApi>)mediatorFactory.Invoke();
            return true;
        }

        /// <inheritdoc />
        public int Count => ConcurrentRegistry.Count;

        /// <inheritdoc />
        public bool ContainsCrudMediatorFor<T>() where T : class
            => ContainsCrudMediatorFor<T, int, IEnumerable<T>, IDictionary<string, object>>();

        /// <inheritdoc />
        public bool ContainsCrudMediatorFor<T, TKey>() where T : class
            => ContainsCrudMediatorFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        /// <inheritdoc />
        public bool ContainsCrudMediatorFor<T, TKey, TReadAllResult>() where T : class
            => ContainsCrudMediatorFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        /// <inheritdoc />
        public bool ContainsCrudMediatorFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ConcurrentRegistry.ContainsKey(typeof(IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>));

        /// <inheritdoc />
        public bool ContainsMediatorFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
