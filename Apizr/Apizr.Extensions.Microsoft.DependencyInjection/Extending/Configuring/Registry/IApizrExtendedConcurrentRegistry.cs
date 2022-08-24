using System;
using System.Collections.Concurrent;

namespace Apizr.Extending.Configuring.Registry
{
    public interface IApizrExtendedConcurrentRegistry : IApizrExtendedRegistry, IApizrExtendedConcurrentRegistryBase
    {
        internal ConcurrentDictionary<Type, Func<IApizrManager>> ThrowIfNotConcurrentImplementation();
        internal void ImportFrom(IApizrExtendedConcurrentRegistry registryToImportFrom);
        internal void ExportTo(IApizrExtendedConcurrentRegistry registryToExportTo);
    }
}
