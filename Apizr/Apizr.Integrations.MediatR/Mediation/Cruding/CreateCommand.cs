using Apizr.Mediation.Cruding.Base;

namespace Apizr.Mediation.Cruding
{
    public class CreateCommand<TPayload> : CreateCommandBase<TPayload, TPayload>
    {
        public CreateCommand(TPayload payload) : base(payload)
        {
        }
    }
}