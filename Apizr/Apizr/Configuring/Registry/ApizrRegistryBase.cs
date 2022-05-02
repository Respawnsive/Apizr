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

        public IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> GetCrudManagerFor<T>() where T : class
            => GetManagerFor<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>();

        public IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> GetCrudManagerFor<T, TKey>()
            where T : class
            => GetManagerFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>();

        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> GetCrudManagerFor<T, TKey, TReadAllResult>() 
            where T : class
            => GetManagerFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>();

        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> GetCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>() 
            where T : class
            => GetManagerFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>();

        public IApizrManager<TWebApi> GetManagerFor<TWebApi>()
            => (IApizrManager<TWebApi>)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        public bool TryGetCrudManagerFor<T>(out IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> manager) where T : class
            => TryGetManagerFor<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>(out manager);

        public bool TryGetCrudManagerFor<T, TKey>(out IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> manager) 
            where T : class
            => TryGetManagerFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>(out manager);

        public bool TryGetCrudManagerFor<T, TKey, TReadAllResult>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> manager) where T : class
            => TryGetManagerFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>(out manager);

        public bool TryGetCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> manager) where T : class
            => TryGetManagerFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>(out manager);

        public bool TryGetManagerFor<TWebApi>(out IApizrManager<TWebApi> manager)
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

        public bool ContainsCrudManagerFor<T>() where T : class
            => ContainsManagerFor<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>();

        public bool ContainsCrudManagerFor<T, TKey>() where T : class
            => ContainsManagerFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>();

        public bool ContainsCrudManagerFor<T, TKey, TReadAllResult>() where T : class
            => ContainsManagerFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>();

        public bool ContainsCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ContainsManagerFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>();

        public bool ContainsManagerFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
