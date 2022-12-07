using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Base;
using MediatR;
using Optional;
using Polly;
using System;

namespace Apizr.Optional.Cruding
{
    /// <summary>
    /// The mediation Update optional command
    /// </summary>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    /// <typeparam name="TRequestData">The request data type</typeparam>
    public class UpdateOptionalCommand<TKey, TRequestData> : UpdateCommandBase<TKey, TRequestData, Option<Unit, ApizrException>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Update optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public UpdateOptionalCommand(TKey key, TRequestData requestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, requestData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation Update optional command
    /// </summary>
    /// <typeparam name="TRequestData">The request data type</typeparam>
    public class UpdateOptionalCommand<TRequestData> : UpdateCommandBase<TRequestData, Option<Unit, ApizrException>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Update optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="requestData">The request data to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public UpdateOptionalCommand(int key, TRequestData requestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, requestData, optionsBuilder)
        {
        }
    }
}