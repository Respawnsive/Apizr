using System.Collections.Generic;
using Apizr.Configuring.Registry;

namespace Apizr.Extending.Configuring.Registry
{
    public interface IApizrExtendedRegistry : IApizrEnumerableRegistry
    {
        internal void Import(IApizrExtendedRegistry registry);
    }
}
