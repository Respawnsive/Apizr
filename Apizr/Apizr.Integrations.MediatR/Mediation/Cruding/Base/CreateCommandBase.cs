using System;
using Apizr.Mediation.Commanding;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    /// <summary>
    /// The top level base mediation Create command
    /// </summary>
    /// <typeparam name="TRequestData">The api request type</typeparam>
    /// <typeparam name="TResultData">The api result type</typeparam>
    public abstract class CreateCommandBase<TRequestData, TResultData> : MediationCommandBase<TRequestData, TResultData>
    {
        /// <inheritdoc />
        protected CreateCommandBase(TRequestData requestData, Action<Exception> onException = null) : base(onException)
        {
            RequestData = requestData;
        }

        /// <summary>
        /// The top level base mediation Create command constructor
        /// </summary>
        /// <param name="requestData">The request data to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected CreateCommandBase(TRequestData requestData, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            RequestData = requestData;
        }

        /// <summary>
        /// The request data to send
        /// </summary>
        public TRequestData RequestData { get; }
    }
}
