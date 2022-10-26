using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    /// <summary>
    /// The top level base mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TFormattedModelResultData">The formatted model result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    public abstract class ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TFormattedModelResultData,
        TApiRequestData, TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        ExecuteRequestBase<TFormattedModelResultData, TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrResultRequestOptions
        where TApizrRequestOptionsBuilder : IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteResultRequestBase(
            Expression<Func<TApizrRequestOptions, TWebApi, Task<TApiResultData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelRequestData, optionsBuilder)
        {

        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteResultRequestBase(
            Expression<Func<TApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelRequestData, optionsBuilder)
        {

        }
    }

    /// <summary>
    /// The top level base mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    public abstract class ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TModelResultData, TApiRequestData,
            TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrResultRequestOptions
        where TApizrRequestOptionsBuilder : IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod,
            Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TApizrRequestOptions, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod,
            modelRequestData, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod,
            modelRequestData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TFormattedModelResultData">The formatted model result type</typeparam>
    public abstract class ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData, TApiData, TModelData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrResultRequestOptions
        where TApizrRequestOptionsBuilder : IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public abstract class
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : ExecuteResultRequestBase<TWebApi, TModelData, TApiData
            , TModelData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrResultRequestOptions
        where TApizrRequestOptionsBuilder : IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public abstract class
        ExecuteResultRequestBase<TWebApi, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : ExecuteResultRequestBase<TWebApi, TApiData, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrResultRequestOptions
        where TApizrRequestOptionsBuilder : IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }
    }
}
