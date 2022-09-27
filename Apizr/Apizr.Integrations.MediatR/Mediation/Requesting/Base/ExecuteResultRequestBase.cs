using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
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
        TApiRequestData, TModelRequestData> :
        ExecuteRequestBase<TFormattedModelResultData, TModelRequestData>
    {
        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(
            executeApiMethod, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(
            executeApiMethod, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, context, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, context, onException)
        {
            ClearCache = clearCache;
        }

        /// <summary>
        /// Asking to clear cache before sending the request
        /// </summary>
        public bool ClearCache { get; }
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
        TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TModelResultData, TApiRequestData,
            TModelRequestData>
    {
        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(
            executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(
            executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod,
            modelRequestData, context, clearCache, onException)
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
    public abstract class ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData> :
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData, TApiData, TModelData>
    {
        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(
            executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache, Action<Exception> onException = null) :
            base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, clearCache, onException)
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
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData> : ExecuteResultRequestBase<TWebApi, TModelData, TApiData
            , TModelData>
    {
        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(
            executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache, Action<Exception> onException = null) :
            base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public abstract class
        ExecuteResultRequestBase<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData, TApiData>
    {
        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(
            executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache, Action<Exception> onException = null) :
            base(executeApiMethod, context, clearCache, onException)
        {
        }
    }
}
