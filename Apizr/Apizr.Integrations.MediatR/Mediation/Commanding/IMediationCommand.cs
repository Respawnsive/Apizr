using MediatR;

namespace Apizr.Mediation.Commanding
{
    public interface IMediationCommand<out TPayload, out TResponse> :
        IRequest<TResponse>
    {
    }

    public interface IMediationCommand<out TPayload> :
        IRequest<Unit>
    {
    }

    public interface IMediationCommand : IRequest<Unit>
    {
    }
}
