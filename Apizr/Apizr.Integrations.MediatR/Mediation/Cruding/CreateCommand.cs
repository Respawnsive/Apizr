using System;
using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    /// <summary>
    /// The mediation Create command
    /// </summary>
    /// <typeparam name="TModelData">The data type</typeparam>
    public class CreateCommand<TModelData> : CreateCommandBase<TModelData, TModelData>
    {
        /// <summary>
        /// The mediation Create command constructor
        /// </summary>
        /// <param name="modelData">The request data to send</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public CreateCommand(TModelData modelData, Action<Exception> onException = null) : base(modelData, onException)
        {
        }

        /// <summary>
        /// The mediation Create command constructor
        /// </summary>
        /// <param name="modelData">The request data to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public CreateCommand(TModelData modelData, Context context, Action<Exception> onException = null) : base(modelData, context, onException)
        {
        }
    }
}