using System;
using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Base;
using Polly;
using Refit;

namespace Apizr.Mediation.Cruding
{
    /// <summary>
    /// The mediation Create command
    /// </summary>
    /// <typeparam name="TModelData">The data type</typeparam>
    public class SafeCreateCommand<TModelData> : CreateCommandBase<TModelData, IApizrResponse<TModelData>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Create command constructor
        /// </summary>
        /// <param name="modelData">The request data to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public SafeCreateCommand(TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(modelData, optionsBuilder)
        {
        }
    }
}