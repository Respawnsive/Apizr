using System;
using System.Reactive.Linq;
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

        public async Task Set(string key, object value, TimeSpan? lifeSpan = null)
        {
            if (lifeSpan.HasValue)
                await BlobCache.LocalMachine.InsertObject(key, value, lifeSpan.Value);
            else
                await BlobCache.LocalMachine.InsertObject(key, value);
        }

        public async Task<T> Get<T>(string key)
        {
            return await BlobCache.LocalMachine.GetObject<T>(key).Catch(Observable.Return(default(T)));
        }

        public async Task<bool> Remove(string key)
        {
            try
            {
                await BlobCache.LocalMachine.Invalidate(key);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Apizr: clearing cache throwed an exception with message: {e.Message}");
                return false;
            }
        }

        public async Task Clear()
        {
            await BlobCache.LocalMachine.InvalidateAll();
        }
    }
}
