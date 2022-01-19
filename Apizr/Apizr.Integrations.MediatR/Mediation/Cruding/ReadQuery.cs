using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class ReadQuery<TResultData, TKey> : ReadQueryBase<TResultData, TKey>
    {
        public ReadQuery(TKey key, bool clearCache = false) : base(key, clearCache)
        {
        }

        public ReadQuery(TKey key, int priority, bool clearCache = false) : base(key, priority, clearCache)
        {
        }

        public ReadQuery(TKey key, Context context, bool clearCache = false) : base(key, context, clearCache)
        {
        }

        public ReadQuery(TKey key, int priority, Context context, bool clearCache = false) : base(key, priority, context, clearCache)
        {
        }
    }

    public class ReadQuery<TResultData> : ReadQueryBase<TResultData>
    {
        public ReadQuery(int key, bool clearCache = false) : base(key, clearCache)
        {
        }

        public ReadQuery(int key, int priority, bool clearCache = false) : base(key, priority, clearCache)
        {
        }

        public ReadQuery(int key, Context context, bool clearCache = false) : base(key, context, clearCache)
        {
        }

        public ReadQuery(int key, int priority, Context context, bool clearCache = false) : base(key, priority, context, clearCache)
        {
        }
    }
}
