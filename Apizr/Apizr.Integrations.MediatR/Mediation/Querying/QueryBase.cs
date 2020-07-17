using Apizr.Mediation.Requesting.Base;
using Fusillade;

namespace Apizr.Mediation.Querying
{
    public abstract class QueryBase<TResponse> : RequestBase<TResponse>, IQuery<TResponse>
    {
        protected QueryBase(Priority priority = Priority.UserInitiated) : base(priority)
        {

        }
    }
}