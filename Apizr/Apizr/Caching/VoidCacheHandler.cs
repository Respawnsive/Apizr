using System;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr.Caching
{
    public class VoidCacheHandler : ICacheHandler
    {
        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) => Task.FromResult(default(T));
        public Task SetAsync(string key, object obj, TimeSpan? timeSpan = null, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task ClearAsync(CancellationToken cancellationToken = default) => Task.FromResult(false);
        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default) => Task.FromResult(false);
    }
}