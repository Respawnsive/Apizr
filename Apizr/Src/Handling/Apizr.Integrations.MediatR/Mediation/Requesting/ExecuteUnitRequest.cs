using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Base;

namespace Apizr.Mediation.Requesting
{
    /// <summary>
    /// The mediation execute unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteUnitRequest<TWebApi, TModelData, TApiData> :
        ExecuteUnitRequestBase<TWebApi, TModelData, TApiData, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteUnitRequest(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteUnitRequest(Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation execute unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    public class ExecuteUnitRequest<TWebApi> : ExecuteUnitRequestBase<TWebApi, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteUnitRequest(Expression<Func<TWebApi, Task>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ExecuteUnitRequest(Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
    }
}
