using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Requesting;

namespace Apizr.Configuring.Registry
{
    public abstract class ApizrRegistryBase : IApizrEnumerableRegistry
    {
        protected readonly IDictionary<Type, Func<IApizrManager>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IApizrManager>>();

        protected ApizrRegistryBase()
        {
            
        }

        public IEnumerator<KeyValuePair<Type, Func<IApizrManager>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> GetFor<T, TKey>()
            where T : class
            => GetFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>();

        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> GetFor<T, TKey, TReadAllResult>() 
            where T : class
            => GetFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>();

        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> GetFor<T, TKey, TReadAllResult, TReadAllParams>() 
            where T : class
            => GetFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>();

        public IApizrManager<TWebApi> GetFor<TWebApi>()
            => (IApizrManager<TWebApi>)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        public bool TryGetFor<T, TKey>(out IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> manager) 
            where T : class
            => TryGetFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(out manager);

        public bool TryGetFor<T, TKey, TReadAllResult>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> manager) where T : class
            => TryGetFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(out manager);

        public bool TryGetFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> manager) where T : class
            => TryGetFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(out manager);

        public bool TryGetFor<TWebApi>(out IApizrManager<TWebApi> manager)
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(TWebApi), out var managerFactory))
            {
                manager = default;
                return false;
            }

            manager = (IApizrManager<TWebApi>)managerFactory.Invoke();
            return true;
        }

        public int Count => ConcurrentRegistry.Count;

        public bool ContainsFor<T, TKey>() where T : class
            => ContainsFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>();

        public bool ContainsFor<T, TKey, TReadAllResult>() where T : class
            => ContainsFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>();

        public bool ContainsFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ContainsFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>();

        public bool ContainsFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
