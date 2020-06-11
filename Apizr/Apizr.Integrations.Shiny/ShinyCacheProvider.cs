using System;
using System.Threading.Tasks;
using Apizr.Caching;
using Shiny.Caching;

namespace Apizr
{
    public class ShinyCacheProvider : ICacheProvider
    {
        private readonly ICache _shinyCache;

        public ShinyCacheProvider(ICache shinyCache)
        {
            _shinyCache = shinyCache;
        }

        public Task Set(string key, object value, TimeSpan? lifeSpan = null) => _shinyCache.Set(key, value, lifeSpan);

        public Task<T> Get<T>(string key) => _shinyCache.Get<T>(key);

        public Task<bool> Remove(string key) => _shinyCache.Remove(key);

        public Task Clear() => _shinyCache.Clear();
    }
}
