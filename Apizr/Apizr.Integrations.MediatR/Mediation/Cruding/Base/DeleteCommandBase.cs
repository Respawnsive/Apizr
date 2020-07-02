using Apizr.Mediation.Commanding;
using MediatR;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class DeleteCommandBase<T, TKey, TResponse> : ICommand<TKey, TResponse>
    {
        protected DeleteCommandBase(TKey key)
        {
            Key = key;
        }

        public TKey Key { get; }
    }

    public abstract class DeleteCommandBase<T, TKey> : DeleteCommandBase<T, TKey, Unit>
    {
        protected DeleteCommandBase(TKey key) : base(key)
        {
        }
    }

    public abstract class DeleteCommandBase<T> : DeleteCommandBase<T, int, Unit>
    {
        protected DeleteCommandBase(int key) : base(key)
        {
        }
    }
}