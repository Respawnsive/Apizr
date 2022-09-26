using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Optional.Requesting.Base;
using Polly;

namespace Apizr.Optional.Requesting
{
    /// <summary>
    /// The mediation execute optional result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    public class ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> : ExecuteOptionalResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData>
    {
        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, bool clearCache = false) : base(executeApiMethod, modelData, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, bool clearCache = false) : base(executeApiMethod, modelData, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, Context context, bool clearCache = false) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, Context context, bool clearCache = false) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    /// <summary>
    /// The mediation execute optional result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData> :
        ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData>
    {
        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache = false) : base(executeApiMethod, modelData, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache = false) : base(executeApiMethod, modelData, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache = false) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache = false) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    /// <summary>
    /// The mediation execute optional result request (returning result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteOptionalResultRequest<TWebApi, TApiData> : ExecuteOptionalResultRequestBase<TWebApi, TApiData>
    {
        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }
    }
}
