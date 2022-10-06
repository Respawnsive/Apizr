using System;
using Apizr.Configuring.Request;
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
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected PrioritizedRequestBase(Action<Exception> onException) : this(-1, null, onException)
        {
            
        }

        /// <summary>
        /// The base prioritized mediation query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected PrioritizedRequestBase(int priority, Action<Exception> onException) : this(priority, null, onException)
        {
        }

        /// <summary>
        /// The base prioritized mediation query constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected PrioritizedRequestBase(Context context, Action<Exception> onException) : this(-1, context, onException)
        {

        }

        /// <summary>
        /// The base prioritized mediation query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected PrioritizedRequestBase(int priority, Context context, Action<Exception> onException) : base(context, onException)
        {
            Priority = priority;
        }

        /// <summary>
        /// The base prioritized mediation query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected PrioritizedRequestBase(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : this(-1, optionsBuilder)
        {

        }

        /// <summary>
        /// The base prioritized mediation query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected PrioritizedRequestBase(int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
            Priority = priority;
        }

        /// <summary>
        /// The execution priority to apply
        /// </summary>
        public int Priority { get; }
    }
}
