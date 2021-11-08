using System;

namespace Apizr.Configuring.Registry
{
    interface IApizrConcurrentRegistry : IApizrRegistry
    {
        void AddOrUpdateFor<TWebApi>(Func<IApizrManager<TWebApi>> managerFactory);
    }
}
