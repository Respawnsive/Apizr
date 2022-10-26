using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;
using System;

namespace Apizr.Optional.Cruding
{
    /// <summary>
    /// The mediation Create optional command
    /// </summary>
    /// <typeparam name="TModelData">The data type</typeparam>
    public class CreateOptionalCommand<TModelData> : CreateCommandBase<TModelData, Option<TModelData, ApizrException>, IApizrResultRequestOptions, IApizrResultRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Create optional command constructor
        /// </summary>
        /// <param name="modelData">The request data to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public CreateOptionalCommand(TModelData modelData, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null) : base(modelData, optionsBuilder)
        {
        }
    }
}