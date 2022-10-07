using System;
using Apizr.Configuring.Request;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    /// <summary>
    /// The top level base mediation request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    public abstract class RequestBase<TFormattedModelResultData> : IRequest<TFormattedModelResultData>
    {
        /// <summary>
        /// The base request constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected RequestBase(Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            OptionsBuilder = optionsBuilder;
        }

        /// <summary>
        /// The request options builder
        /// </summary>
        public Action<IApizrRequestOptionsBuilder> OptionsBuilder { get; protected set; }
    }

    /// <summary>
    /// The top level base mediation request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    /// <typeparam name="TModelRequestData">The request type</typeparam>
    public abstract class RequestBase<TFormattedModelResultData, TModelRequestData> : RequestBase<TFormattedModelResultData>
    {
        /// <summary>
        /// The base request constructor
        /// </summary>
        /// <param name="modelRequestData">The request type</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected RequestBase(TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
            ModelRequestData = modelRequestData;
        }

        /// <summary>
        /// The request to send
        /// </summary>
        public TModelRequestData ModelRequestData { get; }
    }
}
