using System;
using Apizr.Configuring.Request;
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
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected RequestBase(Action<Exception> onException) : this(null, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected RequestBase(Context context) : this(context, null)
        {
        }

        /// <summary>
        /// The base request constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected RequestBase(Context context, Action<Exception> onException) : this(options =>
            options.WithContext(context).WithExceptionCatcher(onException))
        {
        }

        /// <summary>
        /// The base request constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected RequestBase(Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            OptionsBuilder = optionsBuilder;
        }

        /// <summary>
        /// The request options builder
        /// </summary>
        public Action<IApizrRequestOptionsBuilder> OptionsBuilder { get; protected set; }
    }

    /// <summary>
    /// The top level base mediation request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    /// <typeparam name="TModelRequestData">The request type</typeparam>
    public abstract class RequestBase<TFormattedModelResultData, TModelRequestData> : RequestBase<TFormattedModelResultData>
    {
        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected RequestBase(Action<Exception> onException) : this(default, null, onException)
        {

        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected RequestBase(Context context) : this(default, context, null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected RequestBase(Context context, Action<Exception> onException) : this(default, context, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected RequestBase(TModelRequestData modelRequestData, Action<Exception> onException) : this(modelRequestData, null, onException)
        {

        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected RequestBase(TModelRequestData modelRequestData, Context context) : this(modelRequestData, context, null)
        {
        }

        /// <summary>
        /// The base request constructor
        /// </summary>
        /// <param name="modelRequestData">The request type</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected RequestBase(TModelRequestData modelRequestData, Context context, Action<Exception> onException) : this(modelRequestData, options =>
            options.WithContext(context).WithExceptionCatcher(onException))
        {
        }

        /// <summary>
        /// The base request constructor
        /// </summary>
        /// <param name="modelRequestData">The request type</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected RequestBase(TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
            ModelRequestData = modelRequestData;
        }

        /// <summary>
        /// The request to send
        /// </summary>
        public TModelRequestData ModelRequestData { get; }
    }
}
