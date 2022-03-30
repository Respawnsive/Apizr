using System;
using Apizr.Mediation.Cruding.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class DeleteCommand<T, TKey> : DeleteCommandBase<T, TKey, Unit>
    {
        public DeleteCommand(TKey key, Action<Exception> onException = null) : base(key, onException)
        {
        }
        
        public DeleteCommand(TKey key, Context context, Action<Exception> onException = null) : base(key, context, onException)
        {
        }
    }

    public class DeleteCommand<T> : DeleteCommandBase<T>
    {
        public DeleteCommand(int key, Action<Exception> onException = null) : base(key, onException)
        {
        }

        public DeleteCommand(int key, Context context, Action<Exception> onException = null) : base(key, context, onException)
        {
        }
    }
}