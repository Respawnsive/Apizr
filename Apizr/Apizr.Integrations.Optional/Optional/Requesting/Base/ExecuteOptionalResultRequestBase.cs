using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Base
{
    /// <summary>
    /// The top level base mediation execute result request (returning optional result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, Option<TModelResultData, ApizrException<TModelResultData>>, TApiRequestData,
            TModelRequestData>
    {
        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiResultData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute result request (returning optional result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData> :
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData, Option<TModelData, ApizrException<TModelData>>>
    {
        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute result request (returning optional result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData, TApiData,
        Option<TApiData, ApizrException<TApiData>>>
    {
        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
    }
}
