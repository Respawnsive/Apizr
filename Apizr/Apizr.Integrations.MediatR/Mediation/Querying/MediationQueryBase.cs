using Apizr.Mediation.Requesting.Base;
using Polly;

namespace Apizr.Mediation.Querying
{
    public abstract class MediationQueryBase<TResponse> : PrioritizedRequestBase<TResponse>, IMediationQuery<TResponse>
    {
        protected MediationQueryBase() : base()
        {
            
        }

        protected MediationQueryBase(int priority) : base(priority)
        {

        }

        protected MediationQueryBase(Context context) : base(context)
        {

        }

        protected MediationQueryBase(int priority, Context context) : base(priority, context)
        {

        }
    }
}