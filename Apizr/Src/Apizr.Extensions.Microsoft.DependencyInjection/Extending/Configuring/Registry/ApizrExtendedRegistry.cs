using System;
using System.Collections.Concurrent;
using Apizr.Configuring.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Extending.Configuring.Registry
{
    /// <summary>
    /// Registry options available for extended registrations
    /// </summary>
    public class ApizrExtendedRegistry : ApizrRegistryBase, IApizrExtendedConcurrentRegistry
    {
        private IServiceProvider _serviceProvider;

        private ConcurrentDictionary<Type, Func<IApizrManager>> ThrowIfNotConcurrentImplementation()
        {
            if (ConcurrentRegistry is ConcurrentDictionary<Type, Func<IApizrManager>> concurrentRegistry)
            {
                return concurrentRegistry;
            }

            throw new InvalidOperationException($"This {nameof(ApizrExtendedRegistry)} is not configured for concurrent operations.");
        }

        IApizrExtendedRegistry IApizrExtendedRegistry.GetInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return this;
        }

        void IApizrExtendedRegistry.MergeTo(IApizrExtendedRegistry targetRegistry)
        {
            foreach (var entry in ConcurrentRegistry) 
                targetRegistry.TryAddManager(entry.Key, entry.Value);
        }

        bool IApizrExtendedRegistry.TryAddManager(Type managerType, Func<IApizrManager> managerFactory)
        {
            var registry = ThrowIfNotConcurrentImplementation();
            return registry.TryAdd(managerType, managerFactory);
        }

        /// <inheritdoc />
        public void AddOrUpdateManager(Type managerType)
        {
            var registry = ThrowIfNotConcurrentImplementation();
            Func<IApizrManager> managerFactory = () => _serviceProvider.GetRequiredService(managerType) as IApizrManager;
            registry.AddOrUpdate(managerType, k => managerFactory, (k, e) => managerFactory);
        }
    }
}
