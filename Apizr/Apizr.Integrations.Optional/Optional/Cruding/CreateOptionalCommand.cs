using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    /// <summary>
    /// The mediation Create optional command
    /// </summary>
    /// <typeparam name="TModelData">The data type</typeparam>
    public class CreateOptionalCommand<TModelData> : CreateCommandBase<TModelData, Option<TModelData, ApizrException>>
    {
        /// <summary>
        /// The mediation Create optional command constructor
        /// </summary>
        /// <param name="modelData">The request data to send</param>
        public CreateOptionalCommand(TModelData modelData) : base(modelData)
        {
        }

        /// <summary>
        /// The mediation Create optional command constructor
        /// </summary>
        /// <param name="modelData">The request data to send</param>
        /// <param name="context">The Polly context to pass through</param>
        public CreateOptionalCommand(TModelData modelData, Context context) : base(modelData, context)
        {
        }
    }
}