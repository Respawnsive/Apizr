using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fusillade;
using MediatR;
using Optional;

namespace Apizr.Optional.Requesting
{
    public class ExecuteOptionalRequest<TWebApi> : IRequest<Option<Unit, ApizrException>>
    {
        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, Priority priority = Priority.UserInitiated)
        {
            ExecuteApiMethod = executeApiMethod;
            Priority = priority;
        }

        public Expression<Func<CancellationToken, TWebApi, Task>> ExecuteApiMethod { get; }
        public Priority Priority { get; }
    }

    public class ExecuteOptionalRequest<TWebApi, TResult> : IRequest<Option<TResult, ApizrException<TResult>>>
    {
        public ExecuteOptionalRequest(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : this((token, api) => executeApiMethod.Compile()(api), priority)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, Priority priority = Priority.UserInitiated)
        {
            ExecuteApiMethod = executeApiMethod;
            Priority = priority;
        }

        public Expression<Func<CancellationToken, TWebApi, Task<TResult>>> ExecuteApiMethod { get; }
        public Priority Priority { get; }
    }
}
