using Apizr.Mediation.Cruding.Base;
using MediatR;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class UpdateOptionalCommand<TKey, TPayload> : UpdateCommandBase<TKey, TPayload, Option<Unit, ApizrException>>
    {
        public UpdateOptionalCommand(TKey key, TPayload payload) : base(key, payload)
        {
        }
    }

    public class UpdateOptionalCommand<TPayload> : UpdateCommandBase<TPayload, Option<Unit, ApizrException>>
    {
        public UpdateOptionalCommand(int key, TPayload payload) : base(key, payload)
        {
        }
    }
}