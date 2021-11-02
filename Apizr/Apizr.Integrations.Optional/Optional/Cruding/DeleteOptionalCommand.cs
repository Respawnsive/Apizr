using Apizr.Mediation.Cruding.Base;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    public class DeleteOptionalCommand<T, TKey> : DeleteCommandBase<T, TKey, Option<Unit, ApizrException>>
    {
        public DeleteOptionalCommand(TKey key) : base(key)
        {
        }

        public DeleteOptionalCommand(TKey key, Context context) : base(key, context)
        {
        }
    }

    public class DeleteOptionalCommand<T> : DeleteCommandBase<T, Option<Unit, ApizrException>>
    {
        public DeleteOptionalCommand(int key) : base(key)
        {
        }

        public DeleteOptionalCommand(int key, Context context) : base(key, context)
        {
        }
    }
}