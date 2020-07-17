using Apizr.Mediation.Commanding;
using Fusillade;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class CreateCommandBase<TPayload, TResponse> : CommandBase<TPayload, TResponse>
    {
        protected CreateCommandBase(TPayload payload, Priority priority = Priority.UserInitiated) : base(priority)
        {
            Payload = payload;
        }

        public TPayload Payload { get; }
    }
}
