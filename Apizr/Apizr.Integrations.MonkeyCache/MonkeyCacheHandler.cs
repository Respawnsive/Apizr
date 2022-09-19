using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using MonkeyCache;

[assembly: Apizr.Preserve]
namespace Apizr
{
    /// <summary>
    /// MonkeyCache cache handler implementation
    /// </summary>
    public class MonkeyCacheHandler : ICacheHandler
    {
        private readonly IBarrel _barrel;

        public MonkeyCacheHandler(IBarrel barrel)
        {
            _barrel = barrel;
        }

        /// <inheritdoc />
        public Task SetAsync(string key, object value, TimeSpan? lifeSpan = null, CancellationToken cancellationToken = default)
        {
            var maxLifeSpan = DateTime.MaxValue - DateTime.Now;
            _barrel.Add(key, value, lifeSpan ?? maxLifeSpan);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_barrel.Get<T>(key));
        }

        /// <inheritdoc />
        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                _barrel.Empty(key);

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        /// <inheritdoc />
        public Task ClearAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _barrel.EmptyAll();

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}
