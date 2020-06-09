using Fusillade;

namespace Apizr.Prioritizing
{
    public interface ILazyPrioritizedWebApi<T>
    {
        Priority Priority { get; }

        T Value { get; }
    }
}
