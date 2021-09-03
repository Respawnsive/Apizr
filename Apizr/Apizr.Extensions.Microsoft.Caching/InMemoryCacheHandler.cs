using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Apizr
{
    public class InMemoryCacheHandler : ICacheHandler
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheHandler(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task SetAsync(string key, object value, TimeSpan? lifeSpan = null, CancellationToken cancellationToken = default)
        {
            var options = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = lifeSpan };
            _memoryCache.Set(key, value, options);
            return Task.CompletedTask;
        }

        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (!_memoryCache.TryGetValue<T>(key, out var result))
                result = default;

            return Task.FromResult(result);
        }

        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                _memoryCache.Remove(key);
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public Task ClearAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
