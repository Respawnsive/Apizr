using Apizr.Mediation.Commanding;
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

    public class UpdateOptionalCommand<TPayload> : UpdateOptionalCommand<int, TPayload>
    {
        public UpdateOptionalCommand(int key, TPayload payload) : base(key, payload)
        {
        }
    }
}