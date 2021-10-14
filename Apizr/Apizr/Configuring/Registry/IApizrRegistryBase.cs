using System;
using System.Collections.Generic;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistryBase : IEnumerable<KeyValuePair<Type, Func<IApizrManager>>>
    {
        IApizrManager<TWebApi> GetFor<TWebApi>();

        TApizrManager GetFor<TWebApi, TApizrManager>() where TApizrManager : IApizrManager<TWebApi>;

        bool TryGetFor<TWebApi>(out IApizrManager<TWebApi> manager);

        bool TryGetFor<TWebApi, TApizrManager>(out TApizrManager manager) where TApizrManager : IApizrManager<TWebApi>;

        int Count { get; }

        bool ContainsFor<TWebApi>();
    }
}
