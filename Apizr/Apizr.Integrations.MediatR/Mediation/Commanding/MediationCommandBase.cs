using Apizr.Mediation.Requesting.Base;
using MediatR;

namespace Apizr.Mediation.Commanding
{
    public abstract class MediationCommandBase<TPayload, TResponse> : RequestBase<TResponse>, IMediationCommand<TPayload, TResponse>
    {
    }

    public abstract class MediationCommandBase<TPayload> : RequestBase<Unit>, IMediationCommand<TPayload>
    {
    }

    public abstract class MediationCommandBase : RequestBase<Unit>, IMediationCommand
    {
    }
}
