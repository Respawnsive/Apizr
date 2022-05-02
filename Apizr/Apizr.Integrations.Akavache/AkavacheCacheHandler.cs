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

        /// <summary>
        /// Set Akavache as CacheHandler with LocalMachine blob cache and ApizrAkavacheCacheHandler name
        /// </summary>
        public AkavacheCacheHandler() : this(null, null)
        {
        }

        /// <summary>
        /// Set Akavache as CacheHandler with your blob cache and ApizrAkavacheCacheHandler name
        /// </summary>
        /// <param name="blobCacheFactory">The blob cache factory of your choice</param>
        public AkavacheCacheHandler(Func<IBlobCache> blobCacheFactory) : this(blobCacheFactory, null)
        {
        }

        /// <summary>
        /// Set Akavache as CacheHandler with LocalMachine blob cache and your provided name
        /// </summary>
        /// <param name="applicationName">The application name used by Akavache</param>
        public AkavacheCacheHandler(string applicationName) : this(null, applicationName)
        {
            
        }

        /// <summary>
        /// Set Akavache as CacheHandler with your blob cache and your provided name
        /// </summary>
        /// <param name="blobCacheFactory">The blob cache factory of your choice</param>
        /// <param name="applicationName">The application name used by Akavache</param>
        public AkavacheCacheHandler(Func<IBlobCache> blobCacheFactory, string applicationName)
        {
            Registrations.Start(applicationName ?? $"{nameof(ApizrBuilder)}{nameof(AkavacheCacheHandler)}");
            _blobCache = blobCacheFactory?.Invoke() ?? BlobCache.LocalMachine;
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
