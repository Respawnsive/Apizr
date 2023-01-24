using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Configuring.Registry
{
    internal interface IApizrInternalEnumerableRegistry
    {
        TApizrManager GetManagerInternal<TApizrManager>() where TApizrManager : IApizrManager;
        bool TryGetManagerInternal<TApizrManager>(out TApizrManager manager) where TApizrManager : IApizrManager;
        bool ContainsManagerInternal<TApizrManager>() where TApizrManager : IApizrManager;
    }
}
