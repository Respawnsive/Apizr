using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using Polly;

namespace Apizr.Mediation.Requesting
{
    public class ExecuteUnitRequest<TWebApi, TModelData, TApiData> :
        ExecuteUnitRequestBase<TWebApi, TModelData, TApiData>
    {
        public ExecuteUnitRequest(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        public ExecuteUnitRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<Exception> onException = null) : base(executeApiMethod, modelData, onException)
        {
        }

        public ExecuteUnitRequest(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }

        public ExecuteUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context, Action<Exception> onException = null) : base(executeApiMethod, modelData, context, onException)
        {
        }
    }

    public class ExecuteUnitRequest<TWebApi> : ExecuteUnitRequestBase<TWebApi>
    {
        public ExecuteUnitRequest(Expression<Func<TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        public ExecuteUnitRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        public ExecuteUnitRequest(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }

        public ExecuteUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }
    }
}
