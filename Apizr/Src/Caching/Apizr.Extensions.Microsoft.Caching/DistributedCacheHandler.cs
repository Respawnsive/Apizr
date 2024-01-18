using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Refit;

namespace Apizr
{
    /// <summary>
    /// Distributed cache handler implementation
    /// </summary>
    /// <typeparam name="TCache"></typeparam>
    public class DistributedCacheHandler<TCache> : ICacheHandler
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpContentSerializer _contentSerializer;

        public DistributedCacheHandler(IDistributedCache distributedCache, IHttpContentSerializer contentSerializer)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache), $"You must register an implementation of {nameof(IDistributedCache)} before trying to use it");
            _contentSerializer = contentSerializer;
        }

        /// <inheritdoc />
        public async Task SetAsync(string key, object value, TimeSpan? lifeSpan = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = lifeSpan };

            if (typeof(TCache) == typeof(byte[]))
            {
                var data = await value.ToByteArrayAsync();
                await _distributedCache.SetAsync(key, data, options, cancellationToken);
            }
            else if (typeof(TCache) == typeof(string))
            {
                var data = await value.ToJsonStringAsync(_contentSerializer);
                await _distributedCache.SetStringAsync(key, data, options, cancellationToken);
            }
            else
            {
                throw new ArgumentException($"{nameof(TCache)} must be either {nameof(String)} or {nameof(Byte)}[]", nameof(TCache));
            }
        }

        /// <inheritdoc />
        public async Task<TData> GetAsync<TData>(string key, CancellationToken cancellationToken = default)
        {
            if (typeof(TCache) == typeof(byte[]))
            {
                var result = await _distributedCache.GetAsync(key, cancellationToken);
                return await result.FromByteArrayAsync<TData>();
            }
            else if (typeof(TCache) == typeof(string))
            {
                var result = await _distributedCache.GetStringAsync(key, cancellationToken);
                return await result.FromJsonStringAsync<TData>(_contentSerializer, cancellationToken);
            }
            else
            {
                throw new ArgumentException($"{nameof(TCache)} must be either {nameof(String)} or {nameof(Byte)}[]", nameof(TCache));
            }
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _distributedCache.RemoveAsync(key, cancellationToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public Task ClearAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Clearing feature is not available with IDistributedCache");
        }
    }
}
