using System;
using Fusillade;

namespace Apizr.Prioritizing
{
    /// <summary>
    /// Prioritization helper
    /// </summary>
    public class Prioritize
    {
        public static ILazyPrioritizedWebApi<T> For<T>(Priority priority, Func<T> valueFactory)
        {
            switch (priority)
            {
                case Priority.Speculative:
                    return new SpeculativeWebApi<T>(valueFactory);
                case Priority.Background:
                    return new BackgroundWebApi<T>(valueFactory);
                case Priority.UserInitiated:
                    return new UserInitiatedWebApi<T>(valueFactory);
                case Priority.Explicit:
                    throw new NotImplementedException();
                default:
                    return new UserInitiatedWebApi<T>(valueFactory);
            }
        }

        public static ILazyPrioritizedWebApi<T> For<T>(Priority priority, Func<object> valueFactory)
        {
            switch (priority)
            {
                case Priority.Speculative:
                    return new SpeculativeWebApi<T>(valueFactory);
                case Priority.Background:
                    return new BackgroundWebApi<T>(valueFactory);
                case Priority.UserInitiated:
                    return new UserInitiatedWebApi<T>(valueFactory);
                case Priority.Explicit:
                    throw new NotImplementedException();
                default:
                    return new UserInitiatedWebApi<T>(valueFactory);
            }
        }

        public static Type TypeFor<T>(Priority priority) => TypeFor(typeof(T), priority);

        public static Type TypeFor(Type webApiType, Priority priority)
        {
            switch (priority)
            {
                case Priority.Speculative:
                    return typeof(SpeculativeWebApi<>).MakeGenericType(webApiType);
                case Priority.Background:
                    return typeof(BackgroundWebApi<>).MakeGenericType(webApiType);
                case Priority.UserInitiated:
                    return typeof(UserInitiatedWebApi<>).MakeGenericType(webApiType);
                case Priority.Explicit:
                    throw new NotImplementedException();
                default:
                    return typeof(UserInitiatedWebApi<>).MakeGenericType(webApiType);
            }
        }
    }
}
