using MediatR;

namespace Apizr.Mediation.Requesting.Base
{
    public abstract class RequestBase<TRequestResponse> : IRequest<TRequestResponse>
    {
    }
}
