using System.Linq.Expressions;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class RequestBase<TRequestResponse> : IRequest<TRequestResponse>
    {
        protected RequestBase(Expression executeApiMethod, Priority priority = Priority.UserInitiated)
        {
            ExecuteApiMethod = executeApiMethod;
            Priority = priority;
        }

        public Expression ExecuteApiMethod { get; }

        public Priority Priority { get; }
    }
}
