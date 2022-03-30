using System;
using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class CreateCommand<TModelData> : CreateCommandBase<TModelData, TModelData>
    {
        public CreateCommand(TModelData modelData, Action<Exception> onException = null) : base(modelData, onException)
        {
        }

        public CreateCommand(TModelData modelData, Context context, Action<Exception> onException = null) : base(modelData, context, onException)
        {
        }
    }
}