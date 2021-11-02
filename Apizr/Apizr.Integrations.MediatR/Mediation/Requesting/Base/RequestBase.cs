using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class RequestBase<TRequestResponse> : IRequest<TRequestResponse>
    {
        protected RequestBase() : this(new Context())
        {
            
        }

        protected RequestBase(Context context)
        {
            Context = context;
        }

        public Context Context { get; }
    }
}
