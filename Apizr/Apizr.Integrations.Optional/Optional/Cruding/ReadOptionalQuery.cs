using Apizr.Mediation.Cruding.Base;
using Apizr.Mediation.Querying;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class ReadOptionalQuery<TResponse, TKey> : ReadQueryBase<Option<TResponse, ApizrException<TResponse>>, TKey>
    {
        public ReadOptionalQuery(TKey key) : base(key)
        {
        }
    }

    public class ReadOptionalQuery<TResponse> : ReadQueryBase<Option<TResponse, ApizrException<TResponse>>>
    {
        public ReadOptionalQuery(int key) : base(key)
        {
        }
    }
}
