using Apizr.Mediation.Cruding.Base;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    /// <summary>
    /// The mediation Update optional command
    /// </summary>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    /// <typeparam name="TRequestData">The request data type</typeparam>
    public class UpdateOptionalCommand<TKey, TRequestData> : UpdateCommandBase<TKey, TRequestData, Option<Unit, ApizrException>>
    {
        /// <summary>
        /// The mediation Update optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        public UpdateOptionalCommand(TKey key, TRequestData requestData) : base(key, requestData)
        {
        }

        /// <summary>
        /// The mediation Update optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="context">The Polly context to pass through</param>
        public UpdateOptionalCommand(TKey key, TRequestData requestData, Context context) : base(key, requestData, context)
        {
        }
    }

    /// <summary>
    /// The mediation Update optional command
    /// </summary>
    /// <typeparam name="TRequestData">The request data type</typeparam>
    public class UpdateOptionalCommand<TRequestData> : UpdateCommandBase<TRequestData, Option<Unit, ApizrException>>
    {
        /// <summary>
        /// The mediation Update optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        public UpdateOptionalCommand(int key, TRequestData requestData) : base(key, requestData)
        {
        }

        /// <summary>
        /// The mediation Update optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="context">The Polly context to pass through</param>
        public UpdateOptionalCommand(int key, TRequestData requestData, Context context) : base(key, requestData, context)
        {
        }
    }
}