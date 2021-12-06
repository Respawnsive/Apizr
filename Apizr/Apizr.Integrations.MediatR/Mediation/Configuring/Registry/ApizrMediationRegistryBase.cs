using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting.Sending;

namespace Apizr.Mediation.Configuring.Registry
{
    public abstract class ApizrMediationRegistryBase : IApizrMediationEnumerableRegistry
    {
        protected readonly IDictionary<Type, Func<IApizrMediatorBase>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IApizrMediatorBase>>();

        protected ApizrMediationRegistryBase()
        {

        }

        public IEnumerator<KeyValuePair<Type, Func<IApizrMediatorBase>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IApizrCrudMediator<T, int, IEnumerable<T>, IDictionary<string, object>> GetCrudFor<T>() where T : class
            => GetCrudFor<T, int, IEnumerable<T>, IDictionary<string, object>>();

        public IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetCrudFor<T, TKey>() where T : class
            => GetCrudFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetCrudFor<T, TKey, TReadAllResult>() where T : class
            => GetCrudFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams> GetCrudFor<T, TKey, TReadAllResult, TReadAllParams>()
            where T : class =>
            (IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>)ConcurrentRegistry[
                typeof(IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>)].Invoke();

        public IApizrMediator<TWebApi> GetFor<TWebApi>()
            => (IApizrMediator<TWebApi>)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        public bool TryGetCrudFor<T>(out IApizrCrudMediator<T, int, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudFor<T, int, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        public bool TryGetCrudFor<T, TKey>(out IApizrCrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        public bool TryGetCrudFor<T, TKey, TReadAllResult>(
            out IApizrCrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudFor<T, TKey, TReadAllResult, IDictionary<string, object>>(out mediator);

        public bool TryGetCrudFor<T, TKey, TReadAllResult, TReadAllParams>(
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

        public bool ContainsCrudFor<T>() where T : class
            => ContainsCrudFor<T, int, IEnumerable<T>, IDictionary<string, object>>();

        public bool ContainsCrudFor<T, TKey>() where T : class
            => ContainsCrudFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public bool ContainsCrudFor<T, TKey, TReadAllResult>() where T : class
            => ContainsCrudFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public bool ContainsCrudFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ConcurrentRegistry.ContainsKey(typeof(IApizrCrudMediator<T, TKey, TReadAllResult, TReadAllParams>));

        public bool ContainsFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
