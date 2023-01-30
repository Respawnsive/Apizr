using MediatR;

namespace Apizr.Mediation.Querying
{
    /// <summary>
    /// A mediation query getting some <typeparamref name="TResultData"/> data
    /// </summary>
    /// <typeparam name="TResultData">The returned data</typeparam>
    public interface IMediationQuery<out TResultData> : 
        IRequest<TResultData>
    {
    }
}
