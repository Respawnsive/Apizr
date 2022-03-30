using System;

namespace Apizr.Extending.Configuring.Registry
{
    public interface IApizrExtendedConcurrentRegistryBase
    {
        void AddOrUpdateFor(Type webApiType, Type serviceType);
    }
}
