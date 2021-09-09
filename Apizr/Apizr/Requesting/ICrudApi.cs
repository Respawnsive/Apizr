using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Refit;

namespace Apizr.Requesting
{
    public interface ICrudApi<T, in TKey, TReadAllResult, in TReadAllParams> where T : class
    {
        [Post("")]
        Task<T> Create([Body] T payload);

        [Post("")]
        Task<T> Create([Body] T payload, CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll();

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams);

        [Get("")]
        Task<TReadAllResult> ReadAll([Property("Priority")] int priority);

        [Get("")]
        Task<TReadAllResult> ReadAll(CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property("Priority")] int priority);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll([Property("Priority")] int priority, CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property("Priority")] int priority, CancellationToken cancellationToken);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [Property("Priority")] int priority);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, CancellationToken cancellationToken);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [Property("Priority")] int priority, CancellationToken cancellationToken);

        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload);

        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload, CancellationToken cancellationToken);

        [Delete("/{key}")]
        Task Delete(TKey key);

        [Delete("/{key}")]
        Task Delete(TKey key, CancellationToken cancellationToken);
    }
}
