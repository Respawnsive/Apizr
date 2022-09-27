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

        internal IApizrExtendedRegistry GetInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return this;
        }

        /// <inheritdoc />
        public void AddOrUpdate(Type webApiType, Type serviceType)
        {
            var registry = ThrowIfNotConcurrentImplementation();
            Func<IApizrManager> managerFactory = () => _serviceProvider.GetRequiredService(serviceType) as IApizrManager;
            registry.AddOrUpdate(webApiType, k => managerFactory, (k, e) => managerFactory);
        }
    }
}
