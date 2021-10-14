using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Configuring.Registry
{
    interface IApizrConcurrentRegistry : IApizrRegistry
    {
        void AddOrUpdateFor<TWebApi>(Func<IApizrManager<TWebApi>> managerFactory);
    }
}
