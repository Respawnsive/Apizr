using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Base;
using Optional;

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
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, Option<TModelResultData, ApizrException<TModelResultData>>, TApiRequestData,
            TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<TApizrRequestOptions, TWebApi, Task<TApiResultData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<TApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute result request (returning optional result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData, Option<TModelData, ApizrException<TModelData>>, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute result request (returning optional result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : ExecuteResultRequestBase<TWebApi, TApiData, TApiData,
        Option<TApiData, ApizrException<TApiData>>, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
    }
}
