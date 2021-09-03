using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Akavache;
using Apizr.Caching;

[assembly: Apizr.Preserve]
namespace Apizr
{
    public class AkavacheCacheHandler : ICacheHandler
    {
        public AkavacheCacheHandler()
        {
            Registrations.Start($"{nameof(Apizr)}{nameof(AkavacheCacheHandler)}");
        }

        public Task SetAsync(string key, object value, TimeSpan? lifeSpan = null,
            CancellationToken cancellationToken = default)
        {
            return lifeSpan.HasValue
                ? BlobCache.LocalMachine.InsertObject(key, value, lifeSpan.Value).ToTask(cancellationToken)
                : BlobCache.LocalMachine.InsertObject(key, value).ToTask(cancellationToken);
        }

        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            return BlobCache.LocalMachine.GetObject<T>(key)
                .Catch(Observable.Return(default(T)))
                .ToTask(cancellationToken);
        }

        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return BlobCache.LocalMachine.Invalidate(key)
                .SelectMany(_ => Observable.Return(true))
                .Catch(Observable.Return(false))
                .ToTask(cancellationToken);
        }

        public Task ClearAsync(CancellationToken cancellationToken = default)
        {
            return BlobCache.LocalMachine.InvalidateAll()
                .SelectMany(_ => Observable.Return(true))
                .Catch(Observable.Return(false))
                .ToTask(cancellationToken);
        }
    }
}
