using System;
using System.Collections.Generic;
using Apizr.Configuring.Registry;

namespace Apizr.Extending.Configuring.Registry
{
    /// <summary>
    /// Registry options available for extended registrations
    /// </summary>
    public interface IApizrExtendedRegistry : IApizrEnumerableRegistry
    {
        internal IApizrExtendedRegistry GetInstance(IServiceProvider serviceProvider);

        internal void MergeTo(IApizrExtendedRegistry targetRegistry);

        internal bool TryAddManager(Type managerType, Func<IApizrManager> managerFactory);
    }
}
