using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Requesting.Base;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Base
{
    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, Option<TModelResultData, ApizrException<TModelResultData>>, TApiRequestData,
            TModelRequestData>
    {
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData> :
        ExecuteResultRequestBase<TWebApi, TModelData, TApiData, Option<TModelData, ApizrException<TModelData>>>
    {
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public abstract class ExecuteOptionalResultRequestBase<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData, TApiData,
        Option<TApiData, ApizrException<TApiData>>>
    {
        protected ExecuteOptionalResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
