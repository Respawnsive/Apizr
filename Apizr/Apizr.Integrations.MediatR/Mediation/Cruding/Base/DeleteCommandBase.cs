using Apizr.Mediation.Commanding;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class DeleteCommandBase<T, TKey, TResponse> : MediationCommandBase<TKey, TResponse>
    {
        protected DeleteCommandBase(TKey key) : base()
        {
            Key = key;
        }

        protected DeleteCommandBase(TKey key, Context context) : base(context)
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

        protected DeleteCommandBase(int key, Context context) : base(key, context)
        {
        }
    }

    public abstract class DeleteCommandBase<T> : DeleteCommandBase<T, Unit>
    {
        protected DeleteCommandBase(int key) : base(key)
        {
        }

        protected DeleteCommandBase(int key, Context context) : base(key, context)
        {
        }
    }
}