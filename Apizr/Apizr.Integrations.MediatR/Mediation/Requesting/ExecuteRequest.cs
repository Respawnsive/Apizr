using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Requesting
{
    public class ExecuteRequest<TWebApi> : IRequest<Unit>
    {
        public ExecuteRequest(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : this((token, api) => executeApiMethod.Compile()(api), priority)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, Priority priority = Priority.UserInitiated)
        {
            ExecuteApiMethod = executeApiMethod;
            Priority = priority;
        }

        public Expression<Func<CancellationToken, TWebApi, Task>> ExecuteApiMethod { get; }
        public Priority Priority { get; }
    }

    public class ExecuteRequest<TWebApi, TResult> : IRequest<TResult>
    {
        public ExecuteRequest(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : this((token, api) => executeApiMethod.Compile()(api), priority)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, Priority priority = Priority.UserInitiated)
        {
            ExecuteApiMethod = executeApiMethod;
            Priority = priority;
        }

        public Expression<Func<CancellationToken, TWebApi, Task<TResult>>> ExecuteApiMethod { get; }
        public Priority Priority { get; }
    }
}
