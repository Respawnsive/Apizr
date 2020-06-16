using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Shiny.Caching;

namespace Apizr
{
    public class ShinyCacheHandler : ICacheHandler
    {
        private readonly ICache _shinyCache;

        public ShinyCacheHandler(ICache shinyCache)
        {
            _shinyCache = shinyCache;
        }

        public Task Set(string key, object value, TimeSpan? lifeSpan = null,
            CancellationToken cancellationToken = default) => _shinyCache.Set(key, value, lifeSpan);

        public Task<T> Get<T>(string key, CancellationToken cancellationToken = default) => _shinyCache.Get<T>(key);

        public Task<bool> Remove(string key, CancellationToken cancellationToken = default) => _shinyCache.Remove(key);

        public Task Clear(CancellationToken cancellationToken = default) => _shinyCache.Clear();
    }
}
