using System;

namespace Apizr.Extending.Configuring.Registry
{
    public interface IApizrExtendedConcurrentRegistryBase
    {
        void AddOrUpdate(Type webApiType, Type serviceType);
    }
}
