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
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, bool clearCache = false) : base(executeApiMethod, modelData, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, bool clearCache = false) : base(executeApiMethod, modelData, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, Context context, bool clearCache = false) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelData, Context context, bool clearCache = false) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    public class ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData> :
        ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData>
    {
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache = false) : base(executeApiMethod, modelData, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, bool clearCache = false) : base(executeApiMethod, modelData, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache = false) : base(executeApiMethod, modelData, context, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context, bool clearCache = false) : base(executeApiMethod, modelData, context, clearCache)
        {
        }
    }

    public class ExecuteOptionalResultRequest<TWebApi, TApiData> : ExecuteOptionalResultRequestBase<TWebApi, TApiData>
    {
        public ExecuteOptionalResultRequest(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, bool clearCache = false) : base(executeApiMethod, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }

        public ExecuteOptionalResultRequest(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context, bool clearCache = false) : base(executeApiMethod, context, clearCache)
        {
        }
    }
}
