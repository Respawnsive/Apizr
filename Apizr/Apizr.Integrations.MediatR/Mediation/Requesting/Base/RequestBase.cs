using Fusillade;
using MediatR;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class RequestBase<TRequestResponse> : IRequest<TRequestResponse>
    {
        protected RequestBase(Priority priority = Priority.UserInitiated)
        {
            Priority = priority;
        }

        public Priority Priority { get; }
    }
}
