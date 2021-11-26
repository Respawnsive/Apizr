using Apizr.Mediation.Requesting.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Commanding
{
    public abstract class MediationCommandBase<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> : RequestBase<TModelResultData>, IMediationCommand<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>
    {
        protected MediationCommandBase() : base()
        {

        }

        protected MediationCommandBase(Context context) : base(context)
        {

        }
    }

    public abstract class MediationCommandBase<TRequestData, TResultData> : RequestBase<TResultData>, IMediationCommand<TRequestData, TResultData>
    {
        protected MediationCommandBase() : base()
        {

        }

        protected MediationCommandBase(Context context) : base(context)
        {

        }
    }

    public abstract class MediationCommandBase<TRequestData> : RequestBase<Unit>, IMediationCommand<TRequestData>
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
