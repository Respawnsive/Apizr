using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Optional.Cruding.Sending;
using Apizr.Optional.Requesting.Sending;

namespace Apizr.Optional.Configuring.Registry
{
    public abstract class ApizrOptionalMediationRegistryBase : IApizrOptionalMediationEnumerableRegistry
    {
        protected readonly IDictionary<Type, Func<IApizrOptionalMediator>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IApizrOptionalMediator>>();

        protected ApizrOptionalMediationRegistryBase()
        {

        }

        public IEnumerator<KeyValuePair<Type, Func<IApizrOptionalMediator>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetFor<T, TKey>() where T : class
            => GetFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetFor<T, TKey, TReadAllResult>() where T : class
            => GetFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> GetFor<T, TKey, TReadAllResult, TReadAllParams>()
            where T : class =>
            (IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>)ConcurrentRegistry[
                typeof(IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>)].Invoke();

        public IApizrOptionalMediator<TWebApi> GetFor<TWebApi>()
            => (IApizrOptionalMediator<TWebApi>)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        public bool TryGetFor<T, TKey>(
            out IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        public bool TryGetFor<T, TKey, TReadAllResult>(
            out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class
            => TryGetFor<T, TKey, TReadAllResult, IDictionary<string, object>>(out mediator);

        public bool TryGetFor<T, TKey, TReadAllResult, TReadAllParams>(
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

        public bool TryGetFor<TWebApi>(out IApizrOptionalMediator<TWebApi> mediator)
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(TWebApi), out var mediatorFactory))
            {
                mediator = default;
                return false;
            }

            mediator = (IApizrOptionalMediator<TWebApi>)mediatorFactory.Invoke();
            return true;
        }

        public int Count => ConcurrentRegistry.Count;

        public bool ContainsFor<T, TKey>() where T : class
            => ContainsFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public bool ContainsFor<T, TKey, TReadAllResult>() where T : class
            => ContainsFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public bool ContainsFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ConcurrentRegistry.ContainsKey(typeof(IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>));

        public bool ContainsFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
