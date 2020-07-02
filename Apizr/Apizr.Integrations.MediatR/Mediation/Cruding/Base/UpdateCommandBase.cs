using Apizr.Mediation.Commanding;
using MediatR;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class UpdateCommandBase<TKey, TPayload, TResponse> : ICommand<TPayload, TResponse>
    {
        protected UpdateCommandBase(TKey key, TPayload payload)
        {
            Key = key;
            Payload = payload;
        }

        public TKey Key { get; }

        public TPayload Payload { get; }
    }

    public abstract class UpdateCommandBase<TKey, TPayload> : UpdateCommandBase<TKey, TPayload, Unit>
    {
        protected UpdateCommandBase(TKey key, TPayload payload) : base(key, payload)
        {
        }
    }

    public abstract class UpdateCommandBase<TPayload> : UpdateCommandBase<int, TPayload>
    {
        protected UpdateCommandBase(int key, TPayload payload) : base(key, payload)
        {
        }
    }
}