using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Optional.Requesting.Base;
using Polly;

namespace Apizr.Optional.Requesting
{
    public class ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> : ExecuteOptionalResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData>
    {
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData) : base(executeApiMethod, modelData)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData) : base(executeApiMethod, modelData)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public class ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData> :
        ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData>
    {
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public class ExecuteOptionalRequest<TWebApi, TApiData> : ExecuteOptionalResultRequestBase<TWebApi, TApiData>
    {
        public ExecuteOptionalRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        public ExecuteOptionalRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
