using MediatR;

namespace Apizr.Mediation.Querying
{
    public interface IMediationQuery<out TResultData> : 
        IRequest<TResultData>
    {
    }
}
