using System;
using Apizr.Configuring.Common;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistry : IApizrRegistryBase
    {
        void Populate(Action<Type, Func<object>> populateAction);
    }
}
