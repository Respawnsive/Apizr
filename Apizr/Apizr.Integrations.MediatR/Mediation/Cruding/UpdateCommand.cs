using Apizr.Mediation.Cruding.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class UpdateCommand<TKey, TPayload> : UpdateCommandBase<TKey, TPayload, Unit>
    {
        public UpdateCommand(TKey key, TPayload payload) : base(key, payload)
        {
        }

        public UpdateCommand(TKey key, TPayload payload, Context context) : base(key, payload, context)
        {
        }
    }

    public class UpdateCommand<TPayload> : UpdateCommandBase<TPayload>
    {
        public UpdateCommand(int key, TPayload payload) : base(key, payload)
        {
        }

        public UpdateCommand(int key, TPayload payload, Context context) : base(key, payload, context)
        {
        }
    }
}