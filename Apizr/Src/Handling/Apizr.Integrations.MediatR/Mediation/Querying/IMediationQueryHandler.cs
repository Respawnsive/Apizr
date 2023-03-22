using MediatR;

namespace Apizr.Mediation.Querying
{
    /// <summary>
    /// The mediation query handler
    /// </summary>
    /// <typeparam name="TQuery">The query to handle</typeparam>
    /// <typeparam name="TResponse">The response to send back</typeparam>
    public interface IMediationQueryHandler<in TQuery, TResponse> :
        IRequestHandler<TQuery, TResponse>
        where TQuery : IMediationQuery<TResponse>
    {
    }
}
