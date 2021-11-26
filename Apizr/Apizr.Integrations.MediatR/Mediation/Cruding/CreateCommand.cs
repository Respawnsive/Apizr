using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class CreateCommand<TModelData> : CreateCommandBase<TModelData, TModelData>
    {
        public CreateCommand(TModelData modelData) : base(modelData)
        {
        }

        public CreateCommand(TModelData modelData, Context context) : base(modelData, context)
        {
        }
    }
}