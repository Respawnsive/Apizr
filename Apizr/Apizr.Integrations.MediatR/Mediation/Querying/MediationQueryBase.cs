using Apizr.Mediation.Requesting.Base;
using Fusillade;

namespace Apizr.Mediation.Querying
{
    public abstract class MediationQueryBase<TResponse> : RequestBase<TResponse>, IMediationQuery<TResponse>
    {
        protected MediationQueryBase(Priority priority = Priority.UserInitiated) : base(priority)
        {

        }
    }
}