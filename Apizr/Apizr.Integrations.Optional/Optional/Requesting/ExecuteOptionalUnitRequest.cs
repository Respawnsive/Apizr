using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Optional.Requesting.Base;
using Polly;

namespace Apizr.Optional.Requesting
{
    /// <summary>
    /// The mediation execute optional unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData> : ExecuteOptionalUnitRequestBase<TWebApi, TModelData, TApiData>
    {
        /// <summary>
        /// The mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        public ExecuteOptionalUnitRequest(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        /// <summary>
        /// The mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        public ExecuteOptionalUnitRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        /// <summary>
        /// The mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        public ExecuteOptionalUnitRequest(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        /// <summary>
        /// The mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        public ExecuteOptionalUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    /// <summary>
    /// The mediation execute optional unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    public class ExecuteOptionalUnitRequest<TWebApi> : ExecuteOptionalUnitRequestBase<TWebApi>
    {
        /// <summary>
        /// The mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        public ExecuteOptionalUnitRequest(Expression<Func<TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        /// <summary>
        /// The mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        public ExecuteOptionalUnitRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        /// <summary>
        /// The mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        public ExecuteOptionalUnitRequest(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        /// <summary>
        /// The mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        public ExecuteOptionalUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
