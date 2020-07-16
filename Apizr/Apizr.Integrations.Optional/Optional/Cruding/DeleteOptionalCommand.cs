using Apizr.Mediation.Cruding.Base;
using Fusillade;
using MediatR;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class DeleteOptionalCommand<T, TKey> : DeleteCommandBase<T, TKey, Option<Unit, ApizrException>>
    {
        public DeleteOptionalCommand(TKey key, Priority priority = Priority.UserInitiated) : base(key, priority)
        {
        }
    }

    public class DeleteOptionalCommand<T> : DeleteCommandBase<T, Option<Unit, ApizrException>>
    {
        public DeleteOptionalCommand(int key, Priority priority = Priority.UserInitiated) : base(key, priority)
        {
        }
    }
}