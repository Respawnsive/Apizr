using MediatR;

namespace Apizr.Mediation.Commanding
{
    public interface ICommandHandler<in TCommand, TResponse> : 
        IRequestHandler<TCommand, TResponse> where TCommand : IRequest<TResponse>
    {

    }
    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit> where TCommand : IRequest<Unit>
    {

    }
}