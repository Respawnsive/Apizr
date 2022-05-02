using System;
using System.Collections.Concurrent;
using Apizr.Optional.Requesting.Sending;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Optional.Configuring.Registry
{
    public class ApizrOptionalMediationRegistry : ApizrOptionalMediationRegistryBase, IApizrOptionalMediationConcurrentRegistry
    {
        private IServiceProvider _serviceProvider;

        private ConcurrentDictionary<Type, Func<IApizrOptionalMediatorBase>> ThrowIfNotConcurrentImplementation()
        {
            if (ConcurrentRegistry is ConcurrentDictionary<Type, Func<IApizrOptionalMediatorBase>> concurrentRegistry)
            {
                return concurrentRegistry;
            }

            throw new InvalidOperationException($"This {nameof(ApizrOptionalMediationRegistry)} is not configured for concurrent operations.");
        }

        internal IApizrOptionalMediationRegistry GetInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return this;
        }

        public void AddOrUpdate(Type webApiType, Type serviceType)
        {
            var registry = ThrowIfNotConcurrentImplementation();
            Func<IApizrOptionalMediatorBase> mediatorFactory = () => _serviceProvider.GetRequiredService(serviceType) as IApizrOptionalMediatorBase;
            registry.AddOrUpdate(webApiType, k => mediatorFactory, (k, e) => mediatorFactory);
        }
    }
}
