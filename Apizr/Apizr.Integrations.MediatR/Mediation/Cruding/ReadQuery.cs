using Apizr.Mediation.Cruding.Base;

namespace Apizr.Mediation.Cruding
{
    public class ReadQuery<TResponse, TKey> : ReadQueryBase<TResponse, TKey>
    {
        public ReadQuery(TKey key) : base(key)
        {
        }

        public ReadQuery(TKey key, int priority) : base(key, priority)
        {
        }
    }

    public class ReadQuery<TResponse> : ReadQueryBase<TResponse>
    {
        public ReadQuery(int key) : base(key)
        {
        }

        public ReadQuery(int key, int priority) : base(key, priority)
        {
        }
    }
}
