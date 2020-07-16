using Apizr.Mediation.Cruding.Base;
using Fusillade;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class CreateOptionalCommand<TPayload> : CreateCommandBase<TPayload, Option<TPayload, ApizrException>>
    {
        public CreateOptionalCommand(TPayload payload, Priority priority = Priority.UserInitiated) : base(payload, priority)
        {
        }
    }
}