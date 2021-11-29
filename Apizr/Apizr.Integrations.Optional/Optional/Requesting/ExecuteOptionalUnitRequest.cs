using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Optional.Requesting.Base;
using Polly;

namespace Apizr.Optional.Requesting
{
    public class ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData> : ExecuteOptionalUnitRequestBase<TWebApi, TModelData, TApiData>
    {
        protected ExecuteOptionalUnitRequest(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteOptionalUnitRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteOptionalUnitRequest(Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        protected ExecuteOptionalUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public class ExecuteOptionalUnitRequest<TWebApi> : ExecuteOptionalUnitRequestBase<TWebApi>
    {
        public ExecuteOptionalUnitRequest(Expression<Func<TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalUnitRequest(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalUnitRequest(Expression<Func<Context, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalUnitRequest(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
