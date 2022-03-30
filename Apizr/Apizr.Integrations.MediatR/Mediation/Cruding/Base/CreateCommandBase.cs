using System;
using Apizr.Mediation.Commanding;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class CreateCommandBase<TRequestData, TResultData> : MediationCommandBase<TRequestData, TResultData>
    {
        protected CreateCommandBase(TRequestData requestData, Action<Exception> onException = null) : base(onException)
        {
            RequestData = requestData;
        }

        protected CreateCommandBase(TRequestData requestData, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            RequestData = requestData;
        }

        public TRequestData RequestData { get; }
    }
}
