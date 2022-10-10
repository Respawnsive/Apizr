using System;
using Apizr.Configuring.Request;
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
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected MediationQueryBase(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
        }

        /// <summary>
        /// The base mediation query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected MediationQueryBase(int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(priority, optionsBuilder)
        {
        }
    }
}