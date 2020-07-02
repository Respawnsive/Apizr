using Apizr.Mediation.Cruding.Base;
using MediatR;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class DeleteOptionalCommand<T, TKey> : DeleteCommandBase<T, TKey, Option<Unit, ApizrException>>
    {
        public DeleteOptionalCommand(TKey key) : base(key)
        {
        }
    }

    public class DeleteOptionalCommand<T> : DeleteCommandBase<T, int, Option<Unit, ApizrException>>
    {
        public DeleteOptionalCommand(int key) : base(key)
        {
        }
    }
}