using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class RequestBase<TFormattedModelResultData> : IRequest<TFormattedModelResultData>
    {
        protected RequestBase() : this(null)
        {

        }

        protected RequestBase(Context context)
        {
            Context = context;
        }

        public Context Context { get; }
    }

    public abstract class RequestBase<TFormattedModelResultData, TModelRequestData> : RequestBase<TFormattedModelResultData>
    {
        protected RequestBase() : this(default, null)
        {

        }
        protected RequestBase(TModelRequestData modelRequestData) : this(modelRequestData, null)
        {

        }

        protected RequestBase(Context context) : this(default, context)
        {
        }

        protected RequestBase(TModelRequestData modelRequestData, Context context) : base(context)
        {
            ModelRequestData = modelRequestData;
        }

        public TModelRequestData ModelRequestData { get; }
    }
}
