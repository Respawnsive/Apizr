using System;
using System.Collections.Concurrent;

namespace Apizr.Configuring.Registry
{
    /// <inheritdoc cref="IApizrConcurrentRegistry"/>
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

        /// <inheritdoc />
        public void AddOrUpdateManagerFor<TWebApi>(Func<IApizrManager<TWebApi>> managerFactory)
            => AddOrUpdateManagerFor(typeof(TWebApi), managerFactory);

        /// <inheritdoc />
        public void AddOrUpdateManagerFor(Type webApiType, Func<IApizrManager> managerFactory)
        {
            var registry = ThrowIfNotConcurrentImplementation();

            registry.AddOrUpdate(webApiType, k => managerFactory, (k, e) => managerFactory);
        }

        /// <inheritdoc />
        public void Populate(Action<Type, Func<object>> populateAction)
        {
            foreach (var entry in ConcurrentRegistry)
            {
                populateAction(typeof(IApizrManager<>).MakeGenericType(entry.Key), entry.Value);
            }
        }
    }
}
