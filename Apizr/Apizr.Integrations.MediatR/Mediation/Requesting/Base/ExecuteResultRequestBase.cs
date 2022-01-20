using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TFormattedModelResultData,
        TApiRequestData, TModelRequestData> :
        ExecuteRequestBase<TFormattedModelResultData, TModelRequestData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod)
        {
            ClearCache = clearCache;
        }

        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod)
        {
            ClearCache = clearCache;
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context)
        {
            ClearCache = clearCache;
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context)
        {
            ClearCache = clearCache;
        }

        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache) : base(executeApiMethod, modelRequestData)
        {
            ClearCache = clearCache;
        }

        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache) : base(executeApiMethod, modelRequestData)
        {
            ClearCache = clearCache;
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache) : base(executeApiMethod, modelRequestData, context)
        {
            ClearCache = clearCache;
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache) : base(executeApiMethod, modelRequestData, context)
        {
            ClearCache = clearCache;
        }

        public bool ClearCache { get; }
    }

    public abstract class ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TModelResultData, TApiRequestData,
            TModelRequestData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache) : base(executeApiMethod, modelRequestData, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, bool clearCache) : base(executeApiMethod, modelRequestData, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context, bool clearCache) : base(executeApiMethod, modelRequestData, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context, bool clearCache) : base(executeApiMethod,
            modelRequestData, context, clearCache)
        {
        }
    }

    public abstract class ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData> :
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData, TApiData, TModelData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) :
            base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    public abstract class
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData> : ExecuteResultRequestBase<TWebApi, TModelData, TApiData
            , TModelData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) :
            base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    public abstract class
        ExecuteResultRequestBase<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData, TApiData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(
            executeApiMethod, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteResultRequestBase(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) :
            base(executeApiMethod, context, clearCache)
        {
        }
    }
}
