using System.Linq.Expressions;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class ExecuteRequestBase<TFormattedModelResultData> : RequestBase<TFormattedModelResultData>
    {
        protected ExecuteRequestBase(Expression executeApiMethod)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        protected ExecuteRequestBase(Expression executeApiMethod, Context context) : base(context)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        public Expression ExecuteApiMethod { get; }
    }

    public abstract class ExecuteRequestBase<TFormattedModelResultData, TModelRequestData> : ExecuteRequestBase<TFormattedModelResultData>
    {
        protected ExecuteRequestBase(Expression executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression executeApiMethod, TModelRequestData modelRequestData) : base(executeApiMethod)
        {
            ModelRequestData = modelRequestData;
        }

        protected ExecuteRequestBase(Expression executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }

        protected ExecuteRequestBase(Expression executeApiMethod, TModelRequestData modelRequestData, Context context) : base(executeApiMethod, context)
        {
            ModelRequestData = modelRequestData;
        }

        public TModelRequestData ModelRequestData { get; }
    }

    public abstract class ExecuteRequestBase : ExecuteRequestBase<Unit>
    {
        protected ExecuteRequestBase(Expression executeApiMethod) : base(executeApiMethod)
        {
        }

        protected ExecuteRequestBase(Expression executeApiMethod, Context context) : base(executeApiMethod, context)
        {
        }
    }
}
