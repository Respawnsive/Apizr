using System;
using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class ReadQuery<TResultData, TKey> : ReadQueryBase<TResultData, TKey>
    {
        public ReadQuery(TKey key, bool clearCache = false, Action<Exception> onException = null) : base(key, clearCache, onException)
        {
        }

        public ReadQuery(TKey key, int priority, bool clearCache = false, Action<Exception> onException = null) : base(key, priority, clearCache, onException)
        {
        }

        public ReadQuery(TKey key, Context context, bool clearCache = false, Action<Exception> onException = null) : base(key, context, clearCache, onException)
        {
        }

        public ReadQuery(TKey key, int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(key, priority, context, clearCache, onException)
        {
        }
    }

    public class ReadQuery<TResultData> : ReadQueryBase<TResultData>
    {
        public ReadQuery(int key, bool clearCache = false, Action<Exception> onException = null) : base(key, clearCache, onException)
        {
        }

        public ReadQuery(int key, int priority, bool clearCache = false, Action<Exception> onException = null) : base(key, priority, clearCache, onException)
        {
        }

        public ReadQuery(int key, Context context, bool clearCache = false, Action<Exception> onException = null) : base(key, context, clearCache, onException)
        {
        }

        public ReadQuery(int key, int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(key, priority, context, clearCache, onException)
        {
        }
    }
}
