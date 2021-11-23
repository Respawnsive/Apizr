using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using Polly;

namespace Apizr.Mediation.Requesting
{
    public class ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>
    {
        public ExecuteResultRequest(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData) : base(executeApiMethod, modelRequestData)
        {
        }

        public ExecuteResultRequest(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData) : base(executeApiMethod, modelRequestData)
        {
        }

        public ExecuteResultRequest(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context) : base(executeApiMethod, modelRequestData, context)
        {
        }

        public ExecuteResultRequest(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod, TModelRequestData modelRequestData, Context context) : base(executeApiMethod,
            modelRequestData, context)
        {
        }
    }

    public class ExecuteResultRequest<TWebApi, TModelData, TApiData> :
            ExecuteResultRequestBase<TWebApi, TModelData, TApiData>
    {
        public ExecuteResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteResultRequest(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        public ExecuteResultRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public class ExecuteResultRequest<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData>
    {
        public ExecuteResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
