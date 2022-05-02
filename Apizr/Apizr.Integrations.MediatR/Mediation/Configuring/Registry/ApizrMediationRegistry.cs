using System;
using System.Collections.Concurrent;
using Apizr.Mediation.Requesting.Sending;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Mediation.Configuring.Registry
{
    public class ApizrMediationRegistry : ApizrMediationRegistryBase, IApizrMediationConcurrentRegistry
    {
        private IServiceProvider _serviceProvider;

        private ConcurrentDictionary<Type, Func<IApizrMediatorBase>> ThrowIfNotConcurrentImplementation()
        {
            if (ConcurrentRegistry is ConcurrentDictionary<Type, Func<IApizrMediatorBase>> concurrentRegistry)
            {
                return concurrentRegistry;
            }

            throw new InvalidOperationException($"This {nameof(ApizrMediationRegistry)} is not configured for concurrent operations.");
        }

        internal IApizrMediationRegistry GetInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return this;
        }

        public void AddOrUpdate(Type webApiType, Type serviceType)
        {
            var registry = ThrowIfNotConcurrentImplementation();
            Func<IApizrMediatorBase> mediatorFactory = () => _serviceProvider.GetRequiredService(serviceType) as IApizrMediatorBase;
            registry.AddOrUpdate(webApiType, k => mediatorFactory, (k, e) => mediatorFactory);
        }
    }
}
