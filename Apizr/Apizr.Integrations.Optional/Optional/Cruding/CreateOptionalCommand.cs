using Apizr.Mediation.Cruding.Base;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class CreateOptionalCommand<TPayload> : CreateCommandBase<TPayload, Option<TPayload, ApizrException>>
    {
        public CreateOptionalCommand(TPayload payload) : base(payload)
        {
        }
    }
}