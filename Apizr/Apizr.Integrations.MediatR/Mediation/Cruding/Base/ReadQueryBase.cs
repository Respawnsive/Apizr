using Apizr.Mediation.Querying;
using Fusillade;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadQueryBase<TResponse, TKey> : IQuery<TResponse>
    {
        protected ReadQueryBase(TKey key, Priority priority = Priority.UserInitiated)
        {
            Key = key;
            Priority = priority;
        }

        public TKey Key { get; }
        public Priority Priority { get; }
    }

    public abstract class ReadQueryBase<TResponse> : ReadQueryBase<TResponse, int>
    {
        protected ReadQueryBase(int key, Priority priority = Priority.UserInitiated) : base(key, priority)
        {
        }
    }
}
