using Apizr.Mediation.Commanding;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class CreateCommandBase<TPayload, TResponse> : MediationCommandBase<TPayload, TResponse>
    {
        protected CreateCommandBase(TPayload payload) : base()
        {
            Payload = payload;
        }

        protected CreateCommandBase(TPayload payload, Context context) : base(context)
        {
            Payload = payload;
        }

        public TPayload Payload { get; }
    }
}
