using Apizr.Mediation.Commanding;

namespace Apizr.Mediation.Cruding
{
    public class UpdateCommand<TKey, TPayload> : CommandBase<TPayload>
    {
        public UpdateCommand(TKey key, TPayload payload) : base(payload)
        {
            Key = key;
        }

        public TKey Key { get; }
    }
}