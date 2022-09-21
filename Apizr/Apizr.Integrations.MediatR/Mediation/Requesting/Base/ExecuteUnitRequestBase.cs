using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    /// <summary>
    /// The top level base mediation execute unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TFormattedModelResultData">The formatted model result data type</typeparam>
    public abstract class ExecuteUnitRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData> : ExecuteRequestBase<TFormattedModelResultData, TModelData>
    {
        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public abstract class ExecuteUnitRequestBase<TWebApi, TModelData, TApiData> : ExecuteRequestBase<Unit, TModelData>
    {
        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TFormattedModelResultData">The formatted model result data type</typeparam>
    public abstract class ExecuteUnitRequestBase<TWebApi, TFormattedModelResultData> : ExecuteRequestBase<TFormattedModelResultData>
    {
        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    public abstract class ExecuteUnitRequestBase<TWebApi> : ExecuteRequestBase
    {
        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }

        /// <summary>
        /// The top level base mediation execute unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }
    }
}
