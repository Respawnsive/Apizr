using System;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr.Caching
{
    /// <inheritdoc />
    public class VoidCacheHandler : ICacheHandler
    {
        /// <inheritdoc />
        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) => Task.FromResult(default(T));

        /// <inheritdoc />
        public Task SetAsync(string key, object obj, TimeSpan? timeSpan = null, CancellationToken cancellationToken = default) => Task.CompletedTask;

        /// <inheritdoc />
        public Task ClearAsync(CancellationToken cancellationToken = default) => Task.FromResult(false);

        /// <inheritdoc />
        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default) => Task.FromResult(false);
    }
}