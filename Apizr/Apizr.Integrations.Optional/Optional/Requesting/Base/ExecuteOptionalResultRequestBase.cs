using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Base
{
    /// <summary>
    /// The top level base mediation execute result request (returning optional result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, Option<TModelResultData, ApizrException<TModelResultData>>, TApiRequestData,
            TModelRequestData>
    {
        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    /// <inheritdoc />
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData> :
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData, Option<TModelData, ApizrException<TModelData>>>
    {
        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    /// <inheritdoc />
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData, TApiData,
        Option<TApiData, ApizrException<TApiData>>>
    {
        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        /// <inheritdoc />
        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }
    }
}
