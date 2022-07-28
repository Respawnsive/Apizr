using System;
using Apizr.Configuring.Common;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistry : IApizrEnumerableRegistry
    { 
        internal IApizrRegistry SubRegistry { get; set; }

        void Populate(Action<Type, Func<object>> populateAction);
    }
}
