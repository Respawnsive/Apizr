using System;
using System.Linq.Expressions;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class ExecuteRequestBase<TFormattedModelResultData> : RequestBase<TFormattedModelResultData>
    {
        protected ExecuteRequestBase(Expression executeApiMethod, Action<Exception> onException = null) : base(onException)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        protected ExecuteRequestBase(Expression executeApiMethod, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            ExecuteApiMethod = executeApiMethod;
        }

        public Expression ExecuteApiMethod { get; }
    }

    public abstract class ExecuteRequestBase<TFormattedModelResultData, TModelRequestData> : ExecuteRequestBase<TFormattedModelResultData>
    {
        protected ExecuteRequestBase(Expression executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        protected ExecuteRequestBase(Expression executeApiMethod, TModelRequestData modelRequestData, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
            ModelRequestData = modelRequestData;
        }

        protected ExecuteRequestBase(Expression executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }

        protected ExecuteRequestBase(Expression executeApiMethod, TModelRequestData modelRequestData, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
            ModelRequestData = modelRequestData;
        }

        public TModelRequestData ModelRequestData { get; }
    }

    public abstract class ExecuteRequestBase : ExecuteRequestBase<Unit>
    {
        protected ExecuteRequestBase(Expression executeApiMethod, Action<Exception> onException = null) : base(executeApiMethod, onException)
        {
        }

        protected ExecuteRequestBase(Expression executeApiMethod, Context context, Action<Exception> onException = null) : base(executeApiMethod, context, onException)
        {
        }
    }
}
