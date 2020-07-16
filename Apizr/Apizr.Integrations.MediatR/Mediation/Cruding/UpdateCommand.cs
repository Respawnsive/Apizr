using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Cruding
{
    public class UpdateCommand<TKey, TPayload> : UpdateCommandBase<TKey, TPayload, Unit>
    {
        public UpdateCommand(TKey key, TPayload payload, Priority priority = Priority.UserInitiated) : base(key, payload, priority)
        {
        }
    }

    public class UpdateCommand<TPayload> : UpdateCommandBase<TPayload>
    {
        public UpdateCommand(int key, TPayload payload, Priority priority = Priority.UserInitiated) : base(key, payload, priority)
        {
        }
    }
}