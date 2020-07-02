using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using MediatR;

namespace Apizr.Mediation.Cruding
{
    public class UpdateCommand<TKey, TPayload> : UpdateCommandBase<TKey, TPayload>
    {
        public UpdateCommand(TKey key, TPayload payload) : base(key, payload)
        {
        }
    }

    public class UpdateCommand<TPayload> : UpdateCommand<int, TPayload>
    {
        public UpdateCommand(int key, TPayload payload) : base(key, payload)
        {
        }
    }
}