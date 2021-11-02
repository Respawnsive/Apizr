using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class CreateCommand<TPayload> : CreateCommandBase<TPayload, TPayload>
    {
        public CreateCommand(TPayload payload) : base(payload)
        {
        }

        public CreateCommand(TPayload payload, Context context) : base(payload, context)
        {
        }
    }
}