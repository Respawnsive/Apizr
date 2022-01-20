using Apizr.Mediation.Querying;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadQueryBase<TResponse, TKey> : MediationQueryBase<TResponse>
    {
        protected ReadQueryBase(TKey key, bool clearCache) : base(clearCache)
        {
            Key = key;
        }

        protected ReadQueryBase(TKey key, int priority, bool clearCache) : base(priority, clearCache)
        {
            Key = key;
        }

        protected ReadQueryBase(TKey key, Context context, bool clearCache) : base(context, clearCache)
        {
            Key = key;
        }

        protected ReadQueryBase(TKey key, int priority, Context context, bool clearCache) : base(priority, context, clearCache)
        {
            Key = key;
        }

        public TKey Key { get; }
    }

    public abstract class ReadQueryBase<TResponse> : ReadQueryBase<TResponse, int>
    {
        protected ReadQueryBase(int key, bool clearCache) : base(key, clearCache)
        {
        }

        protected ReadQueryBase(int key, int priority, bool clearCache) : base(key, priority, clearCache)
        {
        }

        protected ReadQueryBase(int key, Context context, bool clearCache) : base(key, context, clearCache)
        {
        }

        protected ReadQueryBase(int key, int priority, Context context, bool clearCache) : base(key, priority, context, clearCache)
        {
        }
    }
}
