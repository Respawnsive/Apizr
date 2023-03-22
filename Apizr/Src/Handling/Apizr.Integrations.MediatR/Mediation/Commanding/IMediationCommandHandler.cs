using MediatR;

namespace Apizr.Mediation.Commanding
{
    /// <summary>
    /// The mediation command handler
    /// </summary>
    /// <typeparam name="TCommand">The handled command type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public interface IMediationCommandHandler<in TCommand, TResponse> : 
        IRequestHandler<TCommand, TResponse> where TCommand : IRequest<TResponse>
    {

    }

    /// <summary>
    /// The mediation command handler
    /// </summary>
    /// <typeparam name="TCommand">The handled command type</typeparam>
    public interface IMediationCommandHandler<in TCommand> : IMediationCommandHandler<TCommand, Unit> where TCommand : IRequest<Unit>
    {

    }
}