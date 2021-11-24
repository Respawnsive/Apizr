using Apizr.Mediation.Commanding;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class UpdateCommandBase<TKey, TRequestData, TResultData> : MediationCommandBase<TRequestData, TResultData>
    {
        protected UpdateCommandBase(TKey key, TRequestData requestData)
        {
            Key = key;
            RequestData = requestData;
        }

        protected UpdateCommandBase(TKey key, TRequestData requestData, Context context) : base(context)
        {
            Key = key;
            RequestData = requestData;
        }

        public TKey Key { get; }
        public TRequestData RequestData { get; }
    }

    public abstract class UpdateCommandBase<TRequestData, TResultData> : UpdateCommandBase<int, TRequestData, TResultData>
    {
        protected UpdateCommandBase(int key, TRequestData requestData) : base(key, requestData)
        {
        }

        protected UpdateCommandBase(int key, TRequestData requestData, Context context) : base(key, requestData, context)
        {
        }
    }

    public abstract class UpdateCommandBase<TRequestData> : UpdateCommandBase<TRequestData, Unit>
    {
        protected UpdateCommandBase(int key, TRequestData requestData) : base(key, requestData)
        {
        }

        protected UpdateCommandBase(int key, TRequestData requestData, Context context) : base(key, requestData, context)
        {
        }
    }
}