using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Querying
{
    public interface IMediationQuery<out TResponse> : 
        IRequest<TResponse>
    {
    }
}
