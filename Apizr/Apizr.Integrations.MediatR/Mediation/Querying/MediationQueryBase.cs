using System;
using Apizr.Mediation.Requesting.Base;
using Polly;

namespace Apizr.Mediation.Querying
{
    public abstract class MediationQueryBase<TResultData> : PrioritizedRequestBase<TResultData>, IMediationQuery<TResultData>
    {
        protected MediationQueryBase(bool clearCache, Action<Exception> onException = null) : base(onException)
        {
            ClearCache = clearCache;
        }

        protected MediationQueryBase(int priority, bool clearCache, Action<Exception> onException = null) : base(priority, onException)
        {
            ClearCache = clearCache;
        }

        protected MediationQueryBase(Context context, bool clearCache, Action<Exception> onException = null) : base(context, onException)
        {
            ClearCache = clearCache;
        }

        protected MediationQueryBase(int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(priority, context, onException)
        {
            ClearCache = clearCache;
        }

        public bool ClearCache { get; }
    }
}