using Apizr.Mediation.Commanding;
using Apizr.Mediation.Cruding.Base;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Cruding
{
    public class DeleteCommand<T, TKey, TResponse> : DeleteCommandBase<T, TKey, TResponse>
    {
        public DeleteCommand(TKey key, Priority priority = Priority.UserInitiated) : base(key, priority)
        {
        }
    }

    public class DeleteCommand<T, TKey> : DeleteCommandBase<T, TKey, Unit>
    {
        public DeleteCommand(TKey key, Priority priority = Priority.UserInitiated) : base(key, priority)
        {
        }
    }

    public class DeleteCommand<T> : DeleteCommandBase<T>
    {
        public DeleteCommand(int key, Priority priority = Priority.UserInitiated) : base(key, priority)
        {
        }
    }
}