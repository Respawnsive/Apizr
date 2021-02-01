using Apizr.Mediation.Requesting.Base;

namespace Apizr.Mediation.Querying
{
    public abstract class MediationQueryBase<TResponse> : PrioritizedRequestBase<TResponse>, IMediationQuery<TResponse>
    {
        protected MediationQueryBase()
        {
            
        }

        protected MediationQueryBase(int priority) : base(priority)
        {

        }
    }
}