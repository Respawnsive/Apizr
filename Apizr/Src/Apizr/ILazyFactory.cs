namespace Apizr
{
    /// <summary>
    /// A lazy instance
    /// </summary>
    /// <typeparam name="TInstance">Type of your instance</typeparam>
    public interface ILazyFactory<out TInstance>
    {
        /// <summary>
        /// The lazy instance
        /// </summary>
        TInstance Value { get; }
    }
}
