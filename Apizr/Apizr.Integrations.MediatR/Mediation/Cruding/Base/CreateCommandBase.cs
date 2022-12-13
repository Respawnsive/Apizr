using System;
using Apizr.Configuring.Request;
using Apizr.Mediation.Commanding;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    /// <summary>
    /// The top level base mediation Create command
    /// </summary>
    /// <typeparam name="TRequestData">The api request type</typeparam>
    /// <typeparam name="TResultData">The api result type</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class CreateCommandBase<TRequestData, TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : MediationCommandBase<TRequestData, TResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData">The api request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected CreateCommandBase(TRequestData requestData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
            RequestData = requestData;
        }

        /// <summary>
        /// The request data to send
        /// </summary>
        public TRequestData RequestData { get; }
    }
}
