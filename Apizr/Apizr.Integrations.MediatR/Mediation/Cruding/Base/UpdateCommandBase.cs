using System;
using Apizr.Mediation.Commanding;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    /// <summary>
    /// The top level base mediation Update command
    /// </summary>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    /// <typeparam name="TRequestData">The request data type</typeparam>
    /// <typeparam name="TResultData">The result data type</typeparam>
    public abstract class UpdateCommandBase<TKey, TRequestData, TResultData> : MediationCommandBase<TRequestData, TResultData>
    {
        /// <summary>
        /// The top level base mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected UpdateCommandBase(TKey key, TRequestData requestData, Action<Exception> onException = null) : base(onException)
        {
            Key = key;
            RequestData = requestData;
        }

        /// <summary>
        /// The top level base mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected UpdateCommandBase(TKey key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            Key = key;
            RequestData = requestData;
        }

        /// <summary>
        /// The entity's crud key
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// The request data to send
        /// </summary>
        public TRequestData RequestData { get; }
    }

    /// <inheritdoc />
    public abstract class UpdateCommandBase<TRequestData, TResultData> : UpdateCommandBase<int, TRequestData, TResultData>
    {
        /// <inheritdoc />
        protected UpdateCommandBase(int key, TRequestData requestData, Action<Exception> onException = null) : base(key, requestData, onException)
        {
        }

        /// <inheritdoc />
        protected UpdateCommandBase(int key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(key, requestData, context, onException)
        {
        }
    }

    /// <inheritdoc />
    public abstract class UpdateCommandBase<TRequestData> : UpdateCommandBase<TRequestData, Unit>
    {
        /// <inheritdoc />
        protected UpdateCommandBase(int key, TRequestData requestData, Action<Exception> onException = null) : base(key, requestData, onException)
        {
        }

        /// <inheritdoc />
        protected UpdateCommandBase(int key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(key, requestData, context, onException)
        {
        }
    }
}