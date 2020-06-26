using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Querying
{
    public interface IQuery<out TResponse> : 
        IRequest<TResponse>
    {
    }
}
