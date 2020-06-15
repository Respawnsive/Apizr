using Fusillade;

namespace Apizr.Prioritizing
{
    /// <summary>
    /// A lazy and prioritized instance of your web api
    /// </summary>
    /// <typeparam name="TWebApi">Type of your web api</typeparam>
    public interface ILazyPrioritizedWebApi<out TWebApi>
    {
        /// <summary>
        /// The priority of execution
        /// </summary>
        Priority Priority { get; }

        /// <summary>
        /// The lazy prioritized instance of your web api
        /// </summary>
        TWebApi Value { get; }
    }
}
