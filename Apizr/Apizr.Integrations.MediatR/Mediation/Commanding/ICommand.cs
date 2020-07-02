using MediatR;

namespace Apizr.Mediation.Commanding
{
    public interface ICommand<out TPayload, out TResponse> :
        IRequest<TResponse>
    {
    }

    public interface ICommand<out TPayload> :
        IRequest<Unit>
    {
    }

    public interface ICommand : IRequest<Unit>
    {
    }
}
