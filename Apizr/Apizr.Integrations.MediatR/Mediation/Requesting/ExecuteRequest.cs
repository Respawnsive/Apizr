using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Requesting.Base;
using MediatR;

namespace Apizr.Mediation.Requesting
{
    public class ExecuteRequest<TWebApi, TModelResponse, TApiResponse> :
            ExecuteRequestBase<TWebApi, TModelResponse, TApiResponse, TModelResponse>
    {
        public ExecuteRequest(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteRequest(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }
    }

    public class ExecuteRequest<TWebApi, TApiResponse> : ExecuteRequestBase<TWebApi, TApiResponse, TApiResponse>
    {
        public ExecuteRequest(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod) : base(executeApiMethod)
        {
        }
    }

    public class ExecuteRequest<TWebApi> : ExecuteRequestBase<TWebApi, Unit>
    {
        public ExecuteRequest(Expression<Func<TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }
    }
}
