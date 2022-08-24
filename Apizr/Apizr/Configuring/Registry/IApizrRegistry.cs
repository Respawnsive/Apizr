using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Configuring.Common;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistry : IApizrEnumerableRegistry
    { 
        internal void Import(IApizrRegistry registry);
        void Populate(Action<Type, Func<object>> populateAction);
    }
}
