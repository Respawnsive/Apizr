using Apizr.Mediation.Requesting.Base;
using Polly;

namespace Apizr.Mediation.Querying
{
    public abstract class MediationQueryBase<TResultData> : PrioritizedRequestBase<TResultData>, IMediationQuery<TResultData>
    {
        protected MediationQueryBase(bool clearCache) : base()
        {
            ClearCache = clearCache;
        }

        protected MediationQueryBase(int priority, bool clearCache) : base(priority)
        {
            ClearCache = clearCache;
        }

        protected MediationQueryBase(Context context, bool clearCache) : base(context)
        {
            ClearCache = clearCache;
        }

        protected MediationQueryBase(int priority, Context context, bool clearCache) : base(priority, context)
        {
            ClearCache = clearCache;
        }

        public bool ClearCache { get; }
    }
}