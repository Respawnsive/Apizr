using Apizr.Mediation.Cruding.Base;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    public class UpdateOptionalCommand<TKey, TRequestData> : UpdateCommandBase<TKey, TRequestData, Option<Unit, ApizrException>>
    {
        public UpdateOptionalCommand(TKey key, TRequestData requestData) : base(key, requestData)
        {
        }

        public UpdateOptionalCommand(TKey key, TRequestData requestData, Context context) : base(key, requestData, context)
        {
        }
    }

    public class UpdateOptionalCommand<TRequestData> : UpdateCommandBase<TRequestData, Option<Unit, ApizrException>>
    {
        public UpdateOptionalCommand(int key, TRequestData requestData) : base(key, requestData)
        {
        }

        public UpdateOptionalCommand(int key, TRequestData requestData, Context context) : base(key, requestData, context)
        {
        }
    }
}