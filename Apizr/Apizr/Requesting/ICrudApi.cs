using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Policing;
using Refit;

namespace Apizr.Requesting
{
    [Policy("TransientHttpError"), Cache]
    public interface ICrudApi<T, in TKey, TReadAllResult, in TReadAllParams> where T : class
    {
        [Post("")]
        Task<T> Create([Body] T payload, CancellationToken cancellationToken = default);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, CancellationToken cancellationToken = default);

        [Get("")]
        Task<TReadAllResult> ReadAll(CancellationToken cancellationToken = default);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, CancellationToken cancellationToken = default);

        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload, CancellationToken cancellationToken = default);

        [Delete("/{key}")]
        Task Delete(TKey key, CancellationToken cancellationToken = default);
    }

    //public interface ICrudApi<T, in TKey, TReadAllResult> : ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>> where T : class
    //{ }

    //public interface ICrudApi<T, in TKey> : ICrudApi<T, TKey, IEnumerable<T>> where T : class
    //{ }

    //public interface ICrudApi<T> : ICrudApi<T, int> where T : class
    //{ }
}
