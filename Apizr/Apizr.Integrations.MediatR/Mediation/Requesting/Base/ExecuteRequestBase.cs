using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class ExecuteRequestBase<TWebApi, TModelResponse, TApiResponse, TRequestResponse> : RequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : this((token, api, mapper) => executeApiMethod.Compile()(api),
            priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : this((token, api, mapper) => executeApiMethod.Compile()(token, api),
            priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : this((token, api, mapper) => executeApiMethod.Compile()(api, mapper),
            priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(priority)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        public Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> ExecuteApiMethod { get; }
    }

    public abstract class ExecuteRequestBase<TWebApi, TApiResponse, TRequestResponse> : RequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : this((token, api) => executeApiMethod.Compile()(api),
            priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(priority)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        public Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> ExecuteApiMethod { get; }
    }

    public abstract class ExecuteRequestBase<TWebApi, TRequestResponse> : RequestBase<TRequestResponse>
    {
        protected ExecuteRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : this((token, api) => executeApiMethod.Compile()(api),
            priority)
        {
        }

        protected ExecuteRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(priority)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        public Expression<Func<CancellationToken, TWebApi, Task>> ExecuteApiMethod { get; }
    }
}
