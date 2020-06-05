using System;
using System.Threading.Tasks;

namespace Apizr.Caching
{
    internal class VoidCacheProvider : ICacheProvider
    {
        public Task<T> Get<T>(string key) => Task.FromResult(default(T));
        public Task Set(string key, object obj, TimeSpan? timeSpan = null) => Task.CompletedTask;
        public Task Clear() => Task.CompletedTask;
        public Task<bool> Remove(string key) => Task.FromResult(false);
    }
}