using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Requesting;

namespace Apizr.Mediation.Configuring.Registry
{
    public abstract class ApizrMediationRegistryBase : IApizrMediationRegistryBase
    {
        protected readonly IDictionary<Type, Func<IMediator>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IMediator>>();

        protected ApizrMediationRegistryBase()
        {

        }

        public IEnumerator<KeyValuePair<Type, Func<IMediator>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public ICrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> GetFor<T, TKey>() where T : class
            => GetFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public ICrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> GetFor<T, TKey, TReadAllResult>() where T : class
            => GetFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public ICrudMediator<T, TKey, TReadAllResult, TReadAllParams> GetFor<T, TKey, TReadAllResult, TReadAllParams>()
            where T : class =>
            (ICrudMediator<T, TKey, TReadAllResult, TReadAllParams>)ConcurrentRegistry[
                typeof(ICrudMediator<T, TKey, TReadAllResult, TReadAllParams>)].Invoke();

        public IMediator<TWebApi> GetFor<TWebApi>()
            => (IMediator<TWebApi>)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        public bool TryGetFor<T, TKey>(out ICrudMediator<T, TKey, IEnumerable<T>, IDictionary<string, object>> mediator) where T : class
            => TryGetFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>(out mediator);

        public bool TryGetFor<T, TKey, TReadAllResult>(
            out ICrudMediator<T, TKey, TReadAllResult, IDictionary<string, object>> mediator) where T : class
            => TryGetFor<T, TKey, TReadAllResult, IDictionary<string, object>>(out mediator);

        public bool TryGetFor<T, TKey, TReadAllResult, TReadAllParams>(
            out ICrudMediator<T, TKey, TReadAllResult, TReadAllParams> mediator) where T : class
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(ICrudMediator<T, TKey, TReadAllResult, TReadAllParams>), out var mediatorFactory))
            {
                mediator = default;
                return false;
            }

            mediator = (ICrudMediator<T, TKey, TReadAllResult, TReadAllParams>)mediatorFactory.Invoke();
            return true;
        }

        public bool TryGetFor<TWebApi>(out IMediator<TWebApi> mediator)
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(TWebApi), out var mediatorFactory))
            {
                mediator = default;
                return false;
            }

            mediator = (IMediator<TWebApi>)mediatorFactory.Invoke();
            return true;
        }

        public int Count => ConcurrentRegistry.Count;

        public bool ContainsFor<T, TKey>() where T : class
            => ContainsFor<T, TKey, IEnumerable<T>, IDictionary<string, object>>();

        public bool ContainsFor<T, TKey, TReadAllResult>() where T : class
            => ContainsFor<T, TKey, TReadAllResult, IDictionary<string, object>>();

        public bool ContainsFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ConcurrentRegistry.ContainsKey(typeof(ICrudMediator<T, TKey, TReadAllResult, TReadAllParams>));

        public bool ContainsFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
