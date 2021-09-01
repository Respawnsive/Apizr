using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Apizr
{
    public class DistributedCacheHandler<TCache> : ICacheHandler
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task Set(string key, object value, TimeSpan? lifeSpan = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = lifeSpan };
            await _distributedCache.SetAsync<TCache>(key, value, options, cancellationToken);
        }

        public async Task<TData> Get<TData>(string key, CancellationToken cancellationToken = default)
        {
            return await _distributedCache.GetAsync<TCache, TData>(key, cancellationToken);
        }

        public async Task<bool> Remove(string key, CancellationToken cancellationToken = default)
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

        public Task Clear(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
