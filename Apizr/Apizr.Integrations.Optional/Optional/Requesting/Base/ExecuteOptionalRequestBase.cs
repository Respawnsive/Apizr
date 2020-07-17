using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Requesting.Base;
using Fusillade;
using MediatR;
using Optional;

namespace Apizr.Optional.Requesting.Base
{
    public abstract class ExecuteOptionalRequestBase<TWebApi, TModelResponse, TApiResponse> :
        ExecuteRequestBase<TWebApi, TModelResponse, TApiResponse, Option<TModelResponse, ApizrException<TModelResponse>>>
    {
        protected ExecuteOptionalRequestBase(Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }

    public abstract class ExecuteOptionalRequestBase<TWebApi, TApiResponse> : ExecuteRequestBase<TWebApi, TApiResponse,
        Option<TApiResponse, ApizrException<TApiResponse>>>
    {
        protected ExecuteOptionalRequestBase(Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }

    public abstract class ExecuteOptionalRequestBase<TWebApi> : ExecuteRequestBase<TWebApi, Option<Unit, ApizrException>>
    {
        protected ExecuteOptionalRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }

        protected ExecuteOptionalRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated) : base(executeApiMethod, priority)
        {
        }
    }
}
