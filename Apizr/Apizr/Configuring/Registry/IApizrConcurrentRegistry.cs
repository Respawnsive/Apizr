using System;

namespace Apizr.Configuring.Registry
{
    interface IApizrConcurrentRegistry : IApizrRegistry
    {
        void AddOrUpdateManagerFor<TWebApi>(Func<IApizrManager<TWebApi>> managerFactory);
    }
}
