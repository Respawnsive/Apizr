using MediatR;

namespace Apizr.Mediation.Commanding
{
    public interface IMediationCommand<out TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> :
        IRequest<TModelResultData>
    {
    }

    public interface IMediationCommand<out TRequestData, out TResultData> :
        IRequest<TResultData>
    {
    }

    public interface IMediationCommand<out TRequestData> :
        IRequest<Unit>
    {
    }

    public interface IMediationCommand : IRequest<Unit>
    {
    }
}
