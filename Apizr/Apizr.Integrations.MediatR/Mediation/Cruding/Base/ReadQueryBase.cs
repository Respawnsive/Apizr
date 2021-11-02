using Apizr.Mediation.Querying;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadQueryBase<TResponse, TKey> : MediationQueryBase<TResponse>
    {
        protected ReadQueryBase(TKey key)
        {
            Key = key;
        }

        protected ReadQueryBase(TKey key, int priority) : base(priority)
        {
            Key = key;
        }

        protected ReadQueryBase(TKey key, Context context) : base(context)
        {
            Key = key;
        }

        protected ReadQueryBase(TKey key, int priority, Context context) : base(priority, context)
        {
            Key = key;
        }

        public TKey Key { get; }
    }

    public abstract class ReadQueryBase<TResponse> : ReadQueryBase<TResponse, int>
    {
        protected ReadQueryBase(int key) : base(key)
        {
        }

        protected ReadQueryBase(int key, int priority) : base(key, priority)
        {
        }

        protected ReadQueryBase(int key, Context context) : base(key, context)
        {
        }

        protected ReadQueryBase(int key, int priority, Context context) : base(key, priority, context)
        {
        }
    }
}
