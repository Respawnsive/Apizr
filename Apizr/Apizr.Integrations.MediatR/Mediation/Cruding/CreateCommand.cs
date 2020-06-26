using Apizr.Mediation.Commanding;

namespace Apizr.Mediation.Cruding
{
    public class CreateCommand<TPayload> : CommandBase<TPayload, TPayload>
    {
        public CreateCommand(TPayload payload) : base(payload)
        {
        }
    }
}