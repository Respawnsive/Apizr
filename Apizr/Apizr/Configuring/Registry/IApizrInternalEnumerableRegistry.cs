using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Configuring.Registry
{
    internal interface IApizrInternalEnumerableRegistry
    {
        TWrappedManager GetWrappedManager<TWrappedManager>();
        bool TryGetWrappedManager<TWrappedManager>(out TWrappedManager manager);
    }
}
