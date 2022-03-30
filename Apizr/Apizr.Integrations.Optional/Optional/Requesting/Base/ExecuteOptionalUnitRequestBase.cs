using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Base
{
    public abstract class ExecuteOptionalUnitRequestBase<TWebApi, TModelData, TApiData> : ExecuteUnitRequestBase<TWebApi, TModelData, TApiData, Option<Unit, ApizrException>>
    {
        protected ExecuteOptionalUnitRequestBase(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteOptionalUnitRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteOptionalUnitRequestBase(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        protected ExecuteOptionalUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public abstract class ExecuteOptionalUnitRequestBase<TWebApi> : ExecuteUnitRequestBase<TWebApi, Option<Unit, ApizrException>>
    {
        protected ExecuteOptionalUnitRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalUnitRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteOptionalUnitRequestBase(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteOptionalUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
