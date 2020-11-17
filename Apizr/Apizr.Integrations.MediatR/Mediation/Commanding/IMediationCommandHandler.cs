using MediatR;

namespace Apizr.Mediation.Commanding
{
    public interface IMediationCommandHandler<in TCommand, TResponse> : 
        IRequestHandler<TCommand, TResponse> where TCommand : IRequest<TResponse>
    {

    }
    public interface IMediationCommandHandler<in TCommand> : IMediationCommandHandler<TCommand, Unit> where TCommand : IRequest<Unit>
    {

    }
}