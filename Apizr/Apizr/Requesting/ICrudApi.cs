using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Policing;
using Refit;

namespace Apizr.Requesting
{
    public interface ICrudApi<T, in TKey, TReadAllResult, in TReadAllParams> where T : class
    {
        [Post("")]
        Task<T> Create([Body] T payload, CancellationToken cancellationToken = default);

        [Get("")]
        Task<TReadAllResult> ReadAll(CancellationToken cancellationToken = default);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, CancellationToken cancellationToken = default);

        [Get("")]
        Task<TReadAllResult> ReadAll([Property("Priority")] int priority, CancellationToken cancellationToken = default);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property("Priority")] int priority, CancellationToken cancellationToken = default);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, CancellationToken cancellationToken = default);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [Property("Priority")] int priority, CancellationToken cancellationToken = default);

        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload, CancellationToken cancellationToken = default);

        [Delete("/{key}")]
        Task Delete(TKey key, CancellationToken cancellationToken = default);
    }
}
