using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Configuring.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Extending.Configuring.Registry
{
    public class ApizrExtendedRegistry : ApizrRegistryBase, IApizrExtendedConcurrentRegistry
    {
        private IServiceProvider _serviceProvider;

        ConcurrentDictionary<Type, Func<IApizrManager>> IApizrExtendedConcurrentRegistry.ThrowIfNotConcurrentImplementation()
        {
            if (ConcurrentRegistry is ConcurrentDictionary<Type, Func<IApizrManager>> concurrentRegistry)
            {
                return concurrentRegistry;
            }

            throw new InvalidOperationException($"This {nameof(ApizrExtendedRegistry)} is not configured for concurrent operations.");
        }

        void IApizrExtendedRegistry.Import(IApizrExtendedRegistry registry)
        {
            if (registry is not IApizrExtendedConcurrentRegistry registryToImportFrom)
                throw new InvalidOperationException($"Registry must implement {nameof(IApizrExtendedConcurrentRegistry)} interface");

            ((IApizrExtendedConcurrentRegistry)this).ImportFrom(registryToImportFrom);
        }

        void IApizrExtendedConcurrentRegistry.ImportFrom(IApizrExtendedConcurrentRegistry registryToImportFrom)
        {
            registryToImportFrom.ExportTo(this);
        }

        void IApizrExtendedConcurrentRegistry.ExportTo(IApizrExtendedConcurrentRegistry registryToExportTo)
        {
            var concurrentRegistryToImportFrom = ((IApizrExtendedConcurrentRegistry)this).ThrowIfNotConcurrentImplementation();
            var concurrentRegistryToExportTo = registryToExportTo.ThrowIfNotConcurrentImplementation();
            foreach (var webApi in concurrentRegistryToImportFrom)
            {
                concurrentRegistryToExportTo.AddOrUpdate(webApi.Key, k => webApi.Value, (k, e) => webApi.Value);
            }
            //concurrentRegistryToImportFrom.Clear();
        }

        internal IApizrExtendedRegistry GetInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return this;
        }

        public void AddOrUpdate(Type webApiType, Type serviceType)
        {
            var registry = ((IApizrExtendedConcurrentRegistry)this).ThrowIfNotConcurrentImplementation();
            Func<IApizrManager> managerFactory = () => _serviceProvider.GetRequiredService(serviceType) as IApizrManager;
            registry.AddOrUpdate(webApiType, k => managerFactory, (k, e) => managerFactory);
        }
    }
}
