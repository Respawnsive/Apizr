using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    public class ReadOptionalQuery<TResultData, TKey> : ReadQueryBase<Option<TResultData, ApizrException<TResultData>>, TKey>
    {
        public ReadOptionalQuery(TKey key, bool clearCache = false) : base(key, clearCache)
        {
        }

        public ReadOptionalQuery(TKey key, int priority, bool clearCache = false) : base(key, priority, clearCache)
        {
        }

        public ReadOptionalQuery(TKey key, Context context, bool clearCache = false) : base(key, context, clearCache)
        {
        }

        public ReadOptionalQuery(TKey key, int priority, Context context, bool clearCache = false) : base(key, priority, context, clearCache)
        {
        }
    }

    public class ReadOptionalQuery<TResultData> : ReadQueryBase<Option<TResultData, ApizrException<TResultData>>>
    {
        public ReadOptionalQuery(int key, bool clearCache = false) : base(key, clearCache)
        {
        }

        public ReadOptionalQuery(int key, int priority, bool clearCache = false) : base(key, priority, clearCache)
        {
        }

        public ReadOptionalQuery(int key, Context context, bool clearCache = false) : base(key, context, clearCache)
        {
        }

        public ReadOptionalQuery(int key, int priority, Context context, bool clearCache = false) : base(key, priority, context, clearCache)
        {
        }
    }
}
