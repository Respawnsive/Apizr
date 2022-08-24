using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Apizr.Configuring.Registry
{
    public class ApizrRegistry : ApizrRegistryBase, IApizrConcurrentRegistry
    {
        ConcurrentDictionary<Type, Func<IApizrManager>> IApizrConcurrentRegistry.ThrowIfNotConcurrentImplementation()
        {
            if (ConcurrentRegistry is ConcurrentDictionary<Type, Func<IApizrManager>> concurrentRegistry)
            {
                return concurrentRegistry;
            }

            throw new InvalidOperationException($"This {nameof(ApizrRegistryBase)} is not configured for concurrent operations.");
        }

        void IApizrRegistry.Import(IApizrRegistry registry)
        {
            if (registry is not IApizrConcurrentRegistry registryToImportFrom)
                throw new InvalidOperationException($"Registry must implement {nameof(IApizrConcurrentRegistry)} interface");

            ((IApizrConcurrentRegistry)this).ImportFrom(registryToImportFrom);
        }

        void IApizrConcurrentRegistry.ImportFrom(IApizrConcurrentRegistry registryToImportFrom)
        {
            registryToImportFrom.ExportTo(this);
        }

        void IApizrConcurrentRegistry.ExportTo(IApizrConcurrentRegistry registryToExportTo)
        {
            var registryToImportFrom = ((IApizrConcurrentRegistry)this).ThrowIfNotConcurrentImplementation();
            foreach (var webApi in registryToImportFrom)
            {
                registryToExportTo.AddOrUpdateManagerFor(webApi.Key, webApi.Value);
            }
            registryToImportFrom.Clear();
        }

        public void AddOrUpdateManagerFor<TWebApi>(Func<IApizrManager<TWebApi>> managerFactory)
            => AddOrUpdateManagerFor(typeof(TWebApi), managerFactory);

        public void AddOrUpdateManagerFor(Type webApiType, Func<IApizrManager> managerFactory)
        {
            var registry = ((IApizrConcurrentRegistry)this).ThrowIfNotConcurrentImplementation();

            registry.AddOrUpdate(webApiType, k => managerFactory, (k, e) => managerFactory);
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
