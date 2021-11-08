using System;

namespace Apizr.Mediation.Configuring.Registry
{
    public interface IApizrMediationConcurrentRegistry : IApizrMediationRegistry
    {
        void AddOrUpdateFor(Type webApiType, Type mediatorType);
    }
}
