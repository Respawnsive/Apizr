using Apizr.Mediation.Cruding.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class DeleteCommand<T, TKey> : DeleteCommandBase<T, TKey, Unit>
    {
        public DeleteCommand(TKey key) : base(key)
        {
        }
        
        public DeleteCommand(TKey key, Context context) : base(key, context)
        {
        }
    }

    public class DeleteCommand<T> : DeleteCommandBase<T>
    {
        public DeleteCommand(int key) : base(key)
        {
        }

        public DeleteCommand(int key, Context context) : base(key, context)
        {
        }
    }
}