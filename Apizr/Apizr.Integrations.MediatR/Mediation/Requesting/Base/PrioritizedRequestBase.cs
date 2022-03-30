using System;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class PrioritizedRequestBase<TResultData> : RequestBase<TResultData>
    {
        protected PrioritizedRequestBase(Action<Exception> onException = null) : this(-1, onException)
        {
            
        }

        protected PrioritizedRequestBase(int priority, Action<Exception> onException = null) : base(onException)
        {
            Priority = priority;
        }

        protected PrioritizedRequestBase(Context context, Action<Exception> onException = null) : this(-1, context, onException)
        {

        }

        protected PrioritizedRequestBase(int priority, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            Priority = priority;
        }

        public int Priority { get; }
    }
}
