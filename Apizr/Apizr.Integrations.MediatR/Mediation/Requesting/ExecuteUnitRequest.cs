using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting
{
    public class ExecuteUnitRequest<TWebApi, TModelData, TApiData> :
        ExecuteUnitRequestBase<TWebApi, TModelData, TApiData>
    {
        public ExecuteUnitRequest(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        public ExecuteUnitRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        public ExecuteUnitRequest(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        public ExecuteUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public class ExecuteUnitRequest<TWebApi> : ExecuteUnitRequestBase<TWebApi>
    {
        public ExecuteUnitRequest(Expression<Func<TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteUnitRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteUnitRequest(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
