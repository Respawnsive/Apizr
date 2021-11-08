using System;
using System.Collections.Concurrent;
using Apizr.Mediation.Requesting.Sending;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Mediation.Configuring.Registry
{
    public class ApizrMediationRegistry : ApizrMediationRegistryBase, IApizrMediationConcurrentRegistry
    {
        private IServiceProvider _serviceProvider;

        private ConcurrentDictionary<Type, Func<IApizrMediator>> ThrowIfNotConcurrentImplementation()
        {
            if (ConcurrentRegistry is ConcurrentDictionary<Type, Func<IApizrMediator>> concurrentRegistry)
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

        public void AddOrUpdateFor(Type webApiType, Type mediatorType)
        {
            var registry = ThrowIfNotConcurrentImplementation();
            Func<IApizrMediator> mediatorFactory = () => _serviceProvider.GetRequiredService(mediatorType) as IApizrMediator;
            registry.AddOrUpdate(webApiType, k => mediatorFactory, (k, e) => mediatorFactory);
        }
    }
}
