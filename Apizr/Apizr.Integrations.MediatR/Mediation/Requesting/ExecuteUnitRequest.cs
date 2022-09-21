using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using Polly;

namespace Apizr.Mediation.Requesting
{
    /// <summary>
    /// The mediation execute unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteUnitRequest<TWebApi, TModelData, TApiData> :
        ExecuteUnitRequestBase<TWebApi, TModelData, TApiData>
    {
        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteUnitRequest(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteUnitRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteUnitRequest(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }

        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }
    }

    /// <summary>
    /// The mediation execute unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    public class ExecuteUnitRequest<TWebApi> : ExecuteUnitRequestBase<TWebApi>
    {
        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteUnitRequest(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteUnitRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteUnitRequest(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }

        /// <summary>
        /// The mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }
    }
}
