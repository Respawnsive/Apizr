using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class ExecuteUnitRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData> : ExecuteRequestBase<TFormattedModelResultData, TModelData>
    {
        protected ExecuteUnitRequestBase(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }
    }

    public abstract class ExecuteUnitRequestBase<TWebApi, TModelData, TApiData> : ExecuteRequestBase<Unit, TModelData>
    {
        protected ExecuteUnitRequestBase(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }
    }

    public abstract class ExecuteUnitRequestBase<TWebApi, TFormattedModelResultData> : ExecuteRequestBase<TFormattedModelResultData>
    {
        protected ExecuteUnitRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }
    }

    public abstract class ExecuteUnitRequestBase<TWebApi> : ExecuteRequestBase
    {
        protected ExecuteUnitRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }

        protected ExecuteUnitRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }
    }
}
