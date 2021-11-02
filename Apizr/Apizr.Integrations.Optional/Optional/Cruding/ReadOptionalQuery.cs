using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    public class ReadOptionalQuery<TResponse, TKey> : ReadQueryBase<Option<TResponse, ApizrException<TResponse>>, TKey>
    {
        public ReadOptionalQuery(TKey key) : base(key)
        {
        }

        public ReadOptionalQuery(TKey key, int priority) : base(key, priority)
        {
        }

        public ReadOptionalQuery(TKey key, Context context) : base(key, context)
        {
        }

        public ReadOptionalQuery(TKey key, int priority, Context context) : base(key, priority, context)
        {
        }
    }

    public class ReadOptionalQuery<TResponse> : ReadQueryBase<Option<TResponse, ApizrException<TResponse>>>
    {
        public ReadOptionalQuery(int key) : base(key)
        {
        }

        public ReadOptionalQuery(int key, int priority) : base(key, priority)
        {
        }

        public ReadOptionalQuery(int key, Context context) : base(key, context)
        {
        }

        public ReadOptionalQuery(int key, int priority, Context context) : base(key, priority, context)
        {
        }
    }
}
