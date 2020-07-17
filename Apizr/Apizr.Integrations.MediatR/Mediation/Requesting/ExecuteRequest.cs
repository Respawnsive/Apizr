using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Requesting.Base;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Requesting
{
    public class ExecuteRequest<TWebApi, TModelResponse, TApiResponse> :
            ExecuteRequestBase<TWebApi, TModelResponse, TApiResponse, TModelResponse>
    {
        public ExecuteRequest(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteRequest(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }

    public class ExecuteRequest<TWebApi, TApiResponse> : ExecuteRequestBase<TWebApi, TApiResponse, TApiResponse>
    {
        public ExecuteRequest(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }

    public class ExecuteRequest<TWebApi> : ExecuteRequestBase<TWebApi, Unit>
    {
        public ExecuteRequest(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }
}
