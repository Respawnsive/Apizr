using System;
using System.Collections.Concurrent;

namespace Apizr.Configuring.Registry
{
    interface IApizrConcurrentRegistry : IApizrRegistry
    {
        internal ConcurrentDictionary<Type, Func<IApizrManager>> ThrowIfNotConcurrentImplementation();
        void AddOrUpdateManagerFor<TWebApi>(Func<IApizrManager<TWebApi>> managerFactory);
        void AddOrUpdateManagerFor(Type webApiType, Func<IApizrManager> managerFactory);
        internal void ImportFrom(IApizrConcurrentRegistry registryToImportFrom);
        internal void ExportTo(IApizrConcurrentRegistry registryToExportTo);
    }
}
