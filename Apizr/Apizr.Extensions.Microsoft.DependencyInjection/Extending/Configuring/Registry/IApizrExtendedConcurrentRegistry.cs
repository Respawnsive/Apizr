using System;
using Apizr.Configuring.Registry;

namespace Apizr.Extending.Configuring.Registry
{
    public interface IApizrExtendedConcurrentRegistry : IApizrExtendedRegistry
    {
        void AddOrUpdateFor(Type webApiType, Type managerType);
    }
}
