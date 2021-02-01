namespace Apizr
{
    /// <summary>
    /// A lazy and prioritized instance of your web api
    /// </summary>
    /// <typeparam name="TWebApi">Type of your web api</typeparam>
    public interface ILazyWebApi<out TWebApi>
    {
        /// <summary>
        /// The lazy prioritized instance of your web api
        /// </summary>
        TWebApi Value { get; }
    }
}
