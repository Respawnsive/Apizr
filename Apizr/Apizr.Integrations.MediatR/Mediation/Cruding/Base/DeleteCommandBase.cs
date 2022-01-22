using System;
using Apizr.Mediation.Commanding;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class DeleteCommandBase<T, TKey, TResultData> : MediationCommandBase<TKey, TResultData>
    {
        protected DeleteCommandBase(TKey key, Action<Exception> onException = null) : base(onException)
        {
            Key = key;
        }

        protected DeleteCommandBase(TKey key, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            Key = key;
        }

        public TKey Key { get; }
    }

    public abstract class DeleteCommandBase<T, TResultData> : DeleteCommandBase<T, int, TResultData>
    {
        protected DeleteCommandBase(int key, Action<Exception> onException = null) : base(key, onException)
        {
        }

        protected DeleteCommandBase(int key, Context context, Action<Exception> onException = null) : base(key, context, onException)
        {
        }
    }

    public abstract class DeleteCommandBase<T> : DeleteCommandBase<T, Unit>
    {
        protected DeleteCommandBase(int key, Action<Exception> onException = null) : base(key, onException)
        {
        }

        protected DeleteCommandBase(int key, Context context, Action<Exception> onException = null) : base(key, context, onException)
        {
        }
    }
}