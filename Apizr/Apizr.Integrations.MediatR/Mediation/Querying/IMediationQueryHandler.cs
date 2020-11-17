using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Querying
{
    public interface IMediationQueryHandler<in TQuery, TResponse> :
        IRequestHandler<TQuery, TResponse>
        where TQuery : IMediationQuery<TResponse>
    {
    }
}
