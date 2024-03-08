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
    public class SafeUpdateCommand<TKey, TRequestData> : UpdateCommandBase<TKey, TRequestData, IApizrResponse, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public SafeUpdateCommand(TKey key, TRequestData requestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, requestData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation Update command
    /// </summary>
    /// <typeparam name="TRequestData">The request data type</typeparam>
    public class SafeUpdateCommand<TRequestData> : UpdateCommandBase<TRequestData, IApizrResponse, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Update command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public SafeUpdateCommand(int key, TRequestData requestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, requestData, optionsBuilder)
        {
        }
    }
}