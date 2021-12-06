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
        protected readonly IDictionary<Type, Func<IApizrOptionalMediatorBase>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IApizrOptionalMediatorBase>>();

        protected ApizrOptionalMediationRegistryBase()
        {

        }

        public IEnumerator<KeyValuePair<Type, Func<IApizrOptionalMediatorBase>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IApizrCrudOptionalMediator<T, int, IEnumerable<T>, IDictionary<string, object>> GetCrudFor<T>() where T : class
            => GetCrudFor<T, int, IEnumerable<T>, IDictionary<string, object>>();

        public IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetCrudFor<T, TKey>() where T : class
            => GetCrudFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetCrudFor<T, TKey, TReadAllResult>() where T : class
            => GetCrudFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams> GetCrudFor<T, TKey, TReadAllResult, TReadAllParams>()
            where T : class =>
            (IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>)ConcurrentRegistry[
                typeof(IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>)].Invoke();

        public IApizrOptionalMediator<TWebApi> GetFor<TWebApi>()
            => (IApizrOptionalMediator<TWebApi>)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        public bool TryGetCrudFor<T>(out IApizrCrudOptionalMediator<T, int, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudFor<T, int, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        public bool TryGetCrudFor<T, TKey>(
            out IApizrCrudOptionalMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        public bool TryGetCrudFor<T, TKey, TReadAllResult>(
            out IApizrCrudOptionalMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class
            => TryGetCrudFor<T, TKey, TReadAllResult, IDictionary<string, object>>(out mediator);

        public bool TryGetCrudFor<T, TKey, TReadAllResult, TReadAllParams>(
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

        public bool ContainsCrudFor<T>() where T : class
            => ContainsCrudFor<T, int, IEnumerable<T>, IDictionary<string, object>>();

        public bool ContainsCrudFor<T, TKey>() where T : class
            => ContainsCrudFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public bool ContainsCrudFor<T, TKey, TReadAllResult>() where T : class
            => ContainsCrudFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public bool ContainsCrudFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ConcurrentRegistry.ContainsKey(typeof(IApizrCrudOptionalMediator<T, TKey, TReadAllResult, TReadAllParams>));

        public bool ContainsFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
