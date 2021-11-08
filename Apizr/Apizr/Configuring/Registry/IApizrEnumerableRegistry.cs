using System;
using System.Collections.Generic;
using Apizr.Requesting;

namespace Apizr.Configuring.Registry
{
    public interface IApizrEnumerableRegistry : IEnumerable<KeyValuePair<Type, Func<IApizrManager>>>, IApizrEnumerableRegistryBase
    {
        IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> GetFor<T, TKey>() where T : class;

        IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> GetFor<T, TKey,
            TReadAllResult>() where T : class;

        IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> GetFor<T, TKey, TReadAllResult,
            TReadAllParams>() where T : class;

        IApizrManager<TWebApi> GetFor<TWebApi>();

        bool TryGetFor<T, TKey>(out IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> manager) where T : class;

        bool TryGetFor<T, TKey, TReadAllResult>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> manager) where T : class;

        bool TryGetFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> manager) where T : class;

        bool TryGetFor<TWebApi>(out IApizrManager<TWebApi> manager);
    }
}
