using System;
using Apizr.Mediation.Querying;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadQueryBase<TResponse, TKey> : MediationQueryBase<TResponse>
    {
        protected ReadQueryBase(TKey key, bool clearCache, Action<Exception> onException = null) : base(clearCache, onException)
        {
            Key = key;
        }

        protected ReadQueryBase(TKey key, int priority, bool clearCache, Action<Exception> onException = null) : base(priority, clearCache, onException)
        {
            Key = key;
        }

        protected ReadQueryBase(TKey key, Context context, bool clearCache, Action<Exception> onException = null) : base(context, clearCache, onException)
        {
            Key = key;
        }

        protected ReadQueryBase(TKey key, int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(priority, context, clearCache, onException)
        {
            Key = key;
        }

        public TKey Key { get; }
    }

    public abstract class ReadQueryBase<TResponse> : ReadQueryBase<TResponse, int>
    {
        protected ReadQueryBase(int key, bool clearCache, Action<Exception> onException = null) : base(key, clearCache, onException)
        {
        }

        protected ReadQueryBase(int key, int priority, bool clearCache, Action<Exception> onException = null) : base(key, priority, clearCache, onException)
        {
        }

        protected ReadQueryBase(int key, Context context, bool clearCache, Action<Exception> onException = null) : base(key, context, clearCache, onException)
        {
        }

        protected ReadQueryBase(int key, int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(key, priority, context, clearCache, onException)
        {
        }
    }
}
