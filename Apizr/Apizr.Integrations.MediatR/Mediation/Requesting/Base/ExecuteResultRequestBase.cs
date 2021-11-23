using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TFormattedModelResultData, TApiRequestData, TModelRequestData> :
        ExecuteRequestBase<TFormattedModelResultData, TModelRequestData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData) : base(executeApiMethod, modelRequestData)
        {
        }
        
        protected ExecuteResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData) : base(executeApiMethod, modelRequestData)
        {
        }
        
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context) : base(executeApiMethod, modelRequestData, context)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context) : base(executeApiMethod, modelRequestData, context)
        {
        }
    }

    public abstract class ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> :
        ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TModelResultData, TApiRequestData, TModelRequestData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelRequestData) : base(executeApiMethod, modelRequestData)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelRequestData) : base(executeApiMethod, modelRequestData)
        {
        }
        
        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelRequestData, Context context) : base(executeApiMethod, modelRequestData, context)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod, TModelRequestData modelRequestData, Context context) : base(executeApiMethod, modelRequestData, context)
        {
        }
    }

    public abstract class ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData> : ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData, TApiData, TModelData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public abstract class ExecuteResultRequestBase<TWebApi, TModelData, TApiData> : ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TModelData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData) : base(executeApiMethod, modelData)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Context context) : base(executeApiMethod, modelData, context)
        {
        }
    }

    public abstract class ExecuteResultRequestBase<TWebApi, TApiData> : ExecuteResultRequestBase<TWebApi, TApiData, TApiData>
    {
        protected ExecuteResultRequestBase(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteResultRequestBase(Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
