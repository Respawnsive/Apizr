﻿using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Akavache;
using Apizr.Caching;

namespace Apizr
{
    public class AkavacheCacheProvider : ICacheProvider
    {
        public AkavacheCacheProvider()
        {
            Registrations.Start($"{nameof(Apizr)}{nameof(AkavacheCacheProvider)}");
        }

        public Task Set(string key, object value, TimeSpan? lifeSpan = null,
            CancellationToken cancellationToken = default)
        {
            return lifeSpan.HasValue
                ? BlobCache.LocalMachine.InsertObject(key, value, lifeSpan.Value).ToTask(cancellationToken)
                : BlobCache.LocalMachine.InsertObject(key, value).ToTask(cancellationToken);
        }

        public Task<T> Get<T>(string key, CancellationToken cancellationToken = default)
        {
            return BlobCache.LocalMachine.GetObject<T>(key)
                .Catch(Observable.Return(default(T)))
                .ToTask(cancellationToken);
        }

        public Task<bool> Remove(string key, CancellationToken cancellationToken = default)
        {
            return BlobCache.LocalMachine.Invalidate(key)
                .SelectMany(_ => Observable.Return(true))
                .Catch(Observable.Return(false))
                .ToTask(cancellationToken);
        }

        public Task Clear(CancellationToken cancellationToken = default)
        {
            return BlobCache.LocalMachine.InvalidateAll().ToTask(cancellationToken);
        }
    }
}
