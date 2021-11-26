using Apizr.Mediation.Commanding;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class CreateCommandBase<TRequestData, TResultData> : MediationCommandBase<TRequestData, TResultData>
    {
        protected CreateCommandBase(TRequestData requestData) : base()
        {
            RequestData = requestData;
        }

        protected CreateCommandBase(TRequestData requestData, Context context) : base(context)
        {
            RequestData = requestData;
        }

        public TRequestData RequestData { get; }
    }
}
