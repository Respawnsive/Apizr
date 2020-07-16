using Apizr.Mediation.Cruding.Base;
using Fusillade;

namespace Apizr.Mediation.Cruding
{
    public class CreateCommand<TPayload> : CreateCommandBase<TPayload, TPayload>
    {
        public CreateCommand(TPayload payload, Priority priority = Priority.UserInitiated) : base(payload, priority)
        {
        }
    }
}