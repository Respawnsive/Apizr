using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Querying;

namespace Apizr.Mediation.Cruding
{
    public class ReadQuery<TResponse, TKey> : ReadQueryBase<TResponse, TKey>
    {
        public ReadQuery(TKey key) : base(key)
        {
        }
    }

    public class ReadQuery<TResponse> : ReadQueryBase<TResponse>
    {
        public ReadQuery(int key) : base(key)
        {
        }
    }
}
