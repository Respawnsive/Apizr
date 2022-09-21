using System;
using Apizr.Mediation.Cruding.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding
{
    /// <summary>
    /// The mediation Update command
    /// </summary>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    /// <typeparam name="TRequestData">The request data type</typeparam>
    public class UpdateCommand<TKey, TRequestData> : UpdateCommandBase<TKey, TRequestData, Unit>
    {
        /// <summary>
        /// The mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public UpdateCommand(TKey key, TRequestData requestData, Action<Exception> onException = null) : base(key, requestData, onException)
        {
        }

        /// <summary>
        /// The mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public UpdateCommand(TKey key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(key, requestData, context, onException)
        {
        }
    }

    /// <summary>
    /// The mediation Update command
    /// </summary>
    /// <typeparam name="TRequestData">The request data type</typeparam>
    public class UpdateCommand<TRequestData> : UpdateCommandBase<TRequestData>
    {
        /// <summary>
        /// The mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public UpdateCommand(int key, TRequestData requestData, Action<Exception> onException = null) : base(key, requestData, onException)
        {
        }

        /// <summary>
        /// The mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public UpdateCommand(int key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(key, requestData, context, onException)
        {
        }
    }
}