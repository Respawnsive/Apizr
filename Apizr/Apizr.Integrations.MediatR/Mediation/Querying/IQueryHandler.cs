using Apizr.Requesting;
using MediatR;

namespace Apizr.Mediation.Querying
{
    public interface IQueryHandler<in TQuery, TResponse> :
        IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}
