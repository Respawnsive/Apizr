using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
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
        #region Obsolete

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(
            executeApiMethod, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(
            executeApiMethod, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, context, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, context, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache) : base(executeApiMethod, modelRequestData, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelRequestData, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache) : base(executeApiMethod, modelRequestData, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelRequestData, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache) : base(executeApiMethod, modelRequestData, context)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelRequestData, context, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache) : base(executeApiMethod, modelRequestData, context)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelRequestData, context, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        #endregion

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteResultRequestBase(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiResultData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelRequestData, optionsBuilder)
        {

        }

        /// <summary>
        /// The top level base mediation execute result request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelRequestData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteResultRequestBase(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelRequestData, optionsBuilder)
        {

        }
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
        #region Obsolete

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(
            executeApiMethod, clearCache, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(
            executeApiMethod, clearCache, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, context, clearCache, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, context, clearCache, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache) : base(executeApiMethod, modelRequestData, clearCache, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelRequestData, clearCache, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache) : base(executeApiMethod, modelRequestData, clearCache, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelRequestData, clearCache, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache) : base(executeApiMethod, modelRequestData, context, clearCache, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelRequestData, context, clearCache, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context, bool clearCache) : base(executeApiMethod,
            modelRequestData, context, clearCache, onException: null)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod,
            modelRequestData, context, clearCache, onException)
        {
            OptionsBuilder += options => options.WithCacheCleared(clearCache);
        }

        #endregion

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod,
            modelRequestData, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod,
            modelRequestData, optionsBuilder)
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
        #region Obsolete

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache, onException: null)
        {
        }
        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(
            executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) :
            base(executeApiMethod, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache, Action<Exception> onException) :
            base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }

        #endregion

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
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
        #region Obsolete

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(
            executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) :
            base(executeApiMethod, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache, Action<Exception> onException) :
            base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }

        #endregion

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
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
        #region Obsolete

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(
            executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache, Action<Exception> onException) : base(executeApiMethod, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache, Action<Exception> onException) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) :
            base(executeApiMethod, context, clearCache, onException: null)
        {
        }

        /// <inheritdoc />
        [Obsolete("Use the one with the request options builder parameter instead")]
        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache, Action<Exception> onException) :
            base(executeApiMethod, context, clearCache, onException)
        {
        }

        #endregion

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ExecuteResultRequestBase(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) :
            base(executeApiMethod, optionsBuilder)
        {
        }
    }
}
