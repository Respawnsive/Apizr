using System;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    /// <summary>
    /// The top level base mediation request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    public abstract class RequestBase<TFormattedModelResultData> : IRequest<TFormattedModelResultData>
    {
        protected RequestBase(Action<Exception> onException = null) : this(null, onException)
        {

        }

        protected RequestBase(Context context, Action<Exception> onException = null)
        {
            Context = context;
            OnException = onException;
        }

        public Context Context { get; }

        public Action<Exception> OnException { get; }
    }

    /// <summary>
    /// The top level base mediation request
    /// </summary>
    /// <typeparam name="TFormattedModelResultData">The result type</typeparam>
    /// <typeparam name="TModelRequestData">The request type</typeparam>
    public abstract class RequestBase<TFormattedModelResultData, TModelRequestData> : RequestBase<TFormattedModelResultData>
    {
        protected RequestBase(Action<Exception> onException = null) : this(default, null, onException)
        {

        }
        protected RequestBase(TModelRequestData modelRequestData, Action<Exception> onException = null) : this(modelRequestData, null, onException)
        {

        }

        protected RequestBase(Context context, Action<Exception> onException = null) : this(default, context, onException)
        {
        }

        protected RequestBase(TModelRequestData modelRequestData, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            ModelRequestData = modelRequestData;
        }

        public TModelRequestData ModelRequestData { get; }
    }
}
