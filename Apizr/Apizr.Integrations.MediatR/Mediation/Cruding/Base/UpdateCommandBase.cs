using System;
using Apizr.Mediation.Commanding;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class UpdateCommandBase<TKey, TRequestData, TResultData> : MediationCommandBase<TRequestData, TResultData>
    {
        protected UpdateCommandBase(TKey key, TRequestData requestData, Action<Exception> onException = null) : base(onException)
        {
            Key = key;
            RequestData = requestData;
        }

        protected UpdateCommandBase(TKey key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            Key = key;
            RequestData = requestData;
        }

        public TKey Key { get; }
        public TRequestData RequestData { get; }
    }

    public abstract class UpdateCommandBase<TRequestData, TResultData> : UpdateCommandBase<int, TRequestData, TResultData>
    {
        protected UpdateCommandBase(int key, TRequestData requestData, Action<Exception> onException = null) : base(key, requestData, onException)
        {
        }

        protected UpdateCommandBase(int key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(key, requestData, context, onException)
        {
        }
    }

    public abstract class UpdateCommandBase<TRequestData> : UpdateCommandBase<TRequestData, Unit>
    {
        protected UpdateCommandBase(int key, TRequestData requestData, Action<Exception> onException = null) : base(key, requestData, onException)
        {
        }

        protected UpdateCommandBase(int key, TRequestData requestData, Context context, Action<Exception> onException = null) : base(key, requestData, context, onException)
        {
        }
    }
}