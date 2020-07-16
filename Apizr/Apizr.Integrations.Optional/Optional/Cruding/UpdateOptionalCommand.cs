using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Fusillade;
using MediatR;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class UpdateOptionalCommand<TKey, TPayload> : UpdateCommandBase<TKey, TPayload, Option<Unit, ApizrException>>
    {
        public UpdateOptionalCommand(TKey key, TPayload payload, Priority priority = Priority.UserInitiated) : base(key, payload, priority)
        {
        }
    }

    public class UpdateOptionalCommand<TPayload> : UpdateCommandBase<TPayload, Option<Unit, ApizrException>>
    {
        public UpdateOptionalCommand(int key, TPayload payload, Priority priority = Priority.UserInitiated) : base(key, payload, priority)
        {
        }
    }
}