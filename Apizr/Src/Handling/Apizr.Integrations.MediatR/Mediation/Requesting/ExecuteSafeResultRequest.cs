using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Base;
using Refit;

namespace Apizr.Mediation.Requesting
{
    /// <summary>
    /// The mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    public class ExecuteSafeResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, IApizrResponse<TModelResultData>, IApiResponse<TApiResultData>,
            TApiRequestData, TModelRequestData, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeResultRequest(
            Expression<Func<TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>>
                executeApiMethod, TModelRequestData modelRequestData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod,
            modelRequestData, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeResultRequest(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>>
                executeApiMethod, TModelRequestData modelRequestData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod,
            modelRequestData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteSafeResultRequest<TWebApi, TModelData, TApiData> :
        ExecuteResultRequestBase<TWebApi, IApizrResponse<TModelData>, IApiResponse<TApiData>, TApiData, TModelData, IApizrRequestOptions,
            IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeResultRequest(Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeResultRequest(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeResultRequest(
            Expression<Func<TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData,
            optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeResultRequest(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod,
            modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteSafeResultRequest<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi,
        IApizrResponse<TApiData>, IApiResponse<TApiData>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeResultRequest(Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeResultRequest(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
    }
}