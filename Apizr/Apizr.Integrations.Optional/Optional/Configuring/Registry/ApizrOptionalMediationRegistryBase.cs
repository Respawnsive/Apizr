using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Optional.Cruding.Sending;
using Apizr.Optional.Requesting.Sending;

namespace Apizr.Optional.Configuring.Registry
{
    /// <inheritdoc />
    public abstract class ApizrOptionalMediationRegistryBase : IApizrOptionalMediationEnumerableRegistry
    {
        protected readonly IDictionary<Type, Func<IApizrOptionalMediatorBase>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IApizrOptionalMediatorBase>>();

        protected ApizrOptionalMediationRegistryBase()
        {

        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, Func<IApizrOptionalMediatorBase>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public IApizrCrudOptionalMediator<T, int, IEnumerable<T>, IDictionary<string, object>> GetCrudOptionalMediatorFor<T>() where T : class
            => GetCrudOptionalMediatorFor<T, int, IEnumerable<T>, IDictionary<string, object>>();

        /// <inheritdoc />
        public IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetCrudOptionalMediatorFor<T, TKey>() where T : class
            => GetCrudOptionalMediatorFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        /// <inheritdoc />
        public IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetCrudOptionalMediatorFor<T, TKey, TReadAllResult>() where T : class
            => GetCrudOptionalMediatorFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        /// <inheritdoc />
        public IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> GetCrudOptionalMediatorFor<T, TKey, TReadAllResult, TReadAllParams>()
            where T : class =>
            (IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>)ConcurrentRegistry[
                typeof(IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>)].Invoke();

        /// <inheritdoc />
        public IApizrOptionalMediator<TWebApi> GetOptionalMediatorFor<TWebApi>()
            => (IApizrOptionalMediator<TWebApi>)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        /// <inheritdoc />
        public bool TryGetCrudOptionalMediatorFor<T>(out IApizrCrudOptionalMediator<T, int, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudOptionalMediatorFor<T, int, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        /// <inheritdoc />
        public bool TryGetCrudOptionalMediatorFor<T, TKey>(
            out IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudOptionalMediatorFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        /// <inheritdoc />
        public bool TryGetCrudOptionalMediatorFor<T, TKey, TReadAllResult>(
            out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudOptionalMediatorFor<T, TKey, TReadAllResult, IDictionary<string, object>>(out mediator);

        /// <inheritdoc />
        public bool TryGetCrudOptionalMediatorFor<T, TKey, TReadAllResult, TReadAllParams>(
            out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>), out var mediatorFactory))
            {
                mediator = default;
                return false;
            }

            mediator = (IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>)mediatorFactory.Invoke();
            return true;
        }

        /// <inheritdoc />
        public bool TryGetOptionalMediatorFor<TWebApi>(out IApizrOptionalMediator<TWebApi> mediator)
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(TWebApi), out var mediatorFactory))
            {
                mediator = default;
                return false;
            }

            mediator = (IApizrOptionalMediator<TWebApi>)mediatorFactory.Invoke();
            return true;
        }

        /// <inheritdoc />
        public int Count => ConcurrentRegistry.Count;

        /// <inheritdoc />
        public bool ContainsCrudOptionalMediatorFor<T>() where T : class
            => ContainsCrudOptionalMediatorFor<T, int, IEnumerable<T>, IDictionary<string, object>>();

        /// <inheritdoc />
        public bool ContainsCrudOptionalMediatorFor<T, TKey>() where T : class
            => ContainsCrudOptionalMediatorFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        /// <inheritdoc />
        public bool ContainsCrudOptionalMediatorFor<T, TKey, TReadAllResult>() where T : class
            => ContainsCrudOptionalMediatorFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        /// <inheritdoc />
        public bool ContainsCrudOptionalMediatorFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ConcurrentRegistry.ContainsKey(typeof(IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>));

        /// <inheritdoc />
        public bool ContainsOptionalMediatorFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
