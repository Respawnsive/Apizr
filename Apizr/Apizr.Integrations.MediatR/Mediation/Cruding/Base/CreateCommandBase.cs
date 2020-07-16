using Apizr.Mediation.Commanding;
using Fusillade;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class CreateCommandBase<TPayload, TResponse> : ICommand<TPayload, TResponse>
    {
        protected CreateCommandBase(TPayload payload, Priority priority = Priority.UserInitiated)
        {
            Payload = payload;
            Priority = priority;
        }

        public TPayload Payload { get; }
        public Priority Priority { get; }
    }
}
