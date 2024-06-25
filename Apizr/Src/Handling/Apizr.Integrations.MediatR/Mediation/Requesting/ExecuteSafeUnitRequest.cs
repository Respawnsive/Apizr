using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Base;
using Refit;

namespace Apizr.Mediation.Requesting
{
    /// <summary>
    /// The mediation execute unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteSafeUnitRequest<TWebApi, TModelData, TApiData> :
        ExecuteResultRequestBase<TWebApi, IApizrResponse, IApiResponse, TApiData, TModelData, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeUnitRequest(Expression<Func<TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod, TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeUnitRequest(Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod, TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    public class ExecuteSafeUnitRequest<TWebApi> : ExecuteResultRequestBase<TWebApi, IApizrResponse, IApiResponse,
        IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeUnitRequest(Expression<Func<TWebApi, Task<IApiResponse>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteSafeUnitRequest(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
    }
}
