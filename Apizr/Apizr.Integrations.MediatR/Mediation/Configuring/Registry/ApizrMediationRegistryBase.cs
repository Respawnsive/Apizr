using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Requesting;

namespace Apizr.Mediation.Configuring.Registry
{
    public abstract class ApizrMediationRegistryBase : IApizrMediationEnumerableRegistry
    {
        protected readonly IDictionary<Type, Func<IApizrMediator>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IApizrMediator>>();

        protected ApizrMediationRegistryBase()
        {

        }

        public IEnumerator<KeyValuePair<Type, Func<IApizrMediator>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetFor<T, TKey>() where T : class
            => GetFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetFor<T, TKey, TReadAllResult>() where T : class
            => GetFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> GetFor<T, TKey, TReadAllResult, TReadAllParams>()
            where T : class =>
            (IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>)ConcurrentRegistry[
                typeof(IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>)].Invoke();

        public IApizrMediator<TWebApi> GetFor<TWebApi>()
            => (IApizrMediator<TWebApi>)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        public bool TryGetFor<T, TKey>(out IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        public bool TryGetFor<T, TKey, TReadAllResult>(
            out IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class
            => TryGetFor<T, TKey, TReadAllResult, IDictionary<string, object>>(out mediator);

        public bool TryGetFor<T, TKey, TReadAllResult, TReadAllParams>(
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

        public bool TryGetFor<TWebApi>(out IApizrMediator<TWebApi> mediator)
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(TWebApi), out var mediatorFactory))
            {
                mediator = default;
                return false;
            }

            mediator = (IApizrMediator<TWebApi>)mediatorFactory.Invoke();
            return true;
        }

        public int Count => ConcurrentRegistry.Count;

        public bool ContainsFor<T, TKey>() where T : class
            => ContainsFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public bool ContainsFor<T, TKey, TReadAllResult>() where T : class
            => ContainsFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public bool ContainsFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ConcurrentRegistry.ContainsKey(typeof(IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>));

        public bool ContainsFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
