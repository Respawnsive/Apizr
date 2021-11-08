using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Configuring.Registry
{
    public interface IApizrEnumerableRegistryBase
    {
        int Count { get; }

        bool ContainsFor<T, TKey>() where T : class;

        bool ContainsFor<T, TKey, TReadAllResult>() where T : class;

        bool ContainsFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class;

        bool ContainsFor<TWebApi>();
    }
}
