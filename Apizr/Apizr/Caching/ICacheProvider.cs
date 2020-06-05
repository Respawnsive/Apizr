using System;
using System.Threading.Tasks;

namespace Apizr.Caching
{
    public interface ICacheProvider
    {
        Task Set(string key, object obj, TimeSpan? timeSpan = null);
        Task<T> Get<T>(string key);
        Task<bool> Remove(string key);
        Task Clear();
    }
}
