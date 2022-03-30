using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using Polly;

namespace Apizr.Mediation.Requesting
{
    public class ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>
    {
        public ExecuteResultRequest(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, clearCache, onException)
        {
        }

        public ExecuteResultRequest(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, clearCache, onException)
        {
        }

        public ExecuteResultRequest(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelRequestData, context, clearCache, onException)
        {
        }

        public ExecuteResultRequest(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod,
            modelRequestData, context, clearCache, onException)
        {
        }
    }

    public class ExecuteResultRequest<TWebApi, TModelData, TApiData> :
            ExecuteResultRequestBase<TWebApi, TModelData, TApiData>
    {
        public ExecuteResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelData, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, clearCache, onException)
        {
        }
    }

    public class ExecuteResultRequest<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData>
    {
        public ExecuteResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false, Action<Exception> onException = null) : base(executeApiMethod, context, clearCache, onException)
        {
        }
    }
}
