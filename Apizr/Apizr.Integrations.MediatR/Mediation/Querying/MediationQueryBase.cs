using System;
using Apizr.Mediation.Requesting.Base;
using Polly;

namespace Apizr.Mediation.Querying
{
    /// <summary>
    /// The base mediation query getting some <typeparamref name="TResultData"/> data
    /// </summary>
    /// <typeparam name="TResultData">The returned data</typeparam>
    public abstract class MediationQueryBase<TResultData> : PrioritizedRequestBase<TResultData>, IMediationQuery<TResultData>
    {
        /// <summary>
        /// The base mediation query constructor
        /// </summary>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected MediationQueryBase(bool clearCache, Action<Exception> onException = null) : base(onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The base mediation query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected MediationQueryBase(int priority, bool clearCache, Action<Exception> onException = null) : base(priority, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The base mediation query constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected MediationQueryBase(Context context, bool clearCache, Action<Exception> onException = null) : base(context, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The base mediation query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected MediationQueryBase(int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(priority, context, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// Asking to clear cache before sending the request
        /// </summary>
        public bool ClearCache { get; }
    }
}