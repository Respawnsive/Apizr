using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Base
{
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, Option<TModelResultData, ApizrException<TModelResultData>>, TApiRequestData,
            TModelRequestData>
    {
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData> :
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData, Option<TModelData, ApizrException<TModelData>>>
    {
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache) : base(executeApiMethod, modelData, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData, TApiData,
        Option<TApiData, ApizrException<TApiData>>>
    {
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache) : base(executeApiMethod, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache) : base(executeApiMethod, context, clearCache)
        {
        }
    }
}
