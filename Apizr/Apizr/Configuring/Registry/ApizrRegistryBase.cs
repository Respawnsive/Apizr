using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Apizr.Configuring.Registry
{
    public abstract class ApizrRegistryBase : IApizrRegistryBase
    {
        protected readonly IDictionary<Type, Func<IApizrManager>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IApizrManager>>();

        protected ApizrRegistryBase()
        {
            
        }

        public IEnumerator<KeyValuePair<Type, Func<IApizrManager>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IApizrManager<TWebApi> GetFor<TWebApi>() => GetFor<TWebApi, IApizrManager<TWebApi>>();

        public TApizrManager GetFor<TWebApi, TApizrManager>() where TApizrManager : IApizrManager<TWebApi>
            => (TApizrManager)ConcurrentRegistry[typeof(TWebApi)].Invoke();

        public bool TryGetFor<TWebApi>(out IApizrManager<TWebApi> manager)
            => TryGetFor<TWebApi, IApizrManager<TWebApi>>(out manager);

        public bool TryGetFor<TWebApi, TApizrManager>(out TApizrManager manager) where TApizrManager : IApizrManager<TWebApi>
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(TWebApi), out var managerFactory))
            {
                manager = default;
                return false;
            }

            manager = (TApizrManager)managerFactory.Invoke();
            return true;
        }

        public int Count => ConcurrentRegistry.Count;

        public bool ContainsFor<TWebApi>()
            => ConcurrentRegistry.ContainsKey(typeof(TWebApi));
    }
}
