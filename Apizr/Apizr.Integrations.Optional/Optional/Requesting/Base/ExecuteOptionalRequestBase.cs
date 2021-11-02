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
    public abstract class ExecuteOptionalRequestBase<TWebApi, TModelResponse, TApiResponse> :
        ExecuteRequestBase<TWebApi, TModelResponse, TApiResponse, Option<TModelResponse, ApizrException<TModelResponse>>>
    {
        protected ExecuteOptionalRequestBase(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<Context, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<Context, CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }

    public abstract class ExecuteOptionalRequestBase<TWebApi, TApiResponse> : ExecuteRequestBase<TWebApi, TApiResponse,
        Option<TApiResponse, ApizrException<TApiResponse>>>
    {
        protected ExecuteOptionalRequestBase(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }

    public abstract class ExecuteOptionalRequestBase<TWebApi> : ExecuteRequestBase<TWebApi, Option<Unit, ApizrException>>
    {
        protected ExecuteOptionalRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
