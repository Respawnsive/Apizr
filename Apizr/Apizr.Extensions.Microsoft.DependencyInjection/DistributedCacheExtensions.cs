using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Apizr
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetAsync<TCache>(this IDistributedCache distributedCache, string key,
            object value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default)
        {
            if (typeof(TCache) == typeof(byte[]))
            {
                await distributedCache.SetAsync(key, value.ToByteArray(), options, cancellationToken);
            }
            else if (typeof(TCache) == typeof(string))
            {
                await distributedCache.SetStringAsync(key, value.ToByteArray(), options, cancellationToken);
            }
            else
            {
                throw new ArgumentException($"{nameof(TCache)} must be either {nameof(String)} or {nameof(Byte)}[]", nameof(TCache));
            }
        }

        public static async Task<TData> GetAsync<TCache, TData>(this IDistributedCache distributedCache, string key, CancellationToken cancellationToken = default)
        {
            if (typeof(TCache) == typeof(byte[]))
            {
                var result = await distributedCache.GetAsync(key, cancellationToken);
                return result.FromByteArray<TData>();
            }
            else if (typeof(TCache) == typeof(string))
            {
                var result = await distributedCache.GetStringAsync(key, cancellationToken);
                return result.FromByteArray<TData>();
            }
            else
            {
                throw new ArgumentException($"{nameof(TCache)} must be either {nameof(String)} or {nameof(Byte)}[]", nameof(TCache));
            }
        }
    }
}
