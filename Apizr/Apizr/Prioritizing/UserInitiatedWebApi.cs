using System;
using Fusillade;

namespace Apizr.Prioritizing
{
    public class UserInitiatedWebApi<T> : LazyPrioritizedWebApi<T>
    {
        public UserInitiatedWebApi(Func<T> valueFactory) : base(Priority.UserInitiated, valueFactory)
        {
        }

        public UserInitiatedWebApi(Func<object> valueFactory) : base(Priority.UserInitiated, valueFactory)
        {
        }
    }
}