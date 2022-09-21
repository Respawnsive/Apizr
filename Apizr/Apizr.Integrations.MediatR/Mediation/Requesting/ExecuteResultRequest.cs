using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using Polly;

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
    public class ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>
    {
        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, context, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod,
            modelRequestData, context, clearCache, onException)
        {
        }
    }

    /// <summary>
    /// The mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteResultRequest<TWebApi, TModelData, TApiData> :
            ExecuteResultRequestBase<TWebApi, TModelData, TApiData>
    {
        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }
    }

    /// <summary>
    /// The mediation execute result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteResultRequest<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData>
    {
        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ExecuteResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }
    }
}
