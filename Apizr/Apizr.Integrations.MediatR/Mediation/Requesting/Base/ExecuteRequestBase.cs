using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Fusillade;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class ExecuteRequestBase<TRequestResponse> : RequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression executeApiMethod,
        Priority priority = Priority.UserInitiated) : base(priority)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        public Expression ExecuteApiMethod { get; }
    }

    public abstract class ExecuteRequestBase<TWebApi, TModelResponse, TApiResponse, TRequestResponse> : ExecuteRequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }

    public abstract class ExecuteRequestBase<TWebApi, TApiResponse, TRequestResponse> : ExecuteRequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }

    public abstract class ExecuteRequestBase<TWebApi, TRequestResponse> : ExecuteRequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }
}
