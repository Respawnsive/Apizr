using System;
using Apizr.Configuring.Request;
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
    public class UpdateCommand<TKey, TRequestData> : UpdateCommandBase<TKey, TRequestData, Unit, IApizrCatchUnitRequestOptions, IApizrCatchUnitRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public UpdateCommand(TKey key, TRequestData requestData, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) : base(key, requestData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation Update command
    /// </summary>
    /// <typeparam name="TRequestData">The request data type</typeparam>
    public class UpdateCommand<TRequestData> : UpdateCommandBase<TRequestData, IApizrCatchUnitRequestOptions, IApizrCatchUnitRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public UpdateCommand(int key, TRequestData requestData, Action<IApizrCatchUnitRequestOptionsBuilder> optionsBuilder = null) : base(key, requestData, optionsBuilder)
        {
        }
    }
}