using Apizr.Mediation.Cruding.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class UpdateCommand<TKey, TRequestData> : UpdateCommandBase<TKey, TRequestData, Unit>
    {
        public UpdateCommand(TKey key, TRequestData requestData) : base(key, requestData)
        {
        }

        public UpdateCommand(TKey key, TRequestData requestData, Context context) : base(key, requestData, context)
        {
        }
    }

    public class UpdateCommand<TRequestData> : UpdateCommandBase<TRequestData>
    {
        public UpdateCommand(int key, TRequestData requestData) : base(key, requestData)
        {
        }

        public UpdateCommand(int key, TRequestData requestData, Context context) : base(key, requestData, context)
        {
        }
    }
}