using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Optional.Requesting.Base;
using Polly;

namespace Apizr.Optional.Requesting
{
    public class ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse> :
        ExecuteOptionalRequestBase<TWebApi, TModelResponse, TApiResponse>
    {
        public ExecuteOptionalRequest(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }

    public class ExecuteOptionalRequest<TWebApi, TApiResponse> : ExecuteOptionalRequestBase<TWebApi, TApiResponse>
    {
        public ExecuteOptionalRequest(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }

    public class ExecuteOptionalRequest<TWebApi> : ExecuteOptionalRequestBase<TWebApi>
    {
        public ExecuteOptionalRequest(Expression<Func<TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
