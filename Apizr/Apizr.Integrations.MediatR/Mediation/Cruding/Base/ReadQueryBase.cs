using Apizr.Mediation.Querying;

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
    }
}
