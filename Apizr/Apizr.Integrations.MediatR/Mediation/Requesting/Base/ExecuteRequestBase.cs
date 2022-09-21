using System;
using System.Linq.Expressions;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    /// <summary>
    /// The top level base mediation execute request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    public abstract class ExecuteRequestBase<TFormattedModelResultData> : RequestBase<TFormattedModelResultData>
    {
        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteRequestBase(Expression executeApiMethod, Action<Exception> onException = null) : base(onException)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteRequestBase(Expression executeApiMethod, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        /// <summary>
        /// The request to execute
        /// </summary>
        public Expression ExecuteApiMethod { get; }
    }

    /// <summary>
    /// The top level base mediation execute request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    /// <typeparam name="TModelRequestData">The request data type</typeparam>
    public abstract class ExecuteRequestBase<TFormattedModelResultData, TModelRequestData> : ExecuteRequestBase<TFormattedModelResultData>
    {
        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteRequestBase(Expression executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteRequestBase(Expression executeApiMethod, TModelRequestData modelRequestData, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
            ModelRequestData = modelRequestData;
        }

        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteRequestBase(Expression executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteRequestBase(Expression executeApiMethod, TModelRequestData modelRequestData, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
            ModelRequestData = modelRequestData;
        }

        /// <summary>
        /// The data provided to the request
        /// </summary>
        public TModelRequestData ModelRequestData { get; }
    }

    /// <summary>
    /// The top level base mediation execute request
    /// </summary>
    public abstract class ExecuteRequestBase : ExecuteRequestBase<Unit>
    {
        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteRequestBase(Expression executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteRequestBase(Expression executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }
    }
}
