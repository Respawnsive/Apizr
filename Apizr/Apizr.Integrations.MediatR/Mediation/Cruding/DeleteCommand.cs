using Apizr.Mediation.Cruding.Base;
using MediatR;

namespace Apizr.Mediation.Cruding
{
    public class DeleteCommand<T, TKey, TResponse> : DeleteCommandBase<T, TKey, TResponse>
    {
        public DeleteCommand(TKey key) : base(key)
        {
        }
    }

    public class DeleteCommand<T, TKey> : DeleteCommandBase<T, TKey, Unit>
    {
        public DeleteCommand(TKey key) : base(key)
        {
        }
    }

    public class DeleteCommand<T> : DeleteCommandBase<T>
    {
        public DeleteCommand(int key) : base(key)
        {
        }
    }
}