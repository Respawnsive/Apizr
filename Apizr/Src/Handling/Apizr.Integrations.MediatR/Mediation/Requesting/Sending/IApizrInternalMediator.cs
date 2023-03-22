using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Apizr.Mediation.Requesting.Sending
{
    internal interface IApizrInternalMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
