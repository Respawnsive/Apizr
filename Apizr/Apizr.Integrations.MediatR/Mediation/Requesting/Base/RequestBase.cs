using System;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    /// <summary>
    /// The top level base mediation request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    public abstract class RequestBase<TFormattedModelResultData> : IRequest<TFormattedModelResultData>
    {
        /// <inheritdoc />
        protected RequestBase(Action<Exception> onException = null) : this(null, onException)
        {

        }

        /// <summary>
        /// The base request constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected RequestBase(Context context, Action<Exception> onException = null)
        {
            Context = context;
            OnException = onException;
        }

        /// <summary>
        /// The Polly context to pass through
        /// </summary>
        public Context Context { get; }

        /// <summary>
        /// Action to execute when an exception occurs
        /// </summary>
        public Action<Exception> OnException { get; }
    }

    /// <summary>
    /// The top level base mediation request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    /// <typeparam name="TModelRequestData">The request type</typeparam>
    public abstract class RequestBase<TFormattedModelResultData, TModelRequestData> : RequestBase<TFormattedModelResultData>
    {
        /// <inheritdoc />
        protected RequestBase(Action<Exception> onException = null) : this(default, null, onException)
        {

        }

        /// <inheritdoc />
        protected RequestBase(TModelRequestData modelRequestData, Action<Exception> onException = null) : this(modelRequestData, null, onException)
        {

        }

        /// <inheritdoc />
        protected RequestBase(Context context, Action<Exception> onException = null) : this(default, context, onException)
        {
        }

        /// <summary>
        /// The base request constructor
        /// </summary>
        /// <param name="modelRequestData">The request type</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected RequestBase(TModelRequestData modelRequestData, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            ModelRequestData = modelRequestData;
        }

        /// <summary>
        /// The request to send
        /// </summary>
        public TModelRequestData ModelRequestData { get; }
    }
}
