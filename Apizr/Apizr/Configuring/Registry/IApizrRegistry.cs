using System;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistry : IApizrEnumerableRegistry
    {
        void Populate(Action<Type, Func<object>> populateAction);
    }
}
