using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Optional.Requesting.Base;
using Fusillade;

namespace Apizr.Optional.Requesting
{
    public class ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse> :
        ExecuteOptionalRequestBase<TWebApi, TModelResponse, TApiResponse>
    {
        public ExecuteOptionalRequest(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }

    public class ExecuteOptionalRequest<TWebApi, TApiResponse> : ExecuteOptionalRequestBase<TWebApi, TApiResponse>
    {
        public ExecuteOptionalRequest(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }

    public class ExecuteOptionalRequest<TWebApi> : ExecuteOptionalRequestBase<TWebApi>
    {
        public ExecuteOptionalRequest(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }
}
