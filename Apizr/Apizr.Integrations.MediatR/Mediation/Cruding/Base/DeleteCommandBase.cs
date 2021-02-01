using Apizr.Mediation.Commanding;
using MediatR;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class DeleteCommandBase<T, TKey, TResponse> : MediationCommandBase<TKey, TResponse>
    {
        protected DeleteCommandBase(TKey key)
        {
            Key = key;
        }

        public TKey Key { get; }
    }

    public abstract class DeleteCommandBase<T, TResponse> : DeleteCommandBase<T, int, TResponse>
    {
        protected DeleteCommandBase(int key) : base(key)
        {
        }
    }

    public abstract class DeleteCommandBase<T> : DeleteCommandBase<T, Unit>
    {
        protected DeleteCommandBase(int key) : base(key)
        {
        }
    }
}