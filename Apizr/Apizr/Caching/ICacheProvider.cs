using System;
using System.Threading.Tasks;

namespace Apizr.Caching
{
    public interface ICacheProvider
    {
        Task Set(string key, object value, TimeSpan? lifeSpan = null);
        Task<T> Get<T>(string key);
        Task<bool> Remove(string key);
        Task Clear();
    }
}
