using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    public class CreateOptionalCommand<TModelData> : CreateCommandBase<TModelData, Option<TModelData, ApizrException>>
    {
        public CreateOptionalCommand(TModelData modelData) : base(modelData)
        {
        }

        public CreateOptionalCommand(TModelData modelData, Context context) : base(modelData, context)
        {
        }
    }
}