using Apizr.Mediation.Querying;
using Fusillade;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadQueryBase<TResponse, TKey> : MediationQueryBase<TResponse>
    {
        protected ReadQueryBase(TKey key, Priority priority = Priority.UserInitiated) : base(priority)
        {
            Key = key;
        }

        public TKey Key { get; }
    }

    public abstract class ReadQueryBase<TResponse> : ReadQueryBase<TResponse, int>
    {
        protected ReadQueryBase(int key, Priority priority = Priority.UserInitiated) : base(key, priority)
        {
        }
    }
}
