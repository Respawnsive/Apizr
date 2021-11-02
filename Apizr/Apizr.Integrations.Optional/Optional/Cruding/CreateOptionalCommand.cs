using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    public class CreateOptionalCommand<TPayload> : CreateCommandBase<TPayload, Option<TPayload, ApizrException>>
    {
        public CreateOptionalCommand(TPayload payload) : base(payload)
        {
        }

        public CreateOptionalCommand(TPayload payload, Context context) : base(payload, context)
        {
        }
    }
}