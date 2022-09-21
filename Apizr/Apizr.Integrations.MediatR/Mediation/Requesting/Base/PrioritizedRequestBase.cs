using System;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    /// <summary>
    /// The base prioritized mediation request getting some <typeparamref name="TResultData"/> data
    /// </summary>
    /// <typeparam name="TResultData">The returned data</typeparam>
    public abstract class PrioritizedRequestBase<TResultData> : RequestBase<TResultData>
    {
        /// <summary>
        /// The base prioritized mediation query constructor
        /// </summary>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected PrioritizedRequestBase(Action<Exception> onException = null) : this(-1, onException)
        {
            
        }

        /// <summary>
        /// The base prioritized mediation query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected PrioritizedRequestBase(int priority, Action<Exception> onException = null) : base(onException)
        {
            Priority = priority;
        }

        /// <summary>
        /// The base prioritized mediation query constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected PrioritizedRequestBase(Context context, Action<Exception> onException = null) : this(-1, context, onException)
        {

        }

        /// <summary>
        /// The base prioritized mediation query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected PrioritizedRequestBase(int priority, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            Priority = priority;
        }

        /// <summary>
        /// The execution priority to apply
        /// </summary>
        public int Priority { get; }
    }
}
