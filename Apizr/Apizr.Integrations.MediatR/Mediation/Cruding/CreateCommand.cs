using System;
using Apizr.Configuring.Request;
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
        /// <param name="optionsBuilder">Options provided to the request</param>
        public CreateCommand(TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(modelData, optionsBuilder)
        {
        }
    }
}