using Apizr.Mediation.Commanding;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class CreateCommandBase<TPayload, TResponse> : MediationCommandBase<TPayload, TResponse>
    {
        protected CreateCommandBase(TPayload payload)
        {
            Payload = payload;
        }

        public TPayload Payload { get; }
    }
}
