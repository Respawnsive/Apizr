using System;
using Apizr.Mediation.Cruding.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class UpdateCommand<TKey, TRequestData> : UpdateCommandBase<TKey, TRequestData, Unit>
    {
        public UpdateCommand(TKey key, TRequestData requestData, Action<Exception> onException = null) : base(key, requestData, onException)
        {
        }

        public UpdateCommand(TKey key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(key, requestData, context, onException)
        {
        }
    }

    public class UpdateCommand<TRequestData> : UpdateCommandBase<TRequestData>
    {
        public UpdateCommand(int key, TRequestData requestData, Action<Exception> onException = null) : base(key, requestData, onException)
        {
        }

        public UpdateCommand(int key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(key, requestData, context, onException)
        {
        }
    }
}