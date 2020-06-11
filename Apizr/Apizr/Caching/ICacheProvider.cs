using System;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr.Caching
{
    public interface ICacheProvider
    {
        Task Set(string key, object value, TimeSpan? lifeSpan = null, CancellationToken cancellationToken = default);
        Task<T> Get<T>(string key, CancellationToken cancellationToken = default);
        Task<bool> Remove(string key, CancellationToken cancellationToken = default);
        Task Clear(CancellationToken cancellationToken = default);
    }
}
