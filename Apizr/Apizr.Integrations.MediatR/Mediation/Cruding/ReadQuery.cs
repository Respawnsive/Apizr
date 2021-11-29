using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class ReadQuery<TResultData, TKey> : ReadQueryBase<TResultData, TKey>
    {
        public ReadQuery(TKey key) : base(key)
        {
        }

        public ReadQuery(TKey key, int priority) : base(key, priority)
        {
        }

        public ReadQuery(TKey key, Context context) : base(key, context)
        {
        }

        public ReadQuery(TKey key, int priority, Context context) : base(key, priority, context)
        {
        }
    }

    public class ReadQuery<TResultData> : ReadQueryBase<TResultData>
    {
        public ReadQuery(int key) : base(key)
        {
        }

        public ReadQuery(int key, int priority) : base(key, priority)
        {
        }

        public ReadQuery(int key, Context context) : base(key, context)
        {
        }

        public ReadQuery(int key, int priority, Context context) : base(key, priority, context)
        {
        }
    }
}
