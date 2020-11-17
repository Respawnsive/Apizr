using Apizr.Mediation.Requesting.Base;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Commanding
{
    public abstract class MediationCommandBase<TPayload, TResponse> : RequestBase<TResponse>, IMediationCommand<TPayload, TResponse>
    {
        protected MediationCommandBase(Priority priority = Priority.UserInitiated) : base(priority)
        {
            
        }
    }

    public abstract class MediationCommandBase<TPayload> : RequestBase<Unit>, IMediationCommand<TPayload>
    {
        protected MediationCommandBase(Priority priority = Priority.UserInitiated) : base(priority)
        {

        }
    }

    public abstract class MediationCommandBase : RequestBase<Unit>, IMediationCommand
    {
        protected MediationCommandBase(Priority priority = Priority.UserInitiated) : base(priority)
        {

        }
    }
}
