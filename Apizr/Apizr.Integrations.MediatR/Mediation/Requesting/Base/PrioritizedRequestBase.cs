using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class PrioritizedRequestBase<TRequestResponse> : RequestBase<TRequestResponse>
    {
        protected PrioritizedRequestBase() : this(-1)
        {
            
        }

        protected PrioritizedRequestBase(int priority) : base()
        {
            Priority = priority;
        }

        protected PrioritizedRequestBase(Context context) : this(-1, context)
        {

        }

        protected PrioritizedRequestBase(int priority, Context context) : base(context)
        {
            Priority = priority;
        }

        public int Priority { get; }
    }
}
