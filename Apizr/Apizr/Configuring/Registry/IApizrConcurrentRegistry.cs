using System;
using System.Collections.Concurrent;

namespace Apizr.Configuring.Registry
{
    interface IApizrConcurrentRegistry : IApizrRegistry
    {
        void AddOrUpdateManagerFor<TWebApi>(Func<IApizrManager<TWebApi>> managerFactory);
        void AddOrUpdateManagerFor(Type webApiType, Func<IApizrManager> managerFactory);
    }
}
