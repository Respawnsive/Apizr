using System.Threading.Tasks;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Request;
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

        /// <summary>
        /// Send a Create request with a <typeparamref name="T"/> payload
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <returns></returns>
        [Post("")]
        Task<T> Create([Body] T payload);

        /// <summary>
        /// Send a Create request with a <typeparamref name="T"/> payload
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <returns></returns>
        [Post("")]
        Task<IApiResponse<T>> SafeCreate([Body] T payload);

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
        /// <returns></returns>
        [Post("")]
        Task<IApiResponse<T>> SafeCreate([Body] T payload, [RequestOptions] IApizrRequestOptions options);

        #endregion

        #region ReadAll

        /// <summary>
        /// Send a ReadAll request
        /// </summary>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll();

        /// <summary>
        /// Send a ReadAll request
        /// </summary>
        /// <returns></returns>
        [Get("")]
        Task<IApiResponse<TReadAllResult>> SafeReadAll();

        /// <summary>
        /// Send a ReadAll request
        /// </summary>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll([RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a ReadAll request
        /// </summary>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Get("")]
        Task<IApiResponse<TReadAllResult>> SafeReadAll([RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <returns></returns>
        [Get("")]
        Task<IApiResponse<TReadAllResult>> SafeReadAll([CacheKey] TReadAllParams readAllParams);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Get("")]
        Task<TReadAllResult> ReadAll([CacheKey] TReadAllParams readAllParams, [RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a ReadAll request with some query params used as cache key
        /// </summary>
        /// <param name="readAllParams">Query params used as cache key</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Get("")]
        Task<IApiResponse<TReadAllResult>> SafeReadAll([CacheKey] TReadAllParams readAllParams, [RequestOptions] IApizrRequestOptions options);

        #endregion

        #region Read

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
        /// <returns></returns>
        [Get("/{key}")]
        Task<IApiResponse<T>> SafeRead([CacheKey] TKey key);

        /// <summary>
        /// Send a Read request with a key param
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Get("/{key}")]
        Task<T> Read([CacheKey] TKey key, [RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a Read request with a key param
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Get("/{key}")]
        Task<IApiResponse<T>> SafeRead([CacheKey] TKey key, [RequestOptions] IApizrRequestOptions options);

        #endregion

        #region Update

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
        /// <returns></returns>
        [Put("/{key}")]
        Task<IApiResponse> SafeUpdate(TKey key, [Body] T payload);

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
        /// Send an Update request with a key and a payload
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="payload">The payload</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Put("/{key}")]
        Task<IApiResponse> SafeUpdate(TKey key, [Body] T payload, [RequestOptions] IApizrRequestOptions options);

        #endregion

        #region Delete

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
        /// <returns></returns>
        [Delete("/{key}")]
        Task<IApiResponse> SafeDelete(TKey key);

        /// <summary>
        /// Send a Delete request with a key param
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Delete("/{key}")]
        Task Delete(TKey key, [RequestOptions] IApizrRequestOptions options);

        /// <summary>
        /// Send a Delete request with a key param
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="options">The request options</param>
        /// <returns></returns>
        [Delete("/{key}")]
        Task<IApiResponse> SafeDelete(TKey key, [RequestOptions] IApizrRequestOptions options);

        #endregion
    }
}
