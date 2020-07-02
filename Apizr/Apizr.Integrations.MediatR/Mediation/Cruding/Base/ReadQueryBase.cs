using Apizr.Mediation.Querying;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadQueryBase<TResponse, TKey> : IQuery<TResponse>
    {
        protected ReadQueryBase(TKey key)
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
    }
}
