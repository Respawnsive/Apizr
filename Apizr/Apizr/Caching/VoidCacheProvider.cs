using System;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr.Caching
{
    public class VoidCacheProvider : ICacheProvider
    {
        public Task<T> Get<T>(string key, CancellationToken cancellationToken = default) => Task.FromResult(default(T));
        public Task Set(string key, object obj, TimeSpan? timeSpan = null, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task Clear(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task<bool> Remove(string key, CancellationToken cancellationToken = default) => Task.FromResult(false);
    }
}