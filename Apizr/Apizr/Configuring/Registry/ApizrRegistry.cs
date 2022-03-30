using System;
using System.Collections.Concurrent;

namespace Apizr.Configuring.Registry
{
    public class ApizrRegistry : ApizrRegistryBase, IApizrConcurrentRegistry
    {
        private ConcurrentDictionary<Type, Func<IApizrManager>> ThrowIfNotConcurrentImplementation()
        {
            if (ConcurrentRegistry is ConcurrentDictionary<Type, Func<IApizrManager>> concurrentRegistry)
            {
                return concurrentRegistry;
            }

            throw new InvalidOperationException($"This {nameof(ApizrRegistryBase)} is not configured for concurrent operations.");
        }

        public void AddOrUpdateFor<TWebApi>(Func<IApizrManager<TWebApi>> managerFactory)
        {
            var registry = ThrowIfNotConcurrentImplementation();

            registry.AddOrUpdate(typeof(TWebApi), k => managerFactory, (k, e) => managerFactory);
        }

        public void Populate(Action<Type, Func<object>> populateAction)
        {
            foreach (var entry in ConcurrentRegistry)
            {
                populateAction(typeof(IApizrManager<>).MakeGenericType(entry.Key), entry.Value);
            }
        }
    }
}
