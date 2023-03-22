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
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class MediationQueryBase<TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestBase<TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder>, IMediationQuery<TResultData>
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The base mediation query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected MediationQueryBase(Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
        }
    }
}