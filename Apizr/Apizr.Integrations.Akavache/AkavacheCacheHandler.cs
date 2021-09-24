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
        private readonly IBlobCache _blobCache;
        private static readonly IBlobCache DefaultBlobCache = BlobCache.LocalMachine;
        private static readonly string DefaultApplicationName = $"{nameof(Apizr)}{nameof(AkavacheCacheHandler)}";

        public AkavacheCacheHandler() : this(DefaultBlobCache, DefaultApplicationName)
        {
        }

        public AkavacheCacheHandler(IBlobCache blobCache) : this(blobCache, DefaultApplicationName)
        {
        }

        public AkavacheCacheHandler(string applicationName) : this(DefaultBlobCache, applicationName)
        {
            
        }

        public AkavacheCacheHandler(IBlobCache blobCache, string applicationName)
        {
            _blobCache = blobCache;
            Registrations.Start(applicationName);
        }

        public Task SetAsync(string key, object value, TimeSpan? lifeSpan = null,
            CancellationToken cancellationToken = default)
        {
            return lifeSpan.HasValue
                ? _blobCache.InsertObject(key, value, lifeSpan.Value).ToTask(cancellationToken)
                : _blobCache.InsertObject(key, value).ToTask(cancellationToken);
        }

        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            return _blobCache.GetObject<T>(key)
                .Catch(Observable.Return(default(T)))
                .ToTask(cancellationToken);
        }

        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return _blobCache.Invalidate(key)
                .SelectMany(_ => Observable.Return(true))
                .Catch(Observable.Return(false))
                .ToTask(cancellationToken);
        }

        public Task ClearAsync(CancellationToken cancellationToken = default)
        {
            return _blobCache.InvalidateAll()
                .SelectMany(_ => Observable.Return(true))
                .Catch(Observable.Return(false))
                .ToTask(cancellationToken);
        }
    }
}
