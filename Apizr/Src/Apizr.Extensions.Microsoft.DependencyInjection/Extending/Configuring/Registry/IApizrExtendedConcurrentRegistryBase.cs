using System;

namespace Apizr.Extending.Configuring.Registry
{
    /// <summary>
    /// Registry options available for extended registrations
    /// </summary>
    public interface IApizrExtendedConcurrentRegistryBase
    {
        void AddOrUpdateManager(Type managerType);
    }
}
