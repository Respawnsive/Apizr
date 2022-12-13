using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Request;
using Apizr.Policing;
using Polly;
using Refit;

namespace Apizr.Requesting
{
    /// <summary>
    /// The crud api interface
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    /// <typeparam name="TReadAllResult">The "ReadAll" query result type</typeparam>
    /// <typeparam name="TReadAllParams">ReadAll query parameters type</typeparam>
    public interface ICrudApi<T, in TKey, TReadAllResult, in TReadAllParams> where T : class
    {

        #region Create

        #region Obsolete

        /// <summary>
        /// Send a Create request with a <typeparamref name="T"/> payload, passing a Polly context through the request
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Post("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<T> Create([Body] T payload, [Context] Context context);

        /// <summary>
        /// Send a Create request with a <typeparamref name="T"/> payload, passing a Polly context and a cancellation token through the request
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Post("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<T> Create([Body] T payload, [Context] Context context, CancellationToken cancellationToken);

        #endregion

        /// <summary>
        /// Send a Create request with a <typeparamref name="T"/> payload
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <returns></returns>
        [Post("")]
        Task<T> Create([Body] T payload);

        /// <summary>
        /// Send a Create request with a <typeparamref name="T"/> payload, passing a cancellation token through the request
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Post("")]
        Task<T> Create([Body] T payload, CancellationToken cancellationToken);

        /// <summary>
        /// Send a Create request with a <typeparamref name="T"/> payload, passing a Polly context and a cancellation token through the request
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Post("")]
        Task<T> Create([Body] T payload, [RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a Create request with a <typeparamref name="T"/> payload, passing a Polly context and a cancellation token through the request
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <param name="options">The request options</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Post("")]
        Task<T> Create([Body] T payload, [RequestOptions] IApizrRequestOptions options, CancellationToken cancellationToken);

        #endregion

        #region ReadAll

        #region Obsolete
        
        /// <summary>
        /// Send a ReadAll request with an execution priority level
        /// </summary>
        /// <param name="priority">The execution priority level</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([Property(Constants.PriorityKey)] int priority);

        /// <summary>
        /// Send a ReadAll request, passing a Polly context through the request
        /// </summary>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([Context] Context context);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key and an execution priority level
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="priority">The execution priority level</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key and passing a Polly context through the request
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Context] Context context);

        /// <summary>
        /// Send a ReadAll request with an execution priority level and passing a Polly context through the request
        /// </summary>
        /// <param name="priority">The execution priority level</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([Property(Constants.PriorityKey)] int priority, [Context] Context context);

        /// <summary>
        /// Send a ReadAll request with an execution priority level, passing a cancellation token through the request
        /// </summary>
        /// <param name="priority">The execution priority level</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([Property(Constants.PriorityKey)] int priority, CancellationToken cancellationToken);

        /// <summary>
        /// Send a ReadAll request, passing a Polly context and a cancellation token through the request
        /// </summary>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([Context] Context context, CancellationToken cancellationToken);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key and an execution priority level, passing a Polly context through the request
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="priority">The execution priority level</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority, [Context] Context context);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key and an execution priority level, passing a cancellation token through the request
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="priority">The execution priority level</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority, CancellationToken cancellationToken);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key, passing a Polly context and a cancellation token through the request
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Context] Context context, CancellationToken cancellationToken);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key and an execution priority level, passing a Polly context and a cancellation token through the request
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="priority">The execution priority level</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [Property(Constants.PriorityKey)] int priority, [Context] Context context, CancellationToken cancellationToken);
        
        #endregion

        /// <summary>
        /// Send a ReadAll request
        /// </summary>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll();

        /// <summary>
        /// Send a ReadAll request
        /// </summary>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll([RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams);

        /// <summary>
        /// Send a ReadAll request, passing a cancellation token through the request
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll(CancellationToken cancellationToken);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key, passing a cancellation token through the request
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, CancellationToken cancellationToken);

        /// <summary>
        /// Send a ReadAll request, passing a cancellation token through the request
        /// </summary>
        /// <param name="options">The request options</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll([RequestOptions] IApizrRequestOptions options, CancellationToken cancellationToken);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="options">The request options</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [RequestOptions] IApizrRequestOptions options, CancellationToken cancellationToken);

        #endregion

        #region Read

        #region Obsolete
        
        /// <summary>
        /// Send a Read request with a key param and an execution priority level
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="priority">The execution priority level</param>
        /// <returns></returns>
        [Get("/{key}")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority);

        /// <summary>
        /// Send a Read request with a key param, passing a Polly context through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Get("/{key}")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<T> Read([CacheKey] TKey key, [Context] Context context);
        
        /// <summary>
        /// Send a Read request with a key param and an execution priority level, passing a Polly context through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="priority">The execution priority level</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Get("/{key}")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority, [Context] Context context);

        /// <summary>
        /// Send a Read request with a key param and an execution priority level, passing a cancellation token through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="priority">The execution priority level</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("/{key}")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority, CancellationToken cancellationToken);

        /// <summary>
        /// Send a Read request with a key param and an execution priority level, passing a Polly context and a cancellation token through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="priority">The execution priority level</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("/{key}")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task<T> Read([CacheKey] TKey key, [Property(Constants.PriorityKey)] int priority, [Context] Context context, CancellationToken cancellationToken);
        
        #endregion

        /// <summary>
        /// Send a Read request with a key param
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key);

        /// <summary>
        /// Send a Read request with a key param
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a Read request with a key param, passing a cancellation token through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, CancellationToken cancellationToken);

        /// <summary>
        /// Send a Read request with a key param
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="options">The request options</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [RequestOptions] IApizrRequestOptions options, CancellationToken cancellationToken);

        #endregion

        #region Update

        #region Obsolete
        
        /// <summary>
        /// Send an Update request with a key and a payload, passing a Polly context through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="payload">The payload</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Put("/{key}")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task Update(TKey key, [Body] T payload, [Context] Context context);
        
        /// <summary>
        /// Send an Update request with a key and a payload, passing a Polly context and a cancellation token through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="payload">The payload</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Put("/{key}")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task Update(TKey key, [Body] T payload, [Context] Context context, CancellationToken cancellationToken);

        #endregion

        /// <summary>
        /// Send an Update request with a key and a payload
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="payload">The payload</param>
        /// <returns></returns>
        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload);

        /// <summary>
        /// Send an Update request with a key and a payload
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="payload">The payload</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload, [RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send an Update request with a key and a payload, passing a cancellation token through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="payload">The payload</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload, CancellationToken cancellationToken);

        /// <summary>
        /// Send an Update request with a key and a payload, passing a cancellation token through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="payload">The payload</param>
        /// <param name="options">The request options</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Put("/{key}")]
        Task Update(TKey key, [Body] T payload, [RequestOptions] IApizrRequestOptions options, CancellationToken cancellationToken);

        #endregion

        #region Delete

        #region Obsolete
        
        /// <summary>
        /// Send a Delete request with a key param, passing a Polly context through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="context">The Polly context</param>
        /// <returns></returns>
        [Delete("/{key}")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task Delete(TKey key, [Context] Context context);

        /// <summary>
        /// Send a Delete request with a key param, passing a Polly context and a cancellation token through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="context">The Polly context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Delete("/{key}")]
        [Obsolete("Use the one with the request options parameter instead")]
        Task Delete(TKey key, [Context] Context context, CancellationToken cancellationToken);

        #endregion

        /// <summary>
        /// Send a Delete request with a key param
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        [Delete("/{key}")]
        Task Delete(TKey key);

        /// <summary>
        /// Send a Delete request with a key param
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Delete("/{key}")]
        Task Delete(TKey key, [RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a Delete request with a key param, passing a cancellation token through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Delete("/{key}")]
        Task Delete(TKey key, CancellationToken cancellationToken);

        /// <summary>
        /// Send a Delete request with a key param, passing a cancellation token through the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="options">The request options</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [Delete("/{key}")]
        Task Delete(TKey key, [RequestOptions] IApizrRequestOptions options, CancellationToken cancellationToken);

        #endregion
    }
}
