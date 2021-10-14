using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Configuring.Registry
{
    interface IApizrConcurrentRegistry : IApizrRegistry
    {
        void AddOrUpdateFor<TWebApi>(Func<IApizrManager<TWebApi>> managerFactory);

        void AddOrUpdateFor<TWebApi, TApizrManager>(Func<TApizrManager> managerFactory) where TApizrManager : IApizrManager<TWebApi>;
    }
}
