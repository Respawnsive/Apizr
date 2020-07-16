using Apizr.Mediation.Cruding.Base;
using Fusillade;

namespace Apizr.Mediation.Cruding
{
    public class ReadQuery<TResponse, TKey> : ReadQueryBase<TResponse, TKey>
    {
        public ReadQuery(TKey key, Priority priority = Priority.UserInitiated) : base(key, priority)
        {
        }
    }

    public class ReadQuery<TResponse> : ReadQueryBase<TResponse>
    {
        public ReadQuery(int key, Priority priority = Priority.UserInitiated) : base(key, priority)
        {
        }
    }
}
