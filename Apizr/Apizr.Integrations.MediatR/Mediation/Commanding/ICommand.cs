using MediatR;

namespace Apizr.Mediation.Commanding
{
    public interface ICommand<out TPayload, out TResponse> :
        IRequest<TResponse>
    {
        TPayload Payload { get; }
    }

    public interface ICommand<out TPayload> :
        IRequest
    {
    }

    public interface ICommand : IRequest
    {
    }
}
