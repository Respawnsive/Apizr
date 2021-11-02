using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class ExecuteRequestBase<TRequestResponse> : RequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression executeApiMethod)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        protected ExecuteRequestBase(Expression executeApiMethod, Context context) : base(context)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        public Expression ExecuteApiMethod { get; }
    }

    public abstract class ExecuteRequestBase<TWebApi, TModelResponse, TApiResponse, TRequestResponse> : ExecuteRequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteRequestBase(Expression<Func<Context, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteRequestBase(Expression<Func<Context, CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }

    public abstract class ExecuteRequestBase<TWebApi, TApiResponse, TRequestResponse> : ExecuteRequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }

    public abstract class ExecuteRequestBase<TWebApi, TRequestResponse> : ExecuteRequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
