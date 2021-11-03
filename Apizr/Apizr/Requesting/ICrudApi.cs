using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching.Attributes;
using Apizr.Policing;
using Polly;
using Refit;

namespace Apizr.Requesting
{
    public interface ICrudApi<T, in TKey, TReadAllResult, in TReadAllParams> where T : class
    {
        #region Create

        [Post("")]
        Task<T> Create([Body] T payload);

        [Post("")]
        Task<T> Create([Body] T payload, [Context] Context context);

        [Post("")]
        Task<T> Create([Body] T payload, CancellationToken cancellationToken);

        [Post("")]
        Task<T> Create([Body] T payload, [Context] Context context, CancellationToken cancellationToken);

        #endregion

        #region ReadAll

        [Get("")]
        Task<TReadAllResult> ReadAll();

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams);

        [Get("")]
        Task<TReadAllResult> ReadAll([Property("Priority")] int priority);

        [Get("")]
        Task<TReadAllResult> ReadAll([Context] Context context);

        [Get("")]
        Task<TReadAllResult> ReadAll(CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property("Priority")] int priority);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Context] Context context);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll([Property("Priority")] int priority, [Context] Context context);

        [Get("")]
        Task<TReadAllResult> ReadAll([Property("Priority")] int priority, CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll([Context] Context context, CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property("Priority")] int priority, [Context] Context context);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property("Priority")] int priority, CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Context] Context context, CancellationToken cancellationToken);

        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property("Priority")] int priority, [Context] Context context, CancellationToken cancellationToken);

        #endregion

        #region Read

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [Property("Priority")] int priority);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [Context] Context context);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, CancellationToken cancellationToken);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [Property("Priority")] int priority, [Context] Context context);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [Property("Priority")] int priority, CancellationToken cancellationToken);

        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [Property("Priority")] int priority, [Context] Context context, CancellationToken cancellationToken);

        #endregion

        #region Update

        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload);

        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload, [Context] Context context);

        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload, CancellationToken cancellationToken);

        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload, [Context] Context context, CancellationToken cancellationToken);

        #endregion

        #region Delete

        [Delete("/{key}")]
        Task Delete(TKey key);

        [Delete("/{key}")]
        Task Delete(TKey key, [Context] Context context);

        [Delete("/{key}")]
        Task Delete(TKey key, CancellationToken cancellationToken);

        [Delete("/{key}")]
        Task Delete(TKey key, [Context] Context context, CancellationToken cancellationToken); 

        #endregion
    }
}
