using Apizr.Mediation.Querying;

namespace Apizr.Mediation.Cruding
{
    public class ReadQuery<TResponse, TKey> : IQuery<TResponse>
    {
        public ReadQuery(TKey key)
        {
            Key = key;
        }

        public TKey Key { get; }
    }
}
