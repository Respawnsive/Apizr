using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using MediatR;

namespace Apizr.Mediation.Cruding
{
    public class UpdateCommand<TKey, TPayload> : UpdateCommandBase<TKey, TPayload, Unit>
    {
        public UpdateCommand(TKey key, TPayload payload) : base(key, payload)
        {
        }
    }

    public class UpdateCommand<TPayload> : UpdateCommandBase<TPayload>
    {
        public UpdateCommand(int key, TPayload payload) : base(key, payload)
        {
        }
    }
}