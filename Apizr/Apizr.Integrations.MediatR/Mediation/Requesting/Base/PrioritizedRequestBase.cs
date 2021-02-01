using MediatR;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class PrioritizedRequestBase<TRequestResponse> : RequestBase<TRequestResponse>
    {
        protected PrioritizedRequestBase() : this(-1)
        {
            
        }

        protected PrioritizedRequestBase(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; }
    }
}
