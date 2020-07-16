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
            Priority priority = Priority.UserInitiated) : this((token, api) => executeApiMethod.Compile()(api),
            CancellationToken.None, priority)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken cancellationToken, Priority priority = Priority.UserInitiated)
        {
            ExecuteApiMethod = executeApiMethod;
            CancellationToken = cancellationToken;
            Priority = priority;
        }

        public Expression<Func<CancellationToken, TWebApi, Task>> ExecuteApiMethod { get; }
        public CancellationToken CancellationToken { get; }
        public Priority Priority { get; }
    }

    public class ExecuteRequest<TWebApi, TResult> : IRequest<TResult>
    {
        public ExecuteRequest(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : this((token, api) => executeApiMethod.Compile()(api),
            CancellationToken.None, priority)
        {
        }

        public ExecuteRequest(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod,
            CancellationToken cancellationToken, Priority priority = Priority.UserInitiated)
        {
            ExecuteApiMethod = executeApiMethod;
            CancellationToken = cancellationToken;
            Priority = priority;
        }

        public Expression<Func<CancellationToken, TWebApi, Task<TResult>>> ExecuteApiMethod { get; }
        public CancellationToken CancellationToken { get; }
        public Priority Priority { get; }
    }
}
