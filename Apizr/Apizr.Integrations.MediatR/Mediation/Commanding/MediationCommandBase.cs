using Apizr.Mediation.Requesting.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Commanding
{
    public abstract class MediationCommandBase<TPayload, TResponse> : RequestBase<TResponse>, IMediationCommand<TPayload, TResponse>
    {
        protected MediationCommandBase() : base()
        {

        }

        protected MediationCommandBase(Context context) : base(context)
        {

        }
    }

    public abstract class MediationCommandBase<TPayload> : RequestBase<Unit>, IMediationCommand<TPayload>
    {
        protected MediationCommandBase() : base()
        {

        }

        protected MediationCommandBase(Context context) : base(context)
        {

        }
    }

    public abstract class MediationCommandBase : RequestBase<Unit>, IMediationCommand
    {
        protected MediationCommandBase() : base()
        {

        }

        protected MediationCommandBase(Context context) : base(context)
        {

        }
    }
}
