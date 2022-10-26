using System;
using System.Linq.Expressions;
using Apizr.Configuring.Request;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    /// <summary>
    /// The top level base mediation execute request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    public abstract class ExecuteRequestBase<TFormattedModelResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestBase<TFormattedModelResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder> 
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteRequestBase(Expression executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        /// <summary>
        /// The request to execute
        /// </summary>
        public Expression ExecuteApiMethod { get; }
    }

    /// <summary>
    /// The top level base mediation execute request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    /// <typeparam name="TModelRequestData">The request data type</typeparam>
    public abstract class ExecuteRequestBase<TFormattedModelResultData, TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : ExecuteRequestBase<TFormattedModelResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder> 
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteRequestBase(Expression executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteRequestBase(Expression executeApiMethod, TModelRequestData modelRequestData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
            ModelRequestData = modelRequestData;
        }

        /// <summary>
        /// The data provided to the request
        /// </summary>
        public TModelRequestData ModelRequestData { get; }
    }

    /// <summary>
    /// The top level base mediation execute request
    /// </summary>
    public abstract class ExecuteRequestBase<TApizrRequestOptions, TApizrRequestOptionsBuilder> : ExecuteRequestBase<Unit, TApizrRequestOptions, TApizrRequestOptionsBuilder> 
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation execute request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteRequestBase(Expression executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
    }
}
